using Newtonsoft.Json;

/**
 * This class helps perform JSON operations on files.
 *
 * Methods
 * GetFilesJSONData | Gets JSON data from a list and stores it in a string list.
 * GetJSONSongAsString | Gets the string of a song object to be added to database file.
 *
 *
 *
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
    
}