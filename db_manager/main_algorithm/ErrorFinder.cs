using System.Text.RegularExpressions;
using Newtonsoft.Json;

/**
 * Finds formatting errors before running main algorithm 
 *
 * Methods
 * FindCapErrors | Find capitalization errors.
 * AllArtistPicturesExist | Ensures every artist has picture.
 * FindDuplicates | Find song duplicates in repertoire.json. 
 * CleanText | Get rid of parenthesis text in string
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

            if (songAndArtist != null) {
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
                        string artist = artists.Split('/')[0].Trim();

                        if (artist == "AC") artist = "AC/DC";
                        if (artist == "Yusuf") artist = "Yusuf / Cat Stevens";

                        if (!checkedArtists.Contains(artist))
                        {
                            checkedArtists.Add(artist);

                            artist = artist.Replace(".", "").Replace("'", "").Replace("/", ":");
                            artist = Helper.ReplaceNonAsciiChars(artist);

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
            if (originals.Count <= 1) {
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
}