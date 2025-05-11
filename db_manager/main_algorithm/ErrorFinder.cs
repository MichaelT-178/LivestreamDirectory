using System.Text.RegularExpressions;
using Newtonsoft.Json;

/**
 * Finds formatting errors before running main algorithm 
 *
 * Methods
 * FindCapErrors | Find capitalization errors.
 * AllArtistPicturesExist | Ensures every artist has picture.
 * FindDuplicates | Find song duplicates in repertoire.json. 
 * CheckDuplicateTitlesWithDifferentArtists | Checks that a song doesn't have different artists.
 * CleanText | Get rid of parenthesis text in string
 * CheckKeysToKeep | Find new keys to add to keys_to_keep.json
 *
 * @author Michael Totaro
 */
class ErrorFinder
{   

    /** Static character array with one empty string */
    private static readonly char[] SpaceSeparator = [' '];


    /**
     * Makes sure that all songs with the same name
     * have the same capitalization in the all-timestamps.txt
     * file. Then returns a string list containing all songs 
     * once without their keys.
     * @return A string list that contains each song title once 
     *         without it's keys.
     */
    public static List<string> FindCapErrors()
    {
        string timestampsFilePath = "./db_manager/timestamps/all-timestamps.txt";

        //Guard clause. If invalid file path, throw exception
        if (!File.Exists(timestampsFilePath))
        {
            throw new FileNotFoundException("Timestamp file path is incorrect!");
        }
        
        string[] file = File.ReadAllLines(timestampsFilePath);
        
        List<string> songs = [];
        List<string> allSongs = [];
        List<string> lowerSongs = [];

        bool capErrorFound = false;
        int lineNum = 0;

        foreach (string line in file) 
        {   
            string[]? songAndArtist = Helper.GetSongAndArtist(line);

            if (songAndArtist != null) 
            {
                string song = songAndArtist[0];

                if (!Helper.ListContainsSongWithCorrectQuotes(songs, song)
                    && Helper.ListContainsSongWithCorrectQuotes(lowerSongs, song.ToLower()))
                {
                    Color.Print("CAPITALIZATION ERROR: ", "Red");
                    Console.Write("\"" + song.Trim().Replace("’", "'").Replace("‘", "'"));
                    Console.WriteLine("\" on Line " + (lineNum + 1));
                    capErrorFound = true;
                }

                if (!Helper.ListContainsSongWithCorrectQuotes(songs, song)) 
                {
                    songs.Add(Helper.ReplaceWithCorrectQuotes(song.Trim()));
                    lowerSongs.Add(Helper.ReplaceWithCorrectQuotes(song.Trim().ToLower()));
                }

                allSongs.Add(song.Trim());
            } //if statement ends 

            lineNum++;
        } //foreach loop ends 

        if (capErrorFound)
        {
            Console.WriteLine("\nGo fix capitalization and rerun\n");
            OS.OpenFileInVSCode("./db_manager/timestamps/all-timestamps.txt");
            Environment.Exit(0);
        }

        return songs;
    }

    /**
     * Checks that all image paths exist and every artist 
     * has a picture. If error occurs, message is printed 
     * and the program is exited.
     */
    public static void AllArtistPicturesExist()
    {
        string filePath = "./db_manager/timestamps/all-timestamps.txt";

        if (File.Exists(filePath))
        {
            using (StreamReader reader = new StreamReader(filePath))
            {
                List<string> checkedArtists = new List<string>();
                List<string> artistsWithOutImage = new List<string>();

                while (!reader.EndOfStream)
                {
                    string? line = reader.ReadLine();
                    string[]? songAndArtist = Helper.GetSongAndArtist(line ?? "");

                    if (songAndArtist != null)
                    {
                        string artists = songAndArtist[1];
                        string artist = artists.Split('+')[0].Trim();

                        if (!checkedArtists.Contains(artist))
                        {
                            checkedArtists.Add(artist);
                            
                            artist = TextCleaner.CleanText(artist);

                            string imagePath = "../LivestreamDirectory/pics/" + artist + ".jpg";

                            //Check if image exists 
                            if (!File.Exists(imagePath))
                            {
                                artistsWithOutImage.Add(artist);
                            }
                        } //checked artists contains ends
                    } //songAndArtist conditional ends 
                } //While reader has stream

                if (artistsWithOutImage.Count != 0)
                {
                    Console.WriteLine();

                    foreach (string artist in artistsWithOutImage)
                    {
                        Color.Print("Image not found", "Red");
                        Console.WriteLine($": {artist}");
                    }

                    Console.WriteLine("Go find images for these artists and rerun\n");
                    Environment.Exit(0);
                }
            } //Using StreamReader ends 
        } //Check file exists ends 
        else
        {
            Color.DisplayError("File not found: " + filePath);
            Environment.Exit(0);
        }
    }
    

