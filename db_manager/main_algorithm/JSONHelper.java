package db_manager.main_algorithm;

import java.io.FileReader;
import java.io.FileWriter;
import java.io.IOException;
import java.util.ArrayList;
import java.util.List;

import org.json.*;

/**
 * This class helps with json file processes
 * @author Michael Totaro
 */
public class JSONHelper {

    /**
     * Timestamps a video link
     * @param videoId The videos id
     * @param theTime The time the song is played
     * @return The video linked to the specific time
     */
    public String youtubeLinks(String videoId, String theTime) {
        String[] timeArr = theTime.split(":");
        int timeArrLength = timeArr.length;

        int[] t = new int[timeArrLength];

        for (int i = 0; i < timeArrLength; i++) {
            t[i] = Integer.parseInt(timeArr[i]);
        }

        int seconds = (timeArrLength == 2)  ?  (t[1] + (t[0] * 60))  :  (t[2] + (t[1] * 60) + (t[0] * 3600));
        
        return "https://youtu.be/" + videoId + "&t=" + seconds + " , ";
        
    }

    /**
     * Reads in JSON data from a file and converts it to a list.
     * Used for the files in the json_files folder.
     * @param filePath Path of the file
     * @return String list of the JSON data from the file.
     */
    public List<String> getFilesJSONData(String filePath) {

        List<String> artists = new ArrayList<>();

        try {

            FileReader fileReader = new FileReader(filePath);

            JSONTokener tokener = new JSONTokener(fileReader);
            JSONArray jsonArray = new JSONArray(tokener);

            for (int i = 0; i < jsonArray.length(); i++) {
                String artist = jsonArray.getString(i);
                artists.add(artist);
            }

            fileReader.close();
        } catch (Exception e) {
            e.printStackTrace();
        }

        return artists;

    }

    /**
     * Takes a string list and formats it into JSON data, an
     * example of the format would be artists.json
     * @param theList The list to be converted to a string
     * @return The lists data as a json format string.
     */
    public String formatList(List<String> theList) {
        String string = "[";

        for (String item : theList) {
            string += "\n\t\"" + item + "\",";
        }
        
        string = string.substring(0, string.length() - 1);
        string += "\n]";

        return string;
    }

    /**
     * Removes a song from noRepeats, updates the no_repeats file, and 
     * returns the updated noRepeatsList
     * @param noRepeatsList The noRepeats list
     * @param item The song to be removed from no repeats
     * @return The updated noRepeatsList
     */
    public List<String> removeFromNoRepeats(List<String> noRepeatsList, String item) {
        noRepeatsList.remove(item);
        writeJSONToFile(noRepeatsList, "./db_manager/json_files/no_repeats.json");
        return noRepeatsList;
    }

    /**
     * Adds a song to noRepeats, updates the no_repeats file, and 
     * returns the updated noRepeatsList
     * @param noRepeatsList The noRepeats list
     * @param item The song to be added to no repeats
     * @return The updated noRepeatsList
     */
    public List<String> addNoRepeats(List<String> noRepeatsList, String item) {
        noRepeatsList.add(item);
        writeJSONToFile(noRepeatsList, "./db_manager/json_files/no_repeats.json");
        return noRepeatsList;
    }

    /**
     * Removes a song from the no_repeats list, then adds it to the onlyKeys list. This method
     * updates both the no_repeats.json file and the only_with_keys.json file. Returns the updated
     * onlyKeysList when done. If the song is not repeated it goes in the noRepeats list, if the song 
     * is only repeated with keys (Ex. (Electric riff), (Partial), etc.) it goes in onlyKeys
     * @param noRepeatsList The no_repeats list.
     * @param item The Song to be removed from no repeats 
     * @param onlyKeysList The only keys list to be updated.
     * @param line Line that contains songs with keys seperated by the substring %! to split them
     * @return The updated only keys list.
     */
    public List<String> removeNoRepeatAddKeys(List<String> noRepeatsList, String item, List<String> onlyKeysList, String line) {

        removeFromNoRepeats(noRepeatsList, item);

        String[] keyList = line.split("%!");

        onlyKeysList.add(keyList[0]);
        onlyKeysList.add(keyList[1]);

        writeJSONToFile(onlyKeysList, "./db_manager/json_files/only_with_keys.json");

        return onlyKeysList;
    }

    /**
     * Write a json list to a file. Not used for song_list.json
     * @param list List to be written to file
     * @param path Path of the file to be wrtitten to
     */
    public void writeJSONToFile(List<String> list, String path) {
        try {
            FileWriter writer = new FileWriter(path);
            writer.write(formatList(list));
            writer.close();
        } catch (IOException e) {
            printRedError("PROBLEM WRITING JSON TO FILE", "");
            System.out.println(" " + e.getMessage());
        }
    }

    /**
     * Write a string a file. Used of song_list.json
     * @param str String to be written to file.
     * @param path Path of the file to be wrtitten to
     */
    public void writeStringToFile(String str, String path) {
        try {
            FileWriter writer = new FileWriter(path);
            writer.write(str);
            writer.close();
        } catch (IOException e) {
            printRedError("PROBLEM WRITING JSON TO FILE", "");
            System.out.println(" " + e.getMessage());
        }
    }
    
    /**
     * Creates a string list of every song in the database with the 
     * format *Title* by *Artist* (Ex. Grace by Jeff Buckley)
     * @return The stirng list of the formatted song data
     */
    public List<String> getSongList() {

        List<String> songList = new ArrayList<>();

        try {
            String filePath = "./database/song_list.json";
            FileReader fileReader = new FileReader(filePath);
            JSONTokener jsonTokener = new JSONTokener(fileReader);
            JSONObject jsonObject = new JSONObject(jsonTokener);
            JSONArray songsArray = jsonObject.getJSONArray("songs");

            for (int i = 0; i < songsArray.length(); i++) {
                JSONObject song = songsArray.getJSONObject(i);
                songList.add(song.getString("Title") + " by " + song.getString("Artist"));
            }
        
            fileReader.close();
        } catch (Exception e) {
            printRedError("PROBLEM READING JSON TO FILE", "");
            e.printStackTrace();
        }

        return songList;
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

}