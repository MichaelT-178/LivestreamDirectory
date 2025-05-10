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
    

    
    
    public static void PopulateInstrumentMap()
    {
        string vueInstrumentPath = "../VueLivestreamDirectory/src/assets/Instruments/instruments.json";
        string localInstrumentPath = "./db_manager/json_files/instruments.json";

        string jsonContent = File.ReadAllText(vueInstrumentPath);
        File.WriteAllText(localInstrumentPath, jsonContent);
        
        List<Song> songs = JSONHelper.GetDatabaseSongs();
        var instrumentMap = CreateInstrumentMap();

        foreach (var song in songs)
        {
            Console.WriteLine(song.CleanedTitle);
            string appearances = song.Appearances;

            string[] appearList = appearances.Split(",");
            
            
            foreach (string appear in appearList)
            {
                Console.WriteLine(song.Title);
                Console.Write(appear + " | ");
                List<string> keyList = AlgorithmHelper.GetAllKeysFromLines("", appear);
                
                Console.WriteLine(AlgorithmHelper.GetKeysJoinedAsString(keyList));

                instrumentMap["electric-guitar"].Add("Did this work");
            }
        }

        PrintInstrumentMap(instrumentMap);
    }
    
    
    public static Dictionary<string, List<string>> CreateInstrumentMap()
    {
        var instrumentMap = new Dictionary<string, List<string>>();
        List<Instrument> instruments = GetInstruments();
        
        // Create a key for each instrument in the dictionary
        foreach (var instrument in instruments)
        {
            if (!string.IsNullOrWhiteSpace(instrument.CleanedName))
            {
                instrumentMap[instrument.CleanedName] = new List<string>();
            }
        }

        
        return instrumentMap;
    }



    public static void PrintInstrumentMap(Dictionary<string, List<string>> instrumentMap)
    {
        foreach (var entry in instrumentMap)
        {
            Console.WriteLine($"Instrument: {entry.Key}");
            
            if (entry.Value.Count == 0)
            {
                Console.WriteLine("  (No performances listed)");
            }
            else
            {
                foreach (var performance in entry.Value)
                {
                    Console.WriteLine($"  - {performance}");
                }
            }
            
            Console.WriteLine(); // Extra line for readability
        }
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





// acoustic-guitar
// electric-guitar
// classical-guitar
// 12-string-guitar
// mandolin
// harmonica
// blues-slide
// brickhouse-demo
// furch-blue-gc-sa
// martin-dx1r
// maestro-double-top
// martin-00-15m
// norman-st68
// furch-oom-sr-db
// seagull-artist-studio
// sgi-avenir-cw20
// stonebridge-om35asr-db
// furch-vintage-2-rs-sr
// dearmond-m75
// furch-oom-vintage-1
// maestro-fan-fretted-singa-flamed-maple-adirondack
// godin-progression-plus-cherry-burst-rn
// godin-stadium-59-desert-green-rn
// godin-passion-rg-3-indigo-burst-rn
// godin-5th-ave-uptown-gt-ltd-trans-cream
// martin-hd-28
// furch-vintage-2-d-sr
// boucher-sg-52
// furch-blue-d-cm
// boucher-sg-52-i
// godin-rialto-jr-satina-gray-hg-q-discrete
// godin-metropolis-natural-cedar-eq
// godin-metropolis-ltd-natural-hg-eq
// godin-metropolis-ltd-havana-burst-hg-eq
// godin-fairmount-concert-hall-natural-hg-eq
// boucher-hg-56
// furch-om22tsw-c-db-sgi
// furch-g25cr-c
// furch-sgi-d22tsr
// furch-om34tsr-db-b2
// la-patrie-etude
// breedlove-discovery-concert-ce
// norman-st40-parlor-burnt-umber
// norman-b15-dark-almond
// norman-b20-burnt-umber
// norman-st40-cw-gt-presys
// norman-st68-cw
// stonebridge-d22sr





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