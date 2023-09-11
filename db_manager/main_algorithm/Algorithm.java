package db_manager.main_algorithm;

import java.io.FileInputStream;
import java.io.FileNotFoundException;
import java.io.IOException;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.List;
import java.util.Scanner;

public class Algorithm {

    public static void run(List<String> songs, List<String> noRepeats, List<String> onlyKeys, List<String> artistsPlayed, Scanner scanner) {
        StringBuilder songInfo = new StringBuilder();
        
        songInfo.append("{");
        songInfo.append("\n	\"songs\":[");

        JSONHelper jsonHelper = new JSONHelper();
        OperatingSystem os = new OperatingSystem();

        for (String song : songs) {

            String title = song.replace("’", "'").replace("‘", "'");
            String appearances = "";
            String instruments = "";
            String artist = "";
            String time = "";
            String links = "";

            List<String> theSongs = new ArrayList<>();
            List<String> theArtists = new ArrayList<>();

            FileInputStream fileInput;
            Scanner input = null;

            try {
                fileInput = new FileInputStream("../timestamps/all-timestamps.txt");
                input = new Scanner(fileInput);
            } catch (FileNotFoundException e) {
                printRedError("File could not be read. Algorithm.java line 40.", "\n");
                System.exit(0);
            }

            String lsNum = null;
            String link = null;
            String line;
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

                if (line.contains("https")) {
                    String[] splitLink = line.strip().split("/");
                    link = splitLink[splitLink.length - 1];
                }

                String[] splitLine = rsplit(line);

                if (splitLine != null) {

                    String[] timeSplit = line.split(" ");

                    String realTitle = splitLine[0].replace(timeSplit[0], "").strip();

                    if (!noRepeats.contains(title) && !onlyKeys.contains(title)) {
                        realTitle = realTitle.replace("(Electric riff)", "").replace("(Classical Guitar)", "").replace("(Mandolin)", "").replace("(Electric Song)", "").replace("(12/twelve String)","").replace("(Partial)", "");
                    }
                    
                    //LOOK AT THIS 
                    title = title.contains("Fugue") ? title.replace(" (Classical Guitar)", "") : title;
                    title = title.contains("1006a") ? title.replace(" (Classical Guitar)", "") : title;
                
                    if (title.strip().equalsIgnoreCase(realTitle.strip())) {
                        artist = (splitLine[1].length() > artist.length()) ? splitLine[1].strip() : artist;
                        

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
                        else {
                            appearances += !lsNum.toLowerCase().contains("solo video") ? ("Livestream " + lsNum + ",") : (lsNum.strip() + ",");
                        }

                        if (isNumeric(lsNum)) {
                            if (Integer.parseInt(lsNum) == 6 && artist.equals("Chris Whitley")) {
                                appearances = appearances.replace("Livestream 136,", "Livestream 136 (w/ Blues Slide),");
                            }
                        }

                        try {
                            links += jsonHelper.youtubeLinks(link, timeSplit[0]);
                        } catch (Exception e) {
                            printRedError("YOUTUBE LINK DIDN'T WORK RIGHT", "\n");
                        }             

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
                            !line.contains("Acoustic Guitar") && !line.contains("(H)") && 
                            !line.contains("Electric Riff Session #")) 
                        { 
                            instruments += "Acoustic Guitar, ";
                        }

                        if (line.contains("Forget Her")) instruments += "Electric Guitar, ";
                        if (title.contains("Blues Slide") || appearances.contains("Blues Slide")) instruments += " Blues Slide, ";

                    }
                }
            }

            input.close();

            StringBuilder other = new StringBuilder();

            //Line 312
            if (!appearances.strip().equals("")) {
                songInfo.append("\n\t\t{");

                title = title.replace("(I)", "").replace("(H)", "").strip();
                if (title.toLowerCase().contains("intro “out of the mist”")) title = "Intro “Out of the Mist”";

                if (title.contains("“") || title.contains("”")) other.append(title + ", ");

                String rSlash = "\\\"";
                title = title.replace("“", rSlash).replace("”", rSlash);

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

                songInfo.append(jsonStr("Other_Artists", otherArtists.substring(0, otherArtists.length() - 2).strip().replace("  ", " "), ","));

                if (title.contains("Machine Gun")) 
                    appearances = ErrorHandler.replaceNth(appearances, " (Electric Song)", "" , 2);

                if (title.contains("Led Boots"))
                    appearances = appearances.replace(" 50 (Electric Song)", " 50 (Electric riff)");

                songInfo.append(jsonStr("Appearances", appearances.substring(0, appearances.length() - 1), ","));

                
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

                //Line 363
                if (artist.equals("P!nk")) other.append("Pink, " );
                if (artist.contains("Red Hot Chili")) other.append("The Red Hot Chili Peppers, ");
                if (artist.contains("Young") && !artist.equals("Neil Young")) other.append("Neil Young, ");
                if (title.contains("Starbird")) other.append("Star bird, ");
                if (artist.contains("Allman")) other.append("The Allman Brothers, ");
                if (artist.contains("Nelly") || artist.contains("Flo Rida")) other.append("Rap, ");
                if (artist.equals("Extreme")) other.append("The Extreme, ");

                if (title.contains("Grey")) other.append(title.replace("Grey", "Gray"));
                if (artist.contains("Grey")) other.append(artist.contains("Grey"));

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

                if (artist.contains(".") || artist.contains("'") || artist.contains("’")) other.append(artist.strip().replace(".", "").replace("'", "").replace("’", "").replace("‘", "'") + ", ");


                if (artist.contains("É") || artist.contains("í") || artist.contains("é") || artist.contains("á") || artist.contains("ü")) {
                    other.append(artist.replace('É', 'E').replace('í', 'i').replace('é','e').replace('á','a').replace("ü", "u") + ", ");
                }

                String otherStr = other.toString();
                songInfo.append(jsonStr("Other", otherStr.substring(0, otherStr.length() - 2).replace("  ", " "), ","));

                String finalInstruments = instruments.strip().substring(0, instruments.strip().length() - 1);

                songInfo.append(jsonStr("Instruments", finalInstruments, ","));

                List<String> accentStrs = Arrays.asList("Édith Piaf","Agustín Barrios Mangoré","Beyoncé, JAY-Z","Francisco Tárrega");

                if (!isAscii(artist) && !accentStrs.contains(artist.strip())) {
                    MainAlgorithm.printWithColor("HAS ACCENT ", MainAlgorithm.Color.CYAN, "");
                    System.out.println(artist + ". Needs to be added to list manually!");
                }

                artist = artist.replace('É', 'E').replace('í', 'i').replace('é','e').replace('á','a').replace("ü", "u").replace("/", ":");
                

                if (!artistsPlayed.contains(artist.replace(":", "/").strip())) {
                    MainAlgorithm.printWithColor("\nNEW ARTIST", MainAlgorithm.Color.MAGENTA, "");
                    System.out.println(" \"" + artist + "\" written to file!");

                    artistsPlayed.add(artist.strip());

                    jsonHelper.writeJSONToFile(artistsPlayed, "../json_files/artists.json");

                    // pic_question = input("Do you want to exit the program to find a picture? : ")

                    System.out.print("Do you want to exit the program to find a picture? : ");
                    String picQuestion = scanner.nextLine();

                    if (picQuestion.equalsIgnoreCase("Y") || picQuestion.equalsIgnoreCase("YES")) {
                        System.exit(0);
                    }
                }


                String artistPic = artist.strip().replace(".", "").replace("'", "").replace("’", "").replace("‘", "'");

                String image = "../pics/" + artistPic + ".jpg";

                try {
                    os.chdir("../");
                } catch (IOException e) {
                    printRedError("DIDN'T CHANGE DIRECTORIES CORRECTLY LINE 302", "\n");
                    System.exit(0);
                }

                boolean fileExists = os.fileExists(image);

                if (!fileExists) {
                    printRedError("This artist needs a picture", "");
                    System.out.println(": " + artistPic);
                }  

                try {
                    os.chdir("../db_manager/main_algorithm");
                } catch (IOException e) {
                    printRedError("DIDN'T CHANGE DIRECTORIES CORRECTLY LINE 316", "\n");
                    System.exit(0);
                }

                songInfo.append(jsonStr("Image", image.substring(1), ","));
                songInfo.append(jsonStr("Links", links.substring(0, links.length() - 3), ""));
                songInfo.append("\n		},");

            }
        }

        songInfo.deleteCharAt(songInfo.length() - 1);
        songInfo.append("\n	]");
        songInfo.append("\n}");

        try {
            os.chdir("../");
        } catch (IOException e) {
            printRedError("DIDN'T CHANGE DIRECTORIES CORRECTLY LINE 316", "\n");
            System.exit(0);
        }

        jsonHelper.writeStringToFile(songInfo.toString(), "../database/song_list.json");

    }


    private static void printRedError(String str, String newLine) {
        System.out.print("\u001B[31m" + str + "\u001B[0m" + newLine);
    }

    /**
     * Splits a line into a string array by the last instance 
     * of the " by " substring. If can't be split returns null.
     * @param line The line to split 
     * @return An string array on the line split by the last 
     *         index of by. Returns null if " by " not in string.
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