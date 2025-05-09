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

                // Console.WriteLine(appearances);
                // Console.WriteLine("\n");

                string[] appearList = appearances.Split(",");

                foreach (string appear in appearList)
                {
                    Console.WriteLine(song.Title);
                    Console.Write(appear + " | ");
                    List<string> keyList = AlgorithmHelper.GetAllKeysFromLines("", appear);
                    
                    Console.WriteLine(AlgorithmHelper.GetKeysJoinedAsString(keyList));

                }

                Console.WriteLine("\n");


                //Console.WriteLine(song.Appearances);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("An error occurred while reading the file: " + ex.Message);
        }
    }



    public static string[]? AnalyzeTitle(string title, string appearance)
    {

        if (string.IsNullOrEmpty(appearance) || appearance == "(Audio Issues)")
        {
            return ["Cool", "Hello"];
        }



        // (Album Version) -> Album Version
        string trimmedAppearance = appearance.Substring(1, appearance.Length - 2);

        return null;
    }







    public static List<Instrument> GetInstruments()
    {
        string localInstrumentPath = "./db_manager/json_files/instruments.json";

        string jsonContent = File.ReadAllText(localInstrumentPath);
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        InstrumentWrapper data = JsonSerializer.Deserialize<InstrumentWrapper>(jsonContent, options)!;

        return data.Instruments;
    }


    //dotnet run | grep "M15M" | wc -l

    //keysToCount 
    // if it has an alias do NOT count it 
    // Count the beginning thing as a key -> (BH) -
    // Count whole thing if no (key) in front 
    // Electric riff, Electric Song, Classical Guitar
    //
    // 

    // public static List<string> GetKeysToCount()
    // {
    //     List<Instrument> instruments = GetInstruments();
        
    //     List<string> keys = instruments
    //         .Where(instr => !string.IsNullOrWhiteSpace(instr.key))
    //         .Select(instr => instr.key!)
    //         .ToList();
            
    //     return keys;
    // }


    
}