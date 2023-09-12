package db_manager.main_algorithm;

import java.io.File;
import java.io.FileNotFoundException;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.List;
import java.util.Scanner;

public class ErrorHandler {

    public List<List<String>> findCapErrors(List<String> songs, List<String> allSongs, List<String> lowerSongs) {
        File file;
        Scanner input = null;

        try {
            //file = new File("../timestamps/all-timestamps.txt");
            file = new File("./db_manager/timestamps/all-timestamps.txt");
            input = new Scanner(file);
        } catch (FileNotFoundException ex) {
            printRedError("This should never be throw. File missing", "\n");
        }

        boolean capErrorFound = false;
        int lineNum = 0;

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

        if (capErrorFound) {
            System.out.println("\nGo fix capitalization and rerun\n");

            OperatingSystem os = new OperatingSystem();
            os.openApp("./db_manager/timestamps/all-timestamps.txt", "Visual Studio Code");

            System.exit(0);
        }

        List<List<String>> listOfLists = new ArrayList<>();
        listOfLists.add(songs);
        listOfLists.add(allSongs);
        listOfLists.add(lowerSongs);

        return listOfLists;
    }


    public List<String> findNoRepeats(List<String> allSongs, List<String> noRepeats, List<String> onlyKeys) {
        List<String> matches = new ArrayList<>();

        for (int i = 0; i < allSongs.size(); i++) {
            for (int j = i + 1; j < allSongs.size(); j++) {
                String s1 = allSongs.get(i).replace("(Electric riff)", "").replace("(Classical Guitar)", "").replace("(Mandolin)", "").replace("(Electric Song)", "").replace("(12/twelve String)","").replace("(Partial)", "");
                String s2 = allSongs.get(j).replace("(Electric riff)", "").replace("(Classical Guitar)", "").replace("(Mandolin)", "").replace("(Electric Song)", "").replace("(12/twelve String)","").replace("(Partial)", "");
        
                if (s1.strip().equals(s2.strip())) {
                    matches.add(s1.strip());
                }
            }
        }

        for (String song : allSongs) {
            if (titleContainsInstrument(song)) {
        
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

    public List<String> addToOnlyWithKeys(List<String> allSongs, List<String> noRepeats, List<String> onlyKeys) {
        List<String> theDuplicates = new ArrayList<>();

        for (String repeatSong : noRepeats) {
            for (String song : allSongs) {
                String song1 = song.replace("(Electric riff)", "").replace("(Classical Guitar)", "").replace("(Mandolin)", "").replace("(Electric Song)", "").replace("(12/twelve String)","").replace("(Partial)", "");
                String r1 = repeatSong.replace("(Electric riff)", "").replace("(Classical Guitar)", "").replace("(Mandolin)", "").replace("(Electric Song)", "").replace("(12/twelve String)","").replace("(Partial)", "");

                if (titleContainsInstrument(song)) {
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


    public void manageStringInNoRepeats(String song, List<String> noRepeats) {
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

    // public static String replaceNth(String fullStr, String oldStr, String newStr, int occurrence) {
    //     String[] arr = fullStr.split(oldStr, occurrence + 1);
    //     String part1 = String.join(oldStr, Arrays.copyOfRange(arr, 0, occurrence));
    //     String part2 = String.join(oldStr, Arrays.copyOfRange(arr, occurrence, arr.length));
    //     return part1 + newStr + part2;
    // }

    // public static String replaceNth(String fullStr, String oldStr, String newStr, int occurrence) {
    //     String[] arr = fullStr.split(oldStr, -1);
    
    //     if (occurrence < 0 || occurrence >= arr.length - 1) {
    //         // If occurrence is out of bounds, or there aren't enough occurrences, return the original string.
    //         return fullStr;
    //     }
    
    //     String part1 = String.join(oldStr, Arrays.copyOfRange(arr, 0, occurrence));
    //     String part2 = String.join(oldStr, Arrays.copyOfRange(arr, occurrence + 1, arr.length));
    
    //     return part1 + newStr + part2;
    // }
    
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

    private static void printRedError(String str, String newLine) {
        System.out.print("\u001B[31m" + str + "\u001B[0m" + newLine);
    }

    private static boolean titleContainsInstrument(String song) {
        return (song.contains("(Electric riff)") || song.contains("(Classical Guitar)") ||
                song.contains("(Mandolin)") || song.contains("(Electric Song)") ||
                song.contains("(12/twelve String)") || song.contains("(Partial)"));
    }
}