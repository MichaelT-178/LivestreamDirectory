package db_manager.main_algorithm;

import java.io.File;
import java.io.FileNotFoundException;
import java.util.ArrayList;
import java.util.List;
import java.util.Scanner;

/**
 * This class helps with error handling at the 
 * beginning of the main algorithm
 * @author Michael Totaro
 */
public class ErrorHandler { 

    /**
     * Checks to make sure that the same song has the same consistent capitalization 
     * throughout the file to ensure that the string comparisons work as expected.
     * Ex. Make sure one instance of "Bring It on Home" by Led Zeppelin is not capitalized 
     * differently as "Bring It On Home". Also replaces certain charcters like " ’ " with " ' " to 
     * avoid songs being considered different due to containing different versions of the same 
     * character. 
     * @param songs List to be updated with only one occurence of every song title in the all-timestamps file
     *              If the song was played multiple times, the title will be in the list only once.
     *              Note that keys make the song titles be considered different. "Grace (Partial)" and grace wil 
     *              both be in the list. Same with allSongs
     * @param allSongs List to be updated with the title of every song in the all-timestamps file.
     *                 If the song was played multiple times, the title will be in the list multiple times.
     * @return A list of string lists. The first item is the updated "songs"list, the second item is the updated 
     *         allSongs list. There will only be two lists in the list.
     */
    public List<List<String>> findCapErrors(List<String> songs, List<String> allSongs) {
        File file;
        Scanner input = null;

        //The songs list but the strings are all lowercase 
        List<String> lowerSongs = new ArrayList<>();

        try {
            file = new File("./db_manager/timestamps/all-timestamps.txt");
            input = new Scanner(file);
        } catch (FileNotFoundException ex) {
            printRedError("This should never be throw. File missing", "\n");
        }

        boolean capErrorFound = false;
        int lineNum = 0;

        //Get rid of the "Control + g. Type 900 for stand alone Videos. Format: time title by artist"
        //Line technically contains the " by " substring which is why it's being read.
        input.nextLine();

        while (input.hasNextLine()) {
            String line = input.nextLine();

            String[] timeSplit = line.split(" ");
            line = line.replace(timeSplit[0], "");

            int i = line.lastIndexOf(" by ");
            String[] splitLine = null;

            if (i >= 0) {
                splitLine = new String[] { line.substring(0, i), line.substring(i + 4) }; //4 because " by " is 4 chars
            }


            if (splitLine != null) {

                if (!songs.contains(splitLine[0].strip().replace("’", "'").replace("’", "'").replace("‘", "'")) &&
                lowerSongs.contains(splitLine[0].strip().replace("’", "'").replace("’", "'").replace("‘", "'").toLowerCase())) 
                {
                    printRedError("CAPITALIZATION ERROR: ", "");
                    System.out.print("\"" + splitLine[0].strip().replace("’", "'").replace("‘", "'"));
                    System.out.println("\" on Line " + (lineNum + 1));
                    capErrorFound = true;
                }


                if (!songs.contains(splitLine[0].strip().replace("’", "'").replace("‘", "'"))) {
                    songs.add(splitLine[0].strip().replace("’", "'").replace("‘", "'"));
                    lowerSongs.add(splitLine[0].strip().replace("’", "'").replace("‘", "'").toLowerCase());
                }

                allSongs.add(splitLine[0].strip());

            }

            lineNum++;
        }

        //If capitalization error found the song with the error will be printed,
        //The program will stop and open the all_timestampes.txt file so the user
        //can manually fix the error.
        if (capErrorFound) {
            System.out.println("\nGo fix capitalization and rerun\n");

            OperatingSystem os = new OperatingSystem();
            os.openApp("./db_manager/timestamps/all-timestamps.txt", "Visual Studio Code");

            System.exit(0);
        }

        List<List<String>> listOfLists = new ArrayList<>();
        listOfLists.add(songs);
        listOfLists.add(allSongs);

        return listOfLists;
    }

