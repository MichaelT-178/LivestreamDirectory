using Newtonsoft.Json;

/**
 * This class helps perform JSON operations on files.
 *
 * Methods
 * GetFilesJSONData | Gets JSON data from a list and stores it in a string list.
 * 
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

    


}