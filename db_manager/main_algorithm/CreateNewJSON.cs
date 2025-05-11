using System.Text.Json;
using System.Text.RegularExpressions;
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


    public static Dictionary<string, List<InstrumentSong>> PopulateInstrumentMap()
    {
        List<Song> songs = JSONHelper.GetDatabaseSongs();
        var instrumentMap = CreateInstrumentMap();
        
        int instrumentSongId = 1;
        
        foreach (var song in songs)
        {
            string appearances = song.Appearances;
            string links = song.Links;

            string[] appearList = appearances.Split(",");
            string[] linkList = links.Split(",");

            for (int i = 0; i < appearList.Length; i++)
            {
                string appear = appearList[i];
                string link = linkList[i];

                List<string> keyList = GetKeysAsList(appear);

                List<MapData> mapDataList = GetInstrumentMapData(instrumentSongId, keyList, song, appear, link);

                foreach (var mapData in mapDataList)
                {
                    if (!instrumentMap.ContainsKey(mapData.InstrumentKey))
                    {
                        instrumentMap[mapData.InstrumentKey] = new List<InstrumentSong>();
                    }

                    instrumentMap[mapData.InstrumentKey].Add(mapData.InstrumentSong);
                }

                instrumentSongId++;
            }
        }

        return instrumentMap;
        // PrintInstrumentMap(instrumentMap);
    }



    public static List<MapData> GetInstrumentMapData(
        int id,
        List<string> keyList,
        Song song,
        string appearance,
        string link)
    {
        var result = new List<MapData>();
        string keyListStr = AlgorithmHelper.GetKeysJoinedAsString(keyList).Trim();
        link = link.Trim();

        // Console.Write("COOL | ");
        // Console.WriteLine(keyListStr);

        InstrumentSong instrumentSong = new(id, song.Title, song.Artist, appearance, link);
        
        if (
            string.IsNullOrWhiteSpace(keyListStr)
            || keyListStr == "(Partial)"
            || keyListStr == "(Audio Issues)"
            || keyListStr == "(Audio Issues/Partial)"
        )
        {
            result.Add(new MapData("acoustic-guitar", instrumentSong));
            return result;
        }
        
        foreach (var key in keyList)
        {
            if ((key == "Electric Song" || key == "Electric riff" || song.Title.Contains("Electric Riff Session")) &&
                !keyList.Contains("DM75") &&
                !keyList.Contains("GPPCB") &&
                !keyList.Contains("GSDG") &&
                !keyList.Contains("GPRG") &&
                !keyList.Contains("GLTC")
            )
            {
                result.Add(new MapData("electric-guitar", instrumentSong));
            }
            else if (key == "Classical Guitar" && !keyList.Contains("LPE"))
            {
                result.Add(new MapData("classical-guitar", instrumentSong));
            }
            else if (key == "12-String Guitar")
            {
                result.Add(new MapData("12-string-guitar", instrumentSong));
            }
            else if (key == "Mandolin")
            {
                result.Add(new MapData("mandolin", instrumentSong));
            }
            else if (key == "H")
            {
                result.Add(new MapData("harmonica", instrumentSong));
            }
            else if (key == "Blues Slide")
            {
                result.Add(new MapData("blues-slide", instrumentSong));
            }
            else if (key == "BH")
            {
                result.Add(new MapData("brickhouse-demo", instrumentSong));
            }
            else if (key == "FBG")
            {
                result.Add(new MapData("furch-blue-gc-sa", instrumentSong));
            }
            else if (key == "DX1R")
            {
                result.Add(new MapData("martin-dx1r", instrumentSong));
            }
            else if (key == "MDT")
            {
                result.Add(new MapData("maestro-double-top", instrumentSong));
            }
            else if (key == "M15M")
            {
                result.Add(new MapData("martin-00-15m", instrumentSong));
            }
            else if (key == "NST")
            {
                result.Add(new MapData("norman-st68", instrumentSong));
            }
            else if (key == "OOM")
            {
                result.Add(new MapData("furch-oom-sr-db", instrumentSong));
            }
            else if (key == "OOMV1")
            {
                result.Add(new MapData("furch-oom-vintage-1", instrumentSong));
            }
            else if (key == "SAS")
            {
                result.Add(new MapData("seagull-artist-studio", instrumentSong));
            }
            else if (key == "SGI")
            {
                result.Add(new MapData("sgi-avenir-cw20", instrumentSong));
            }
            else if (key == "SOM")
            {
                result.Add(new MapData("stonebridge-om35asr-db", instrumentSong));
            }
            else if (key == "FV2")
            {
                result.Add(new MapData("furch-vintage-2-rs-sr", instrumentSong));
            }
            else if (key == "DM75")
            {
                result.Add(new MapData("dearmond-m75", instrumentSong));
            }
            else if (key == "MFF")
            {
                result.Add(new MapData("maestro-fan-fretted-singa-flamed-maple-adirondack", instrumentSong));
            }
            else if (key == "GPPCB")
            {
                result.Add(new MapData("godin-progression-plus-cherry-burst-rn", instrumentSong));
            }
            else if (key == "GSDG")
            {
                result.Add(new MapData("godin-stadium-59-desert-green-rn", instrumentSong));
            }
            else if (key == "GPRG")
            {
                result.Add(new MapData("godin-passion-rg-3-indigo-burst-rn", instrumentSong));
            }
            else if (key == "GLTC")
            {
                result.Add(new MapData("godin-5th-ave-uptown-gt-ltd-trans-cream", instrumentSong));
            }
            else if (key == "MHD")
            {
                result.Add(new MapData("martin-hd-28", instrumentSong));
            }
            else if (key == "FVD")
            {
                result.Add(new MapData("furch-vintage-2-d-sr", instrumentSong));
            }
            else if (key == "BSG")
            {
                result.Add(new MapData("boucher-sg-52", instrumentSong));
            }
            else if (key == "FBD")
            {
                result.Add(new MapData("furch-blue-d-cm", instrumentSong));
            }
            else if (key == "BSGI")
            {
                result.Add(new MapData("boucher-sg-52-i", instrumentSong));
            }
            else if (key == "GRSG")
            {
                result.Add(new MapData("godin-rialto-jr-satina-gray-hg-q-discrete", instrumentSong));
            }
            else if (key == "GMNC")
            {
                result.Add(new MapData("godin-metropolis-natural-cedar-eq", instrumentSong));
            }
            else if (key == "GMLN")
            {
                result.Add(new MapData("godin-metropolis-ltd-natural-hg-eq", instrumentSong));
            }
            else if (key == "GMLHB")
            {
                result.Add(new MapData("godin-metropolis-ltd-havana-burst-hg-eq", instrumentSong));
            }
            else if (key == "GFCHN")
            {
                result.Add(new MapData("godin-fairmount-concert-hall-natural-hg-eq", instrumentSong));
            }
            else if (key == "BHG")
            {
                result.Add(new MapData("boucher-hg-56", instrumentSong));
            }
            else if (key == "FOSG")
            {
                result.Add(new MapData("furch-om22tsw-c-db-sgi", instrumentSong));
            }
            else if (key == "FG")
            {
                result.Add(new MapData("furch-g25cr-c", instrumentSong));
            }
            else if (key == "FSD")
            {
                result.Add(new MapData("furch-sgi-d22tsr", instrumentSong));
            }
            else if (key == "FOB")
            {
                result.Add(new MapData("furch-om34tsr-db-b2", instrumentSong));
            }
            else if (key == "LPE")
            {
                result.Add(new MapData("la-patrie-etude", instrumentSong));
            }
            else if (key == "BDC")
            {
                result.Add(new MapData("breedlove-discovery-concert-ce", instrumentSong));
            }
            else if (key == "NSPBU")
            {
                result.Add(new MapData("norman-st40-parlor-burnt-umber", instrumentSong));
            }
            else if (key == "NBDA")
            {
                result.Add(new MapData("norman-b15-dark-almond", instrumentSong));
            }
            else if (key == "NBBU")
            {
                result.Add(new MapData("norman-b20-burnt-umber", instrumentSong));
            }
            else if (key == "NSCG")
            {
                result.Add(new MapData("norman-st40-cw-gt-presys", instrumentSong));
            }
            else if (key == "NSTCW")
            {
                result.Add(new MapData("norman-st68-cw", instrumentSong));
            }
            else if (key == "SD22")
            {
                result.Add(new MapData("stonebridge-d22sr", instrumentSong));
            }
        }

        if (result.Count == 0)
        {
            result.Add(new MapData("acoustic-guitar", instrumentSong));
        }

        return result;
    }


    
    
    
    public static Dictionary<string, List<InstrumentSong>> CreateInstrumentMap()
    {
        var instrumentMap = new Dictionary<string, List<InstrumentSong>>();
        List<Instrument> instruments = GetInstruments();
        
        // Create a key for each instrument in the dictionary
        foreach (var instrument in instruments)
        {
            if (!string.IsNullOrWhiteSpace(instrument.CleanedName))
            {
                instrumentMap[instrument.CleanedName] = new List<InstrumentSong>();
            }
        }

        
        return instrumentMap;
    }



    public static void PrintInstrumentMap(Dictionary<string, List<InstrumentSong>> instrumentMap)
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
                foreach (var instrumentSong in entry.Value)
                {
                    Console.WriteLine($"  - {instrumentSong.SongTitle} | {instrumentSong.Livestream}");
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



    // Livestream 22 (Audio Issues) -> ["Audio Issues"]
    //Livestream 23 (Audio Issues/Partial) -> ["Audio Issues", "Partial"]
    public static List<string> GetKeysAsList(string input)
    {
        var result = new List<string>();
        string pattern = @"\(([^)]*?)\)"; // Match content inside the first set of parentheses

        var match = Regex.Match(input, pattern);
        
        if (match.Success)
        {
            string content = match.Groups[1].Value;
            var splitKeys = content.Split('/');
            
            foreach (string key in splitKeys)
            {
                string trimmed = key.Trim();
                
                if (!string.IsNullOrEmpty(trimmed))
                {
                    result.Add(trimmed);
                }
            }
        }
        
        return result;
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