    /**
     * Find song duplicates in repertoire.json. 
     * 
     * Example output:
     *
     * Cleaned form: bouree in e minor
     * - Bouree In E Minor (Audio Issues) by Johann Sebastian Bach
     * - Bouree in E Minor (Partial) by Johann Sebastian Bach
     *
     */
    public static void FindDuplicates()
    {

        string repertoirePath = "./db_manager/json_files/repertoire.json";

        string whitelistPath = "./db_manager/json_files/whitelist_duplicates.json";

        var repertoire = JsonConvert.DeserializeObject<List<string>>(File.ReadAllText(repertoirePath))!;

        var whitelistJson = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, List<string>>>>(File.ReadAllText(whitelistPath));
        var whitelistMap = new Dictionary<string, HashSet<string>>();

        if (whitelistJson != null && whitelistJson.TryGetValue("whitelist_duplicates", out var whitelistEntries))
        {
            foreach (var kvp in whitelistEntries)
            {
                whitelistMap[kvp.Key.ToLower()] = new HashSet<string>([.. kvp.Value.Select(s => s.ToLower())]);
            }
        }

        // Clean and map
        var cleanedMap = new Dictionary<string, List<string>>();

        foreach (var original in repertoire)
        {
            var cleaned = CleanText(original)?.Trim();

            if (string.IsNullOrWhiteSpace(cleaned))
            {
                continue;
            }

            var normalized = cleaned.ToLower();

            if (!cleanedMap.ContainsKey(normalized))
            {
                cleanedMap[normalized] = new List<string>();
            }

            cleanedMap[normalized].Add(original);
        }

        // Find duplicates
        var duplicates = new Dictionary<string, List<string>>();

        // NOTE: kvp means key value pair
        foreach (var kvp in cleanedMap)
        {
            var cleaned = kvp.Key;
            var originals = kvp.Value;

            // Skip if there's less than 2 instances of song in originals.
            if (originals.Count <= 1) 
            {
                continue;
            }

            // Make originals lowercase
            var lowercaseOriginals = new HashSet<string>(originals.Select(o => o.ToLower()));

            // Check if cleaned song title is in the whitelist 
            if (whitelistMap.TryGetValue(cleaned, out var whitelistOriginals))
            {
                if (lowercaseOriginals.SetEquals(whitelistOriginals))
                {
                    continue; // matches a whitelisted item
                }
            }

            duplicates[cleaned] = originals;
        }

        if (duplicates.Count > 0)
        {
            Console.WriteLine("");
            Color.DisplayError("Duplicates found (ignoring case):");

            foreach (var kvp in duplicates)
            {
                Console.WriteLine($"\nCleaned form: {kvp.Key}");

                foreach (var orig in kvp.Value)
                {
                    Console.WriteLine($"  - {orig}");
                }
            }

            Console.WriteLine("\n");
            Environment.Exit(0);
        }
    }
    
    
    /**
     * Checks that a song doesn't have different artists.
     *
     * Ex. this will cause an error because it's by Hendrix
     * Voodoo Child (Slight Return) by Jimi Hendrix
     * Voodoo Child (Slight Return) by Corey Heuvel
     *
     * Song titles will different artists should be marked like the following
     * using different functions (not this one)
     *
     * Wish You Were Here by Pink Floyd 
     * Wish You Were Here (I) by Incubus 
     */
    public static void CheckDuplicateTitlesWithDifferentArtists()
    {

        string filePath = "./db_manager/timestamps/all-timestamps.txt";

        Dictionary<string, HashSet<string>> titleToArtists = new();

        foreach (var line in File.ReadLines(filePath))
        {
            string trimmedLine = line.Trim();
            if (!trimmedLine.Contains(" by ")) continue;

            var songParts = trimmedLine.Split(new[] { " by " }, StringSplitOptions.None);
            if (songParts.Length != 2) continue;

            // Remove time at the beginning
            string titleRaw = songParts[0].Contains(" ")
                ? songParts[0].Substring(songParts[0].IndexOf(" ") + 1)
                : songParts[0];

            string artist = songParts[1].Split('+')[0].Trim();

            // Clean the title using your custom function
            string title = Helper.RemoveKeys(titleRaw).Trim();

            if (!titleToArtists.ContainsKey(title))
            {
                titleToArtists[title] = new HashSet<string>();
            }

            titleToArtists[title].Add(artist);
        }
        
        bool errorFound = false;

        // Print titles with multiple different artists
        foreach (var kvp in titleToArtists)
        {
            if (kvp.Value.Count > 1)
            {
                Console.WriteLine($"\"{kvp.Key}\" has multiple artists: {string.Join(", ", kvp.Value)}");
                errorFound = true;
            }
        }

        if (errorFound)
        {   
            Console.WriteLine();
            Color.DisplayError("GO FIX THE MULTIPLE DIFFERENT ARTIST!!!!");
            Environment.Exit(0);
        }
    }

    /**
     * Get rid of parenthesis text in string
     * @param text Text to be cleaned
     * @return String with text removed
     */
    private static string CleanText(string text)
    {

        // If entire song in parenthesis "(Nice Dream) by Radiohead" return.
        if (Regex.IsMatch(text, @"^\([^)]+\)\s+by\s+", RegexOptions.IgnoreCase))
        {
            return text;
        }

        // Removes any content within parenthesis
        // Ex. Solsbury Hill (Audio Issues) -> Solsbury Hill
        var noParens = Regex.Replace(text, @"\(([^)]*)\)", "");

        // Turns this "Solsbury    Hill     by   Peter Gabriel" into "Solsbury Hill by Peter Gabriel"
        noParens = string.Join(" ", noParens.Split(SpaceSeparator, StringSplitOptions.RemoveEmptyEntries));

        // Split into first song and artist 
        var songParts = Regex.Split(noParens, @"\bby\b", RegexOptions.IgnoreCase);

        if (songParts.Length <= 1)
        {
            return noParens;
        }

        return string.Join(" by", songParts.Take(songParts.Length - 1)).Trim();
    }



    /**
     * Check that there are no new keys to keep.
     *
     * Example if a new song has a valid part in parenthesis 
     * like -> Drops of Jupiter (Tell Me). ALWAYS add it to 
     * all_keys_from_song_lines and if necessary add it to song_keys
     * MANUALLY
     */
    public static void CheckKeysToKeep()
    {
        HashSet<string> uniqueKeys = new HashSet<string>();
        
        string[] lines = File.ReadAllLines("./db_manager/timestamps/all-timestamps.txt");
        
        foreach (string line in lines)
        {
            if (line.Contains("https") || !line.Contains(":") || line.Contains("Control + g") || !line.Contains(" by "))
            {
                continue;
            }
            
            MatchCollection matches = Regex.Matches(line, @"\((.*?)\)");
            
            foreach (Match match in matches)
            {
                string content = match.Groups[1].Value;
                string[] parts = content.Split('/');
                
                foreach (string part in parts)
                {
                    string cleaned = part.Trim();
                    
                    if (!string.IsNullOrEmpty(cleaned))
                    {
                        uniqueKeys.Add(cleaned);
                    }
                }
            }
        }
        
        List<string> newKeys = new List<string>();
        
        foreach (string key in uniqueKeys)
        {
            newKeys.Add("(" + key + ")");
        }
        
        newKeys.Sort();
        
        List<string> oldKeys = JSONHelper.GetKeyListFromFile("./db_manager/json_files/keys_to_keep.json", "all_keys_from_song_lines");
        
        List<string> missingFromNew = oldKeys.FindAll(k => !newKeys.Contains(k));
        List<string> missingFromOld = newKeys.FindAll(k => !oldKeys.Contains(k));
        
        if (missingFromNew.Count == 0 && missingFromOld.Count == 0)
        {
            return; // Lists are the same, do nothing
        }
        
        if (missingFromNew.Count > 0)
        {
            Console.WriteLine("Old keys not in new keys. Remove it from keys_to_keep if necessary!");
            Console.WriteLine("You really should NEVER see this. It means something changed in all-timestamps");

            foreach (string key in missingFromNew)
            {
                Console.WriteLine(key);
            }
        }
        
        if (missingFromOld.Count > 0)
        {
            Console.WriteLine("\nNew keys not in old keys. Go add it!");
            
            foreach (string key in missingFromOld)
            {
                Console.WriteLine(key);
            }
        }

        Color.PrintLine("\nGO UPDATE keys_to_keep.json!!!!!", "red");
        Color.PrintLine("ALWAYS \"update all_keys_from_song_lines\". Only update \"song_keys\" if necessary.", "CYAN");
        Environment.Exit(0);
    }

}