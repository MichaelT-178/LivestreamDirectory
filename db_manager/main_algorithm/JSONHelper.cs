using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using SystemTextJsonSerializer = System.Text.Json.JsonSerializer;

/**
 * This class helps perform JSON operations on files.
 *
 * Methods
 * GetFilesJSONData | Gets JSON data from a list and stores it in a string list.
 * GetJSONSongAsString | Gets the string of a song object to be added to database file.
 * GetDatabaseSongs | Gets a string list that contains every songs title and artist the database file.
 * WriteJSONToFile | Writes a JSON list to a file
 *
 * @author Michael Totaro
 */
class JSONHelper
{   
    /**
     * Gets the contents of a JSON file and returns it as 
     * a string list. 
     * File format: ["Item1", "Item2", "Item3"].
     * @param filePath The path of the file to be read.
     * @return A string list of the JSON data from the file 
     * @throws ArgumentException If the deserialized JSON object is null.
     */
    public static List<string> GetFilesJSONData(string filePath)
    {
        string json = File.ReadAllText(filePath);

        List<string> songs = JsonConvert.DeserializeObject<List<string>>(json) 
                             ?? throw new ArgumentException("Couldn't get JSON data!");
        return songs;
    }

    /**
     * Gets the string of a song object to be added to database file.
     * @param song The song to be added to the JSON file
     * @return The JSON string of the song object to be added to 
     *         the database.
     */
    public static string GetJSONSongAsString(Song song)
    {
        string objectString = "";
        
        //Space in json file
        string s = "			";

        objectString += "		{\n";
        objectString += $"{s}\"Title\": \"{song.Title}\",\n";
        objectString += $"{s}\"Artist\": \"{song.Artist}\",\n";
        objectString += $"{s}\"Other_Artists\": \"{song.OtherArtists}\",\n";
        objectString += $"{s}\"Appearances\": \"{song.Appearances}\",\n";
        objectString += $"{s}\"Instruments\": \"{song.Instruments}\",\n";
        objectString += $"{s}\"Image\": \"{song.Pic}\",\n";
        objectString += $"{s}\"Links\": \"{song.Links}\"\n";
        objectString += "		},\n";

        return objectString;

    }

    /**
     * Creates and returns a string list that contains every songs title and artist 
     * from the song_list.json database file. The format is *song.Title* by *song.Artist*
     * @return 
     * @throws ArgumentException If SongContainer cannot be created from file.
     * @throws ArgumentException If the Songs list couldn't be gotten from the SongContainer.
     */
    public static List<string> GetDatabaseSongs()
    {
        string jsonData = File.ReadAllText("./database/song_list.json");
        SongsContainer songsContainer = JsonConvert.DeserializeObject<SongsContainer>(jsonData)
                                        ?? throw new ArgumentException("Couldn't create SongContainer!");
                                        
        List<Song> databaseSongs = songsContainer.Songs 
                                   ?? throw new ArgumentException("Couldn't get database songs!");

        List<string> databaseSongInfo = new List<string>();

        
        foreach (Song song in databaseSongs)
        {
            string songAndArtist = $"{song.Title} by {song.Artist}";
            databaseSongInfo.Add(songAndArtist);
            Console.WriteLine(songAndArtist);
        }

        return databaseSongInfo;
    }

    public static void WriteJSONToFile(List<string> yourList)
    {
        string filePath = "/Users/michaeltotaro/add_to_ls/database_songs.json";

        // Serialize the List<string> to JSON format
        string jsonString = SystemTextJsonSerializer.Serialize(yourList, new System.Text.Json.JsonSerializerOptions
        {
            WriteIndented = true, // Makes the JSON output more readable with indentation
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        });

        // Write the JSON string to the file
        File.WriteAllText(filePath, jsonString);
    }

}