    /**
     * Finds songs that have only been played once and add it to noRepeats.
     * @param allSongs List of all song titles in the all-timestampes folder
     * @param noRepeats List of song titles that are only played once and have keys in their title.
     * @param onlyKeys List of song titles that have only have titles with keys
     * @return The noRepeats list
     */
    public List<String> findNoRepeats(List<String> allSongs, List<String> noRepeats, List<String> onlyKeys) {
        List<String> matches = new ArrayList<>();

        //If the titles are the same without their keys, add them to matches.
        for (int i = 0; i < allSongs.size(); i++) {
            for (int j = i + 1; j < allSongs.size(); j++) {
                String s1 = allSongs.get(i).replace("(Electric riff)", "").replace("(Classical Guitar)", "").replace("(Mandolin)", "").replace("(Electric Song)", "").replace("(12/twelve String)","").replace("(Partial)", "");
                String s2 = allSongs.get(j).replace("(Electric riff)", "").replace("(Classical Guitar)", "").replace("(Mandolin)", "").replace("(Electric Song)", "").replace("(12/twelve String)","").replace("(Partial)", "");
        
                if (s1.strip().equals(s2.strip())) {
                    matches.add(s1.strip());
                }
            }
        }

        //If the song contains a key, get rid of it and add it to no repeats if it's not already
        //in matches, noRepeats, or onlykeys 
        for (String song : allSongs) {
            if (titleContainsKey(song)) {
        
                String theTemp = song.replace("(Electric riff)", "").replace("(Classical Guitar)", "").replace("(Mandolin)", "").replace("(Electric Song)", "").replace("(12/twelve String)","").replace("(Partial)", "");
                
                if (!matches.contains(theTemp.strip()) && !noRepeats.contains(song) && !onlyKeys.contains(song)) {
                    printRedError("\nAdd to No_Repeats", "");
                    System.out.println(": " + song.strip() + "\n");

                    JSONHelper jsonHelper = new JSONHelper();
                    noRepeats = jsonHelper.addNoRepeats(noRepeats, song.strip());
                }
            }
        }

        return noRepeats;
    }

    /**
     * Iterates over the noRepeats list and if a song is found to repeat more than once without 
     * it's different keys, than it is added to the onlyKeys list.
     * @param allSongs List of all Songs in the all-timestamps file.
     * @param noRepeats List of songs that are only played once and have keys in their title
     * @param onlyKeys List of songs that have only have titles with keys
     * @return The updated onlyKeys list 
     */
    public List<String> addToOnlyWithKeys(List<String> allSongs, List<String> noRepeats, List<String> onlyKeys) {
        List<String> theDuplicates = new ArrayList<>();

        for (String repeatSong : noRepeats) {
            for (String song : allSongs) {
                String song1 = song.replace("(Electric riff)", "").replace("(Classical Guitar)", "").replace("(Mandolin)", "").replace("(Electric Song)", "").replace("(12/twelve String)","").replace("(Partial)", "");
                String r1 = repeatSong.replace("(Electric riff)", "").replace("(Classical Guitar)", "").replace("(Mandolin)", "").replace("(Electric Song)", "").replace("(12/twelve String)","").replace("(Partial)", "");

                if (titleContainsKey(song)) {
                    if (r1.strip().equals(song1.strip())) {
                        if (theDuplicates.contains(repeatSong.strip())) {
                            String both = repeatSong.strip() + "%!" + song.strip();

                            printRedError("\nAdd to only_with_keys", "");
                            System.out.println(": " + repeatSong.strip());

                            printRedError("Add to only_with_keys", "");
                            System.out.println(": " + song.strip());

                            JSONHelper jh = new JSONHelper();
                            onlyKeys = jh.removeNoRepeatAddKeys(noRepeats, repeatSong.strip(), onlyKeys, both);
                        }

                        theDuplicates.add(repeatSong.strip());
                    }
                }
            }
        }
        
        return onlyKeys;
    }

    /**
     * If noRepeats contains the song title with a key, the song will be removed
     * from noRepeats.
     * @param noRepeats List of songs that have only been played once and have
     *        have keys in their title.
     * @param song The song to be determined if there was a repeat.
     */
    public void manageStringInNoRepeats(List<String> noRepeats, String song) {
        song = song.strip();

        String fullSong;
        JSONHelper jsonHelper = new JSONHelper();

        if (noRepeats.contains(song + " (Electric riff)")) {
            fullSong = song + " (Electric riff)";
            printRedError("\nRemove from no_repeats", "");
            System.out.println(": " + fullSong);
            noRepeats = jsonHelper.removeFromNoRepeats(noRepeats, fullSong);
        }
        else if (noRepeats.contains(song + " (Classical Guitar)")) {
            fullSong = song + " (Classical Guitar)";
            printRedError("\nRemove from no_repeats", "");
            System.out.println(": " + fullSong);
            noRepeats = jsonHelper.removeFromNoRepeats(noRepeats, fullSong);
        } 
        else if (noRepeats.contains(song + " (Mandolin)")) {
            fullSong = song + " (Mandolin)";
            printRedError("\nRemove from no_repeats", "");
            System.out.println(": " + fullSong);
            noRepeats = jsonHelper.removeFromNoRepeats(noRepeats, fullSong);
        }
        else if (noRepeats.contains(song + " (Electric Song)")) {
            fullSong = song + " (Electric Song)";
            printRedError("\nRemove from no_repeats", "");
            System.out.println(": " + fullSong);
            noRepeats = jsonHelper.removeFromNoRepeats(noRepeats, fullSong);
        }
        else if (noRepeats.contains(song + " (12/twelve String)")) {
            fullSong = song + " (12/twelve String)";
            printRedError("\nRemove from no_repeats", "");
            System.out.println(": " + fullSong);
            noRepeats = jsonHelper.removeFromNoRepeats(noRepeats, fullSong);
        }
        else if (noRepeats.contains(song.strip() + " (Partial)")) {
            fullSong = song + " (Partial)";
            printRedError("\nRemove from no_repeats", "");
            System.out.println(": " + fullSong);
            noRepeats = jsonHelper.removeFromNoRepeats(noRepeats, fullSong);
        } 
        else {
        }
    }

    /**
     * Replace the nth instance of a substring in a string. 
     * Example: String testStr = "blue water blue sky blue fish"
     * replace(testStr, "blue", "red", 3) would return this string 
     * "blue water blue sky red fish".
     * @param originalString The full string which contains the substring to replace.
     * @param oldStr The substring to be replaced.
     * @param newStr The string you want to replace the oldStr substring with.
     * @param occurrence The occurence of the oldStr you want to replace. Ex. 3rd occurence.
     * @return The originalString with the replaced substring.
     */
    public static String replaceNth(String originalString, String oldStr, String newStr, int occurrence) {
        int index = originalString.indexOf(oldStr);
        int count = 0;

        while (index != -1) {
            count++;
            if (count == occurrence) {
                originalString = originalString.substring(0, index) + newStr + originalString.substring(index + oldStr.length());
                break;
            }
            index = originalString.indexOf(oldStr, index + 1);
        }

        return originalString;
    }

    /**
     * Prints a string colored red. Can add a newline if you want.
     * @param str The string to be colored red.
     * @param newLine Blank if you dont want to add a newline to the string
     *                being printed, else the newline character "\n" as a string.
     */
    private static void printRedError(String str, String newLine) {
        System.out.print("\u001B[31m" + str + "\u001B[0m" + newLine);
    }

    /**
     * Check if a title contains any instrument keys
     * @param song Song title to be analyzed
     * @return True if the title contains any instrument keys else false.
     */
    private static boolean titleContainsKey(String song) {
        return (song.contains("(Electric riff)") || song.contains("(Classical Guitar)") ||
                song.contains("(Mandolin)") || song.contains("(Electric Song)") ||
                song.contains("(12/twelve String)") || song.contains("(Partial)"));
    }
}