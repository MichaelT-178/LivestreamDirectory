package db_manager.main_algorithm;

import java.io.FileReader;
import java.io.FileWriter;
import java.io.IOException;
import java.util.ArrayList;
import java.util.List;

import org.json.*;

public class JSONHelper {

    public String youtubeLinks(String videoId, String theTime) {
        String[] timeArr = theTime.split(":");
        int timeArrLength = timeArr.length;

        int[] t = new int[timeArrLength];

        for (int i = 0; i < timeArrLength; i++) {
            t[i] = Integer.parseInt(timeArr[i]);
        }

        int seconds = (timeArrLength == 2) ? (t[1] + (t[0] * 60)) : (t[2] + (t[1] * 60) + (t[0] * 3600));

        if (videoId.contains("share")) {
            return "https://www.youtube.com/live/" + videoId + "&t=" + seconds + " , ";
        }

        return "https://youtu.be/" + videoId + "?t=" + seconds + " , ";
        
    }

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

    public String formatList(List<String> theList) {
        String string = "[";

        for (String item : theList) {
            string += "\n\t\"" + item + "\",";
        }
        
        string = string.substring(0, string.length() - 1);
        string += "\n]";

        return string;
    }

    public List<String> removeFromNoRepeats(List<String> noRepeatsList, String item) {
        noRepeatsList.remove(item);
        writeJSONToFile(noRepeatsList, "./db_manager/json_files/no_repeats.json");
        return noRepeatsList;
    }

    public List<String> removeNoRepeatAddKeys(List<String> noRepeatsList, String item, List<String> onlyKeysList, String line) {

        removeFromNoRepeats(noRepeatsList, item);

        String[] keyList = line.split("%!");

        onlyKeysList.add(keyList[0]);
        onlyKeysList.add(keyList[1]);

        writeJSONToFile(onlyKeysList, "./db_manager/json_files/only_with_keys.json");

        return onlyKeysList;
    }

    public List<String> addNoRepeats(List<String> noRepeatsList, String item) {
        noRepeatsList.add(item);
        writeJSONToFile(noRepeatsList, "./db_manager/json_files/no_repeats.json");
        return noRepeatsList;
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

    public List<String> getSongList() {

        List<String> songList = new ArrayList<>();

        try {
            String filePath = "./database/song_list2.json";
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

    //Currently unused
    public List<Song> readSongListJSONData() {

        List<Song> songList = new ArrayList<>();

        try {
            String filePath = "./database/song_list2.json";
            FileReader fileReader = new FileReader(filePath);
            JSONTokener jsonTokener = new JSONTokener(fileReader);
            JSONObject jsonObject = new JSONObject(jsonTokener);
            JSONArray songsArray = jsonObject.getJSONArray("songs");

            for (int i = 0; i < songsArray.length(); i++) {
                JSONObject song = songsArray.getJSONObject(i);
        
                String title = song.getString("Title");
                String artist = song.getString("Artist");
                String appearances = song.getString("Appearances");
                String instruments = song.getString("Instruments");
                String image = song.getString("Image");
                String links = song.getString("Links");

                songList.add(new Song(title, artist, appearances, instruments, image, links));
            }
        
            fileReader.close();
        } catch (Exception e) {
            printRedError("PROBLEM READING JSON TO FILE", "");
            e.printStackTrace();
        }

        return songList;
    }

    private static void printRedError(String str, String newLine) {
        System.out.print("\u001B[31m" + str + "\u001B[0m" + newLine);
    }

}