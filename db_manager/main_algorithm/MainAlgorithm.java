package db_manager.main_algorithm;

import java.awt.Desktop;
import java.io.IOException;
import java.net.URI;
import java.net.URISyntaxException;
import java.text.DecimalFormat;
import java.util.ArrayList;
import java.util.Collections;
import java.util.List;
import java.util.Scanner;
import org.json.*;

/**
 * Creates the song_list.json file out of the all-timestamps.txt file
 * @author Michael Totaro
 */
public class MainAlgorithm {

    /**
     * This algorithms main purpose is to check that the all-timestamps 
     * file doesn't have errors, create a list of json objects out of the 
     * songs out of the all-timestamps file to use as a database, and 
     * push the updated code to Github.
     * @param args Unused command line arguments 
     */
    public static void main(String[] args) {

        printWithColor("REMEBER TO ADD THE YOUTUBE LINK", Color.MAGENTA, "\n");

        Scanner scanner = new Scanner(System.in);
        System.out.print("Do you want to open the \"all-timestamps.txt\" file? : ");
        String openTimestamps = scanner.nextLine();

        OperatingSystem os = new OperatingSystem();

        if (openTimestamps.equalsIgnoreCase("Y") || openTimestamps.equalsIgnoreCase("Yes")) {
            os.openApp("./db_manager/timestamps/all-timestamps.txt", "Visual Studio Code");
            System.exit(0);
        }

        //Start counting program execution time.
        long startTime = System.currentTimeMillis();

        System.out.println("\nPlease be patient. This will take a couple of seconds...");
        System.out.println("Writing to file...");

        JSONHelper jsonHelper = new JSONHelper();

        //Load the data from the json files into the string lists.
        List<String> noRepeats = jsonHelper.getFilesJSONData("./db_manager/json_files/no_repeats.json");
        List<String> onlyKeys = jsonHelper.getFilesJSONData("./db_manager/json_files/only_with_keys.json");
        List<String> artistsPlayed = jsonHelper.getFilesJSONData("./db_manager/json_files/artists.json");

        List<String> songs = new ArrayList<>();
        List<String> allSongs = new ArrayList<>();

        ErrorHandler errorHandler = new ErrorHandler();

        //Make sure the all-timestamps file doesn't have capitalization errors.
        List<List<String>> updatedLists = errorHandler.findCapErrors(songs, allSongs);

        //List of all song titles in the file, with only one occurence of each song title.
        songs = updatedLists.get(0);
       
        //List of all songs titles in the file, with all/multiple occurences of the songs title.
        allSongs = updatedLists.get(1);
        
        //Find all songs that have been played once
        noRepeats = errorHandler.findNoRepeats(allSongs, noRepeats, onlyKeys);

        //All songs that have only been played with keys 
        onlyKeys = errorHandler.addToOnlyWithKeys(allSongs, noRepeats, onlyKeys);

        //Manage how the keys effect whether a song is in noRepeats or onlyKeys or something else.
        for (String song : songs) {
            errorHandler.manageStringInNoRepeats(noRepeats, song);
        }
        
        //Run main algorithm to make song_list.json out of the all-timstamps file.
        Algorithm.run(songs, noRepeats, onlyKeys, artistsPlayed, scanner);

        //List of all songs in the song_list.json file. Format title by artist. 
        //Example Blackbird by The Beatles. It's doesn't contain the full song object.
        List<String> songList = jsonHelper.getSongList();


        //Alphabetically sort all songs into a list. This list is used for reperoire.json
        String letter = "A";
        List<String> alphabetList = new ArrayList<>();

        Collections.sort(songList);

        for (String song : songList) {

            String firstChar = Character.toString(song.charAt(0));

            if (!firstChar.equalsIgnoreCase(letter) && alphabetList.size() > 0) {
                letter = firstChar.toUpperCase();
                alphabetList.add("");
            }

            String rSlash = "\\\"";
            alphabetList.add(song.replace("\"", rSlash));
        }

        jsonHelper.writeJSONToFile(alphabetList, "./db_manager/json_files/repertoire.json");

        System.out.print("File ");
        printWithColor("successfully", Color.GREEN, "");
        System.out.println(" written to!");

        //Calculate and print how long it took the program to execute.
        DecimalFormat decimalFormat = new DecimalFormat("#.##");

        long endTime = System.currentTimeMillis();
        long totalTimeMs = endTime - startTime;
        String totalTimeSeconds = decimalFormat.format(totalTimeMs * 0.001);
        System.out.println("Program took " + totalTimeSeconds + " seconds to run.");

        System.out.print("\nDo you want to open the \"song_list.json\" file? : ");
        String question = scanner.nextLine();
        System.out.println();

        //Open song_list.json to see/check changes if you want.
        if (question.strip().equalsIgnoreCase("Y") || question.strip().equalsIgnoreCase("YES")) {
            os.openApp("./database/song_list.json", "Visual Studio Code");
        }

        System.out.print("Do you want to open the Github Repository? : ");
        String openRepo = scanner.nextLine();

        if (openRepo.equalsIgnoreCase("Y") || openRepo.equalsIgnoreCase("YES")) {
            try {
                Desktop.getDesktop().browse(new URI("https://github.com/MichaelT-178/LivestreamDirectory"));
            } catch (IOException | URISyntaxException e) {
                e.printStackTrace();
            }
        }

        //Add the updates to GitHub.
        try {
            os.executeGitCommands();
            System.out.println();
        } catch (Exception e) {
            printWithColor("Git Commands failed", Color.RED, "\n");
        }

        scanner.close();
    }

    /**
     * Prints a string with a certain color
     * @param str String to be printed with color
     * @param color Color the string will be 
     * @param newline Whether you want to print with a newline of not. Either "\n" or ""
     */
    public static void printWithColor(String str, Color color, String newline) {
        String reset = "\u001B[0m";
        String colorCode = "\u001B[" + color.getCode() + "m";
        System.out.print(colorCode + str + reset + newline);
    }

    /**
     * Color codes that you can print strings with
     */
    enum Color {
        MAGENTA("35"),
        RED("31"),
        CYAN("36"),
        GREEN("32"); 

        private String colorCode;

        Color(String colorCode) {
            this.colorCode = colorCode;
        }
        
        public String getCode() {
            return colorCode;
        }
    }

}