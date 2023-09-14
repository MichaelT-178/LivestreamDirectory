package db_manager.main_algorithm;

import java.io.FileInputStream;
import java.io.FileNotFoundException;
import java.util.Arrays;
import java.util.List;
import java.util.Scanner;

/**
 * The algorithm that turns the all-timestamps.txt file into the 
 * song_list.json file.
 * @author Michael Totaro
 */
public class Algorithm {

    /**
     * The algorithm that turns the all-timestamps.txt file into the 
     * song_list.json file. Loops through all songs and builds a list of json 
     * song json objects with their attributes. 
     * @param songs List that contains only one occurence of every unique song title.
     * @param noRepeats List of song titles that don't repeat 
     * @param onlyKeys List of songs that only appear with keys in their title.
     * @param artistsPlayed List of artists played in the Livestreams 
     * @param scanner Scanner for user input
     */
    public static void run(List<String> songs, List<String> noRepeats, List<String> onlyKeys, List<String> artistsPlayed, Scanner scanner) {
        
        //The string of this builder will be what is written to song_list.json
        StringBuilder songInfo = new StringBuilder();
        
        songInfo.append("{");
        songInfo.append("\n	\"songs\":[");

        JSONHelper jsonHelper = new JSONHelper();
        OperatingSystem os = new OperatingSystem();

        //Loop through the songs and all-timestamps file. Create attributes 
        //out of the appearances, intruments, artists, links, etc.

        for (String song : songs) {

            String title = song.replace("’", "'").replace("‘", "'");
            String appearances = "";
            String instruments = "";
            String artist = "";
            String links = "";

            FileInputStream fileInput;
            Scanner input = null;

            try {
                fileInput = new FileInputStream("./db_manager/timestamps/all-timestamps.txt");
                input = new Scanner(fileInput);
            } catch (FileNotFoundException e) {
                printRedError("File could not be read. Algorithm.java line 40.", "\n");
                System.exit(0);
            }

            String lsNum = null;
            String link = null;
            String line;

            //Get rid of the "Control + g. Type 900 for stand alone Videos. Format: time title by artist"
            //Line technically contains the " by " substring which is why it's being read.
            input.nextLine(); 

            while (input.hasNextLine()) {
                line = input.nextLine();
                line = line.replace("’", "'").replace("‘", "'");

                if (line.contains("Livestream")) {
                    lsNum = line.strip().replace("Livestream ", "");
                }

                if (line.toLowerCase().contains("solo video")) {
                    lsNum = line.strip();
                }

                //If line is a link split the link and get the id
                if (line.contains("https")) {
                    String[] splitLink = line.strip().split("/");
                    link = splitLink[splitLink.length - 1];
                }

                //Split the line by the last occurence of " by "
                String[] splitLine = rsplit(line);

                if (splitLine != null) {

                    String[] timeSplit = line.split(" ");

                    String realTitle = splitLine[0].replace(timeSplit[0], "").strip();

                    //Remove keys for equality checking
                    if (!noRepeats.contains(title) && !onlyKeys.contains(title)) {
                        realTitle = realTitle.replace("(Electric riff)", "").replace("(Classical Guitar)", "").replace("(Mandolin)", "").replace("(Electric Song)", "").replace("(12/twelve String)","").replace("(Partial)", "");
                    }

                    if (title.contains("Fugue")) title = title.replace(" (Classical Guitar)", "");
                    if (title.contains("1006a")) title = title.replace(" (Classical Guitar)", "");

                    //If song equals the title of the song in the line of the all_timestamps file.
                    if (title.strip().equalsIgnoreCase(realTitle.strip())) {
                        
                        //If the artist is longer in one of the instances of the song in the all-timestamps file
                        //make the artist the longer string. Note the artist will be the first index of the artist
                        //string split by the "/" character.
                        artist = (splitLine[1].length() > artist.length()) ? splitLine[1].strip() : artist;

                        //Add keys to appearances based on song title keys
                        if (line.contains("(Electric riff)")) {
                            appearances += !lsNum.toLowerCase().contains("so") ? ("Livestream " + lsNum + " (Electric riff),") : (lsNum.strip() + " (Electric riff),");
                        }
                        else if (line.contains("(Electric Song)")) {
                            appearances += !lsNum.toLowerCase().contains("so") ? ("Livestream " + lsNum + " (Electric Song),") : (lsNum.strip() + " (Electric Song),");
                        }
                        else if (line.contains("(Classical Guitar)")) {
                            appearances += !lsNum.toLowerCase().contains("so") ? ("Livestream " + lsNum + " (Classical Guitar),") : (lsNum.strip() + " (Classical Guitar),");
                        }
                        else if (line.contains("(Mandolin)")) {
                            appearances += !lsNum.toLowerCase().contains("so") ? ("Livestream " + lsNum + " (Mandolin),") : (lsNum.strip() + " (Mandolin),");
                        } 
                        else if (line.contains("(Partial)")) {
                            appearances += !lsNum.toLowerCase().contains("so") ? ("Livestream " + lsNum + " (Partial),") : (lsNum.strip() + " (Partial),");
                        }

                        else {
                            appearances += !lsNum.toLowerCase().contains("solo video") ? ("Livestream " + lsNum + ",") : (lsNum.strip() + ",");
                        }

                        if (isNumeric(lsNum)) {
                            if (Integer.parseInt(lsNum) == 136 && artist.equals("Chris Whitley")) {
                                appearances = appearances.replace("Livestream 136,", "Livestream 136 (w/ Blues Slide),");
                            }
                        }

                        //Add timestamped youtube links to the links attribute
                        try {
                            links += jsonHelper.youtubeLinks(link, timeSplit[0]);
                        } catch (Exception e) {
                            printRedError("YOUTUBE LINK DIDN'T WORK RIGHT", "\n");
                        }             

                        //Based on the keys and substrings, add the corresponding instruments to the instrument attribute
                        if (line.contains("(Electric riff)") && instruments.equals("")) instruments += "Electric Guitar, ";
                        if (line.contains("(Electric riff)") && !instruments.contains("Electric Guitar")) instruments += " Electric Guitar, ";

                        if (line.contains("Electric Riff Session #") && instruments.equals("")) instruments += "Electric Guitar, ";
                        if (line.contains("Electric Riff Session #") && !instruments.contains("Electric Guitar")) instruments += " Electric Guitar, ";

                        if (line.contains("(Electric Song)") && instruments.equals("")) instruments += "Electric Guitar, ";
                        if (line.contains("(Electric Song)") && !instruments.contains("Electric Guitar")) instruments += " Electric Guitar, ";

                        if (line.contains("(Classical Guitar)") && instruments.equals("")) instruments += "Classical Guitar, ";
                        if (line.contains("(Classical Guitar)") && !instruments.contains("Classical Guitar")) instruments += " Classical Guitar, ";

                        if (line.contains("(Mandolin)") && instruments.equals("")) instruments += "Mandolin, ";
                        if (line.contains("(Mandolin)") && !instruments.contains("Mandolin")) instruments += " Mandolin, ";

                        if (line.contains("Rein") && instruments.equals("")) instruments += "Harmonica, ";
                        if (line.contains("Rein") && !instruments.contains("Harmonica")) instruments += " Harmonica, ";

                        if (!line.contains("(Electric riff)") && !line.contains("(Electric Song)") && 
                            !line.contains("(Classical Guitar)") && !line.contains("(Mandolin)") &&
                            !line.contains("(H)") && !line.contains("Electric Riff Session #") &&
                            !instruments.contains("Acoustic Guitar")) 
                        { 
                            instruments += "Acoustic Guitar, ";
                        }

                        if (line.contains("Forget Her")) instruments += "Electric Guitar, ";
                        if (title.contains("Blues Slide") || appearances.contains("Blues Slide")) instruments += " Blues Slide, ";

                    }
                }
            }

            input.close(); //close all-timestamps file

            //The other attribute is for kind of "ghost" search terms 
            //So the user can find songs not just based on title, artist,
            //or instrument. Tries to account for spelling differences/mistakes 
            //and the use of different kind of the same character.
            StringBuilder other = new StringBuilder();

            //Line 312
            if (!appearances.strip().equals("")) {
                songInfo.append("\n\t\t{");

                //(I) is to indicate that the version of "Wish You Were here"
                //is the incubus one and not pink floyd. (H) indicates that 
                //Fox Chase & Lost John doesn't have an acoustic guitar, just harmonica.
                title = title.replace("(I)", "").replace("(H)", "").strip();

                if (title.toLowerCase().contains("intro “out of the mist”")) title = "Intro “Out of the Mist”";

                if (title.contains("“") || title.contains("”")) other.append(title + ", ");

                String rSlash = "\\\"";
                title = title.replace("“", rSlash).replace("”", rSlash);

                //Add quotes to strings with a different kind of quote.
                if (!isAscii(title) && !title.contains("”")) {
                    title = title.replace("“", rSlash).replace("”", rSlash);
                }

                String finalTitle = title.replace("(Classical Guitar)", "").replace("(Mandolin)", "").replace("(Electric Song)", "").replace("(12/twelve String)","").strip();

                songInfo.append(jsonStr("Title", finalTitle, ","));
                
                String[] allArtists =  (!artist.contains("Yusuf") && !artist.contains(",") || artist.contains("Eurythmics") && !artist.contains("AC/DC")) ?
                                        artist.replace("/", ",").split(",") : artist.split("&&&");

                artist = allArtists[0];

                if (artist.contains("AC")) artist = "AC/DC";

                songInfo.append(jsonStr("Artist", artist, ","));

                String otherArtists = "";

                for (int i = 1; i < allArtists.length; i++) {
                    otherArtists += allArtists[i] + ", ";
                }

                otherArtists = artist.equals("AC/DC") ? "" : otherArtists;

                //Can only be one artist. Other artists are people who are also know for writing/covering a song
                //Ex. Both Bob Dylan's and Jimi Hendrix's version of "All Along the Watchtower" are famous. So I
                //inlcude both artists.
                String otherArtistStr = !otherArtists.equals("") ? otherArtists.substring(0, otherArtists.length() - 2).strip().replace("  ", " ") : "";
                
                songInfo.append(jsonStr("Other_Artists", otherArtistStr, ","));

                //Replace 2nd instance of (Electing Song) if title is Machine Gun 
                if (title.contains("Machine Gun")) 
                    appearances = ErrorHandler.replaceNth(appearances, " (Electric Song)", "" , 2);

                if (title.contains("Led Boots"))
                    appearances = appearances.replace(" 50 (Electric Song)", " 50 (Electric riff)");

                songInfo.append(jsonStr("Appearances", appearances.substring(0, appearances.length() - 1), ","));

                //If title contains accented non-ascii characters, append the title with 
                //ascii characters to other.
                if (!isAscii(title)) {
                    other.append(title.replace("É", "E").replace("í", "i").replace("é", "e")
                                      .replace("á","a").replace("à", "a").replace("Á", "A")
                                      .replace("ü", "u") + ", " );
                }


                if (title.contains("'") || title.contains("\"") || title.contains("&") || title.contains("-") || title.contains(",")) 
                {
                    other .append(title.replace("'","").replace(rSlash,"").replace(" & ", " and ")
                                       .replace("-", " ").replace(",", "").replace(".","") + ", ");
                }       
                
                if (title.contains("and")) other.append(title.replace(" and ", " & ") + ", ");

                if (title.contains("'") || title.contains(".")) { other.append(title.replace("'", "’").replace(".", "") + ", "); }

                if (title.toLowerCase().contains(" you "))
                    other.append(title.replace(" You ", " u ").replace(" you ", " u ").replace(",", "").replace("'", "") + ", ");
                
                if (artist.contains("-") || artist.contains(",")) { other.append(artist.replace("-", " ").replace(",", " ") + ", "); }

                //Append various spellings of artists, and titles to other, to make them easier 
                //to search for the user.
                if (artist.equals("P!nk")) other.append("Pink, " );
                if (artist.contains("Red Hot Chili")) other.append("The Red Hot Chili Peppers, ");
                if (artist.contains("Young") && !artist.equals("Neil Young")) other.append("Neil Young, ");
                if (title.contains("Starbird")) other.append("Star bird, ");
                if (artist.contains("Allman")) other.append("The Allman Brothers, ");
                if (artist.contains("Nelly") || artist.contains("Flo Rida")) other.append("Rap, ");
                if (artist.equals("Extreme")) other.append("The Extreme, ");

                if (title.contains("Grey")) other.append(title.replace("Grey", "Gray") + ", ");
                if (artist.contains("Grey")) other.append(artist.replace("Grey", "Gray") + ", ");

                if (title.contains("Man Of Constant Sorrow")) other.append("I Am A Man of Constant Sorrow, ");
                if (title.equals("Vincent")) other.append("Vincent (Starry, Starry Night), ");

                if (title.contains("Xmas")) other.append("Happy Christmas, ");
                if (title.contains("Xmas")) other.append("Merry Christmas, " );

                if (artist.contains("Simon & Gar")) other.append("Simon and Garfunkel, ");
                
                if (artist.contains("Bublé")) other.append("Bubble, ");
                if (artist.contains("Bublé")) other.append("Buble, ");
                if (artist.contains("Simon & Gar")) other.append("Paul Simon, ");
                if (artist.equals("AC/DC")) other.append("ACDC, ");
                if (artist.equals("Dire Straits")) other.append("The Dire Straits, ");
                if (artist.equals("Joe Walsh")) other.append("The Eagles, " );
                if (artist.equals("Elliott Smith")) other.append("Elliot , ");
                if (title.equals("Trouble So Hard")) other.append("Natural Blues by Moby, ");
                if (title.equals("Natural Blues")) other.append("Trouble So Hard by Vera Hall, ");

                if (artist.contains(".") || artist.contains("'") || artist.contains("’")) {
                    other.append(artist.strip().replace(".", "").replace("'", "").replace("’", "").replace("‘", "'") + ", ");
                }
                
                //If artist contains accented characters replace them with standard ascii characters.
                if (artist.contains("É") || artist.contains("í") || artist.contains("é") || artist.contains("á") || artist.contains("ü")) {
                    other.append(artist.replace('É', 'E').replace('í', 'i').replace('é','e').replace('á','a').replace("ü", "u") + ", ");
                }

                String otherStr = other.toString();
                otherStr = !otherStr.equals("") ? otherStr.substring(0, otherStr.length() - 2).replace("  ", " ") : "";
                songInfo.append(jsonStr("Other", otherStr, ","));

                String finalInstruments = instruments.strip().substring(0, instruments.strip().length() - 1);

                songInfo.append(jsonStr("Instruments", finalInstruments, ","));

                List<String> accentStrs = Arrays.asList("Édith Piaf", "Agustín Barrios Mangoré", "Beyoncé, JAY-Z", "Francisco Tárrega");
                
                //If artist has non-ascii chars and is not in Accentstrs print error message.
                if (!isAscii(artist) && !accentStrs.contains(artist.strip())) {
                    MainAlgorithm.printWithColor("HAS ACCENT ", MainAlgorithm.Color.CYAN, "");
                    System.out.println(artist + ". Needs to be added to list manually!");
                }

                artist = artist.replace('É', 'E').replace('í', 'i').replace('é','e').replace('á','a').replace("ü", "u").replace("/", ":");
                
                //If artist is not in artists played find a picture for artist and write the new artist to artists.json
                if (!artistsPlayed.contains(artist.replace(":", "/").strip())) {
                    MainAlgorithm.printWithColor("\nNEW ARTIST", MainAlgorithm.Color.MAGENTA, "");
                    System.out.println(" \"" + artist + "\" written to file!");

                    artistsPlayed.add(artist.strip());

                    jsonHelper.writeJSONToFile(artistsPlayed, "./db_manager/json_files/artists.json");

                    System.out.print("Do you want to exit the program to find a picture? : ");
                    String picQuestion = scanner.nextLine();

                    if (picQuestion.equalsIgnoreCase("Y") || picQuestion.equalsIgnoreCase("YES")) {
                        System.exit(0);
                    }
                }


                String artistPic = artist.strip().replace(".", "").replace("'", "").replace("’", "").replace("‘", "'");

                String image = "../LivestreamDirectory/pics/" + artistPic + ".jpg";

                //If the artist does not have a picture print error message.
                boolean fileExists = os.fileExists(image);

                if (!fileExists) {
                    printRedError("This artist needs a picture", "");
                    System.out.println(": " + artistPic);
                }  

                songInfo.append(jsonStr("Image", image.substring(1).replace("/LivestreamDirectory", ""), ","));
                songInfo.append(jsonStr("Links", links.substring(0, links.length() - 3), ""));
                songInfo.append("\n		},");

            }
        }

        songInfo.deleteCharAt(songInfo.length() - 1); //Delete last "," character
        songInfo.append("\n	]");
        songInfo.append("\n}");

        //Write the songInfo string to the song_list.json file.
        jsonHelper.writeStringToFile(songInfo.toString(), "./database/song_list2.json");

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
     * Splits a line into a string array by the last instance 
     * of the " by " substring. If can't be split returns null.
     * @param line The line to split 
     * @return An string array on the line split by the last 
     *         index of by. Returns null if " by " is not in 
     *         line at least twice.
     */
    private static String[] rsplit(String line) {

        int i = line.lastIndexOf(" by ");
        String[] splitLine = null;

        if (i >= 0) {
            splitLine = new String[] { line.substring(0, i), line.substring(i + 4) }; //4 because " by " is 4 chars
        }

        return splitLine;
    }

    /**
     * Checks if string is integer
     * @param str String to be checked
     * @return True if string is integer else false 
     */
    public static boolean isNumeric(String str) {
        try {
            Integer.parseInt(str);
            return true;
        } catch (NumberFormatException e) {
            return false;
        }
    }

    /**
     * If all characters in string are Ascii characters 
     * return true. Else false 
     * @param str String to be checked.
     * @return True if all chars are ascii else false.
     */
    public static boolean isAscii(String str) {

        for (int i = 0; i < str.length(); i++) {
            int codePoint = str.codePointAt(i);

            if (codePoint > 127) {
                return false; // Found a non-ASCII character
            }
        }

        return true; // All characters are ASCII
    }

    /**
     * 
     * @param name The name of the attribute
     * @param val The value the attribute gets 
     * @param comma Add comma to end of string if necessary might be blank string.
     * @return String formatted to be written to json data.
     */
    public static String jsonStr(String name, String val, String comma) {
        return "\n			\"" + name + "\": \"" + val + "\"" + comma;
    }
    
}