using System.Text.Json;
using SystemTextJsonSerializer = System.Text.Json.JsonSerializer;

/**
 * Handles the album.json and repertoire.json file.
 *
 * Methods
 * PrintAlbumJsonContents | Print the contents of albums.json
 * GetAlbums | Returns the list of albums from albums.json
 * GetRepertoire | Returns the song list from the repertoire.json file
 * SyncAlbumsWithRepertoire | Ensures every song has an associated album.json
 * UpdateRepertoireFile | Update the repertoire.json file given a list of songs
 *
 * @author Michael Totaro
 */
class AlbumRepertoireHandler
{
    /** Path to the album.json file */
    private const string albumJSONFilePath = "./db_manager/json_files/albums.json";

    /** Path to the repertoire.json file */
    private const string repertoireJSONFilePath = "./db_manager/json_files/repertoire.json";

    /**
     * Print the contents of the album.json file.
     */
    public static void PrintAlbumJsonContents()
    {
        try
        {
            string jsonContent = File.ReadAllText(albumJSONFilePath);
            var data = JsonSerializer.Deserialize<AlbumWrapper>(jsonContent)!;
            
            foreach (var album in data.albums)
            {
                Console.WriteLine($"ID: {album.id}");
                Console.WriteLine($"Song: {album.Song}");
                Console.WriteLine($"AlbumTitle: {album.AlbumTitle ?? "null"}");
                Console.WriteLine($"Year: {album.Year}");
                Console.WriteLine();
            }
        }
        catch (Exception ex)
        {
            Color.DisplayError($"Error reading JSON file: {ex.Message}");
        }
    }

    /**
     * Returns the list of albums from the albums.json file
     *
     * @return The List of albums from albums.json
     */
    public static List<Album> GetAlbums()
    {
        try
        {
            string jsonContent = File.ReadAllText(albumJSONFilePath);
            var data = JsonSerializer.Deserialize<AlbumWrapper>(jsonContent);
            return data?.albums ?? new List<Album>();
        }
        catch (Exception ex)
        {
            Color.DisplayError($"Error reading JSON file: {ex.Message}");
            return new List<Album>();
        }
    }

    /**
     * Returns the song list from the repertoire.json file.
     * With no blank strings/spaces.
     * 
     * @return The List of songs from repertoire.json
     */
     public static List<string> GetRepertoire()
     {
        try
        {
            string jsonContent = File.ReadAllText(repertoireJSONFilePath);
            var data = SystemTextJsonSerializer.Deserialize<List<string>>(jsonContent)!;
            
            return data
                    .Where(s => !string.IsNullOrWhiteSpace(s)) // Can't be a blank space
                    .Select(s => s.Substring(0, s.LastIndexOf(" by "))) // get song title
                    .ToList();

        }
        catch (Exception ex)
        {
            Color.DisplayError($"Error reading JSON file: {ex.Message}");
            return new List<string>();
        }
    }

    /**
     * Ensures every song has an associated album.
     * 
     * Prints all songs in albums.json not in repertoire.json.
     * Prints all songs in repertoire.json not in albums.json.
     */
    public static void SyncAlbumsWithRepertoire()
    {
        List<Album> albums = GetAlbums();
        List<string> repertoire = GetRepertoire();
        
        List<string> removeFromAlbums = new();
        List<string> addToAlbums = new();
        
        HashSet<string> albumSongs = albums.Select(a => a.Song).ToHashSet();
        HashSet<string> repertoireSongs = repertoire.ToHashSet();
        
        foreach (var song in albumSongs)
        {
            if (!repertoireSongs.Contains(song))
            {
                removeFromAlbums.Add(song);
            }
        }
        
        
        foreach (var song in repertoireSongs)
        {
            if (!albumSongs.Contains(song))
            {
                addToAlbums.Add(song);
            }
        }

        if (removeFromAlbums.Count == 0 && addToAlbums.Count == 0)
        {
            return;
        }

        if (removeFromAlbums.Count != 0)
        {
            Color.DisplayError("\n\n\nSongs to REMOVE from albums.json:");
            foreach (var song in removeFromAlbums)
            {
                Console.WriteLine($" - {song}");
            }
        }

        if (addToAlbums.Count != 0)
        {
            Color.PrintLine("\n\n\nSongs to ADD to albums.json:", "green");
            foreach (var song in addToAlbums)
            {
                Console.WriteLine($" + {song}");
            }
        }

        if (removeFromAlbums.Count > 0 || addToAlbums.Count > 0)
        {
            Console.WriteLine("\nGO FIX db_manager/json_files/albums.json!");
            Environment.Exit(0);
        }
    }

    /**
     * Update the repertoire.json file given a list of songs
     * 
     * @param songList The song list to write to the file.
     */
    public static void UpdateRepertoireFile(List<string> songList)
    {
        string filePath = "./db_manager/json_files/repertoire.json";

        string jsonString = SystemTextJsonSerializer.Serialize(songList, new JsonSerializerOptions
        {
            WriteIndented = true, 
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        });
        
        File.WriteAllText(filePath, jsonString);
    }

}