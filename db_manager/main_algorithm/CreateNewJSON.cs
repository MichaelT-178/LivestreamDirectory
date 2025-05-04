using System.Text.Json;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

/**
 * This class creates the new JSON file for the VueLivestreamDirectory.
 *
 * Methods
 * AddAlbumAttribute | Sets the "Album" attribute in song_list.json
 *
 * @author Michael Totaro
 */
class CreateNewJSON
{

    /**
     * Sets the "Album" attribute in song_list.json to the songs 
     * album based on the values from albums.json.
     */
    public static void AddAlbumAttribute()
    {
        List<Album> albums = AlbumRepertoireHandler.GetAlbums();
        List<Song> songs = JSONHelper.GetDatabaseSongs();
        
        foreach (var album in albums)
        {
            var matchingSong = songs.FirstOrDefault(song =>
                song.Title == album.Song &&
                (album.Artist == null || song.Artist == album.Artist)
            );
            
            if (matchingSong != null)
            {
                if (!string.IsNullOrWhiteSpace(album.AlbumTitle))
                {
                    string originalTitle = album.AlbumTitle!;
                    string cleanedTitle = TextCleaner.CleanText(originalTitle);
                    
                    matchingSong.Album = originalTitle;
                    matchingSong.AlbumImage = cleanedTitle + ".jpg";
                }
                else
                {
                    matchingSong.Album = album.AlbumTitle!;
                    matchingSong.AlbumImage = null!;
                }
            }
        }
        
        var updatedData = new { songs };
        string json = JsonConvert.SerializeObject(updatedData, Formatting.Indented);
        File.WriteAllText("./database/song_list.json", json);
    }
    
    
    
    
    
    public static void CountInstruments()
    {
        try
        {

            // Write contents of vueInstrumentPath file to 
            // localInstrumentPath file.
            string vueInstrumentPath = "../VueLivestreamDirectory/src/assets/Instruments/instruments.json";
            string localInstrumentPath = "./db_manager/json_files/instruments.json";

            if (File.Exists(vueInstrumentPath))
            {
                string jsonContent = File.ReadAllText(vueInstrumentPath);
                File.WriteAllText(localInstrumentPath, jsonContent);
            }
            else
            {
                Console.WriteLine("File not found at path: " + vueInstrumentPath);
            }

            List<Song> songs = JSONHelper.GetDatabaseSongs();

            Console.WriteLine("SONG TITLES HERE");

            foreach (var song in songs)
            {
                string appearances = song.Appearances;

                foreach (var appear in appearances.Split(","))
                {
                    int startIndex = appear.IndexOf('(');
                    
                    if (startIndex != -1)
                    {
                        string keyValues = appear.Substring(startIndex + 1, appear.Length - startIndex - 2);
                        List<string> keyValuesList = keyValues.Split('/', StringSplitOptions.RemoveEmptyEntries).ToList();

                        Console.WriteLine($"Inside2: {keyValues}");
                        foreach (var keyValue in keyValuesList)
                        {
                            Console.WriteLine($"Inside: {keyValue}");
                        }
                    } else {
                        Console.WriteLine(appear);
                    }
                }

                //Console.WriteLine(song.Appearances);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("An error occurred while reading the file: " + ex.Message);
        }
    }






        public static List<string> GetKeysToCount()
    {
        string localInstrumentPath = "./db_manager/json_files/instruments.json";

        string jsonContent = File.ReadAllText(localInstrumentPath);
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        InstrumentWrapper data = JsonSerializer.Deserialize<InstrumentWrapper>(jsonContent, options)!;

        List<string> keysToCount = new();

        foreach (var instrument in data.Instruments)
        {
            if (!string.IsNullOrWhiteSpace(instrument.Alias))
            {
                keysToCount.Add(instrument.Alias);
            }
            else
            {
                keysToCount.Add(instrument.Name);
            }
        }

        return keysToCount;
    }



    
}