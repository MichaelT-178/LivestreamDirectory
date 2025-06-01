using System.Text.Json;
using System.Text.RegularExpressions;
using JsonSerializer = System.Text.Json.JsonSerializer;

/**
 * This class creates the new JSON instrument.json file for the VueLivestreamDirectory.
 *
 * Methods
 * PopulateInstrumentMap | Populate the lists of the instrument map with songs.
 * WriteInstrumentMapToFile | Writes the instrumentMap to the InstrumentData.json file in VueLivestreamDirectory
 * UpdateInstrumentFiles | Update local and Vue instrument.json with new appearance attribute.
 * CreateInstrumentMap | Creates the skeleton of the instrument map.
 * GetKeysAsList | Gets a string list of keys from an appearance line.
 * GetInstruments | Get an Instrument list from instruments.json
 * GetInstrumentMapData | Returns a MapData object to be added to the instrumentMap
 * PrintInstrumentMap | Prints the instrument map
 * RemoveInstrumentKeys | Removes all the instrument keys from livestream appearances.
 * RemoveAllKeysFromSong | Removes all keys from the song (except keys_to_keeps)
 *
 * @author Michael Totaro
 */
class CreateNewInstrument
{
    
    /**
     * Populate the lists of the instrument map with songs. Works by looping
     * through database songs. Calls WriteInstrumentMapToFile at the end 
     * to update InstrumentData.json in Vue.
     * 
     * @return The fully populated instrument map.
     */
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

        WriteInstrumentMapToFile(instrumentMap);

        return instrumentMap;
        // PrintInstrumentMap(instrumentMap);
    }

    /**
     * Writes the instrumentMap to the InstrumentData.json file in 
     * VueLivestreamDirectory
     *
     * @param instrumentMap The populated instrumentMap
     * @return 
     * InstrumentSong
     */
    public static void WriteInstrumentMapToFile(Dictionary<string, List<InstrumentSong>> instrumentMap)
    {
        string filePath = "../VueLivestreamDirectory/src/assets/Data/InstrumentData.json";

        List<Instrument> instruments = GetInstruments();
        
        Dictionary<string, string> instrumentNameMap = instruments
            .Where(inst => !string.IsNullOrWhiteSpace(inst.CleanedName))
            .ToDictionary(inst => inst.CleanedName, inst => inst.Name);
            
        var formattedMap = new Dictionary<string, object>();
        
        foreach (var entry in instrumentMap)
        {
            string cleanedKey = entry.Key;
            string displayName = instrumentNameMap.ContainsKey(cleanedKey) ? instrumentNameMap[cleanedKey] : cleanedKey;
            
            // Create the value JSON object
            var formattedEntry = new
            {
                instrument = displayName,
                pic = cleanedKey + ".jpg",
                numOfAppearances = entry.Value.Count,
                appearances = entry.Value
            };
            
            formattedMap[cleanedKey] = formattedEntry;
        }
        
        var options = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };
        
        string jsonContent = JsonSerializer.Serialize(formattedMap, options);
        File.WriteAllText(filePath, jsonContent);

        UpdateInstrumentFiles(instrumentMap);

        OS.PushChangesInVue();
    }

    /**
     * Update local and Vue instrument.json with new appearance attribute.
     *
     * @param The instrumentMap that's data will be used to update the 
     *        appearance attribute of the two files.
     */
    public static void UpdateInstrumentFiles(Dictionary<string, List<InstrumentSong>> instrumentMap)
    {
        string localInstrumentPath = "./db_manager/json_files/instruments.json";
        string vueInstrumentPath = "../VueLivestreamDirectory/src/assets/Data/instruments.json";

        
        string jsonContent = File.ReadAllText(localInstrumentPath);

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        InstrumentWrapper wrapper = JsonSerializer.Deserialize<InstrumentWrapper>(jsonContent, options)!;

        foreach (var instrument in wrapper.Instruments)
        {
            if (!string.IsNullOrWhiteSpace(instrument.CleanedName))
            {
                instrument.Appears = instrumentMap.TryGetValue(instrument.CleanedName, out var list) ? list.Count : 0;
            }
        }

        // Save updated JSON
        var writeOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };

        string updatedJson = JsonSerializer.Serialize(wrapper, writeOptions);
        File.WriteAllText(localInstrumentPath, updatedJson);
        

        // If the the local and vue instruments.json aren't the same 
        // override the vue file with the local and push the changes
        // in Livestream directory.
        string vueJson = File.ReadAllText(vueInstrumentPath).Trim();
        string localJson = File.ReadAllText(localInstrumentPath).Trim();

        if (vueJson != localJson)
        {
            Console.WriteLine("NOT THE SAME");
            File.WriteAllText(vueInstrumentPath, localJson);
            Console.WriteLine("Vue JSON file has been updated.");
        }
    }

    
    /**
     * Creates the skeleton of the instrument map. CHANGE AND FILL THIS IN
     *
     * @return The skeleton of the instrument map. The key is a string, the value is an
     * InstrumentSong
     */
    public static Dictionary<string, List<InstrumentSong>> CreateInstrumentMap()
    {
        var instrumentMap = new Dictionary<string, List<InstrumentSong>>();
        List<Instrument> instruments = GetInstruments();
        
        foreach (var instrument in instruments)
        {
            if (!string.IsNullOrWhiteSpace(instrument.CleanedName))
            {
                instrumentMap[instrument.CleanedName] = new List<InstrumentSong>();
            }
        }

        return instrumentMap;
    }


    /**
     * Gets a string list of keys from an appearance line.
     *
     * Ex.
     * Solo Video (Electric Song/GPRG/BH) -> ["Electric Song", "GPRG", "BH"]
     * Livestream 23 (Audio Issues/Partial) -> ["Audio Issues", "Partial"]
     * Livestream 22 (Audio Issues) -> ["Audio Issues"]
     * Livestream 93 -> []
     *
     * @param input Appearance line with keys
     * @return Key list extracted from input line
     */
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
                    Console.WriteLine($"Trimmed: {trimmed}");
                }
            }
        }
        
        return result;
    }


    /**
     * Get all Instruments from instruments.json
     *
     * @return A list of instruments objects.
     */
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
    
    
    /**
     * Returns a MapData object with a cleaned instrument as 
     * a key and InstrumentSong as a value
     *
     * @param id The new id of the InstrumentSong
     * @param keyList List of keys in the 
     * @param song The song to be added
     * @param appearance The appearance (Ex. Livestream 61)
     * @param link Link to the performance
     * @return MapData which is the key and data of the song
     */
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

        string cleanedSongTitle = RemoveAllKeysFromSong(song.Title);
        string cleanedAppearance = RemoveInstrumentKeys(appearance);

        InstrumentSong instrumentSong = new(id, cleanedSongTitle, song.Artist, cleanedAppearance, link);
        
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
            else if (key == "12-String")
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


    /**
     * Prints the instrument map
     * 
     * @param The instrumentMap to print
     */
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
                    Console.WriteLine($"  - {instrumentSong.SongTitle}");
                }
            }
            
            Console.WriteLine();
        }
    }


    /**
     * Removes all the instrument keys from livestream appearances.
     * 
     * Ex.
     * Livestream 5 (Audio Issues/Electric riff) -> Livestream 5 (Audio Issues)
     * Livestream 28 (Mandolin) -> Livestream 28
     *
     * @param The Livestream line with all it's keys
     * @return The Livestream line without it's instruments keys
     */
     public static string RemoveInstrumentKeys(string line)
     {
        HashSet<string> REMOVE_KEYS = [
            "12-String", "BDC", "BH", "BHG", "BSG", "BSGI", "Blues Slide", "Classical Guitar",
            "DM75", "DX1R", "Electric Song", "Electric riff", "FBD", "FBG", "FG", "FOB", "FOSG",
            "FSD", "FV2", "FVD", "GFCHN", "GLTC", "GMLHB", "GMLN", "GMNC", "GPPCB", "GPRG", "GRSG",
            "GSDG", "LPE", "M15M", "MDT", "MFF", "MHD", "Mandolin", "NBBU", "NBDA", "NSCG", "NSPBU",
            "NST", "NSTCW", "OOM", "OOMV1", "SAS", "SD22", "SGI", "SOM"
        ];
        
        string result = Regex.Replace(line, @"\(([^()]+)\)", match =>
        {
            string content = match.Groups[1].Value;
            var parts = content.Split('/');
            
            var filtered = parts
                .Select(part => part.Trim())
                .Where(part => !REMOVE_KEYS.Contains(part))
                .ToList();
            
            return filtered.Count > 0 ? $"({string.Join("/", filtered)})" : "";
        });
        
        // Clean up extra spaces caused by removal
        return Regex.Replace(result, @"\s{2,}", " ").Trim();
    }



    /**
     * Removes all keys from the song (except keys_to_keeps)
     *
     * Ex. 
     * Voodoo Child (Slight Return) -> Voodoo Child (Slight Return)
     * In The Street (That 70's Show Theme) (Partial) -> In The Street (That 70's Show Theme)
     * Wagon Wheel (Partial) -> Wagon Wheel
     *
     * @param line A song title with keys
     * @return The song title without keys
     */
    public static string RemoveAllKeysFromSong(string line)
    {
        HashSet<string> songKeys = [.. JSONHelper.GetKeyListFromFile("./db_manager/json_files/keys_to_keep.json", "song_keys")];

        string result = Regex.Replace(line, @"\([^()]+\)", match =>
        {
            string fullMatch = match.Value; // Ex. "(Electric riff)"
            return songKeys.Contains(fullMatch) ? fullMatch : "";
        });

        return Regex.Replace(result, @"\s{2,}", " ").Trim();
    }

}