package db_manager.main_algorithm;

import java.awt.Desktop;
import java.io.IOException;
import java.net.URI;
import java.net.URISyntaxException;
import java.util.ArrayList;
import java.util.Collections;
import java.util.List;
import java.util.Scanner;
import org.json.*;


public class MainAlgorithm {

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

        JSONHelper jsonHelper = new JSONHelper();

                //../json_files/no_repeats.json
        List<String> noRepeats = jsonHelper.getFilesJSONData("./db_manager/json_files/no_repeats.json");
        List<String> onlyKeys = jsonHelper.getFilesJSONData("./db_manager/json_files/only_with_keys.json");
        List<String> artistsPlayed = jsonHelper.getFilesJSONData("./db_manager/json_files/artists.json");

        List<String> songs = new ArrayList<>();
        List<String> allSongs = new ArrayList<>();
        List<String> lowerSongs = new ArrayList<>();

        ErrorHandler errorHandler = new ErrorHandler();

        List<List<String>> updatedLists = errorHandler.findCapErrors(songs, allSongs, lowerSongs);

        songs = updatedLists.get(0);
        allSongs = updatedLists.get(1);
        lowerSongs = updatedLists.get(2);
        
        noRepeats = errorHandler.findNoRepeats(allSongs, noRepeats, onlyKeys);

        // System.out.println("DO YOU WANT TO CONTINUE");
        // String yeet = scanner.nextLine();
        
        onlyKeys = errorHandler.addToOnlyWithKeys(allSongs, noRepeats, onlyKeys);

        for (String song : songs) {
            errorHandler.manageStringInNoRepeats(song, noRepeats);
        }
        
        Algorithm.run(songs, noRepeats, onlyKeys, artistsPlayed, scanner);


        List<String> songList = jsonHelper.getSongList();

        //WHERE ARE LINE 456
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

        //LINE 470
        System.out.print("File ");
        printWithColor("successfully", Color.GREEN, "");
        System.out.println(" written to!");

        System.out.print("\nDo you want to open the \"song_list.json\" file? : ");
        String question = scanner.nextLine();
        System.out.println();

        if (question.strip().equalsIgnoreCase("Y") || question.strip().equalsIgnoreCase("YES")) {
            os.openApp("./database/song_list2.json", "Visual Studio Code");
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

        try {
            os.executeGitCommands();
        } catch (Exception e) {
            printWithColor("Git Commands failed", Color.RED, "\n");
        }

        scanner.close();
    }

    public static void printWithColor(String str, Color color, String newline) {
        String reset = "\u001B[0m";
        String colorCode = "\u001B[" + color.getCode() + "m";
        System.out.print(colorCode + str + reset + newline);
    }


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