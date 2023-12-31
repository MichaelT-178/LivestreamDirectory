using System.Text.RegularExpressions;

/**
 *
 * This class helps perform functions in the main algorithm.
 *
 * Methods
 * ReplaceWithCorrectQuotes | Replaces stylized quotes with standard ASCII quotes.
 * ExtractKeysFromLines | Gets all the keys from the appearances as string list.
 * GetJoinedKeysAsString | Takes a key list and joins them as a string seperated by '/' characters.
 * GetSongTitleKeyListAsString | Gets a key from the keyList that can be used in a song title.
 * GetYouTubeLink | Gets a timestamped youtube link.
 * GetArtistPic | Gets the artists picture file path as a valid string.
 * GetOtherArtistsAsString | Joins the other artists as a string by commas.
 * AddDefaultAcousticGuitar | Adds acoustic guitar to instruments if necessary
 * GetInstrumentsFromSong | Gets the instrument from a song title based on it's keys.
 * GetJSONSongAsString | Gets the string of a song object to be added to database file.
 * RemoveKeys | Removes all keys from the line
 *
 * @author Michael Totaro
 */
class AlgorithmHelper
{   
    /** Path to the keys_to_keep file*/
    private static string keysToKeepPath = "./db_manager/json_files/keys_to_keep.json";
    
    /** String List of all the keys to not remove */
    private static List<string> keysToKeep = JSONHelper.GetFilesJSONData(keysToKeepPath);

    /**
     * Replaces stylized quotes with standard ASCII quotes.
     * Purpose is to make equality checks more accurate.
     * @param line The line to be modified.
     * @return line with correct quotation chars 
     */
    public static string ReplaceWithCorrectQuotes(string line)
    {
        return line.Replace("’", "'").Replace("‘", "'").Replace("“", "\"").Replace("”", "\"");
    }

    /**
     * Gets all the keys from the current livestream appearance 
     * and songs returns them in one string list. 
     * 
     * @param currLs The current livestream or solo video appearance 
     * @param song The song title which has keys.
     * @return A string list of the keys for 
     */
    public static List<string> ExtractKeysFromLines(string currLs, string song)
        {
        string pattern = @"\((.*?)\)";
        Regex regex = new Regex(pattern);

        List<string> keysFromCurrLs = new List<string>();
        List<string> keysFromSong = new List<string>();

        foreach (Match match in regex.Matches(currLs))
        {
            keysFromCurrLs.Add(match.Groups[1].Value);
        }

        foreach (Match match in regex.Matches(song))
        {
            keysFromSong.Add(match.Groups[1].Value);
        }

        List<string> resultKeys = new List<string>();

        foreach (string key in keysFromCurrLs.Concat(keysFromSong))
        {
            string formattedKey = $"({key})";

            if (!keysToKeep.Contains(formattedKey))
            {
                resultKeys.Add(formattedKey);
            }
        }

        return resultKeys;
    }


    /**
     * Takes a key list and joins them as a string seperated by '/' characters.
     * Example: ["(Cool)", "(Yeet)", "(Versace)"] becomes "(Cool/Yeet/Versace)".
     * @param keyList List of keys to be joined.
     * @return The key list as a joined string or a blank string if there were 
     *         no keys.
     */
    public static string GetJoinedKeysAsString(List<string> keyList)
    {   
        // Guard clause. If list is empty, just return empty string.
        if (keyList.Count == 0) return "";

        string resultString = "";

        foreach (string key in keyList)
        {
            string keyNoParenthesis = key.Replace("(", "").Replace(")", "");
            resultString += keyNoParenthesis + "/";
        }

        //[..^1] removes the last "/" character from resultString. Add parenthesis around string
        return "(" + resultString[..^1] + ")";

    }

    /**
     * Gets a key from the keyList that can be used in a song title. These are 
     * keys that will be shown in the search result title when using the search bar.
     * Example: Back In Black (Electric riff). Meant to show it's not full song.
     * @param keyList. List of keys from the line in all-timestamps.txt
     * @return 
     */
    public static string GetSongTitleKeyListAsString(List<string> keyList)
    {
        if (keyList.Count == 0) return "";

        List<string> validKeys = ["(Rein Rutnik Performance)", "(Electric riff)", "(Partial)"];

        foreach (string key in keyList)
        {
            if (validKeys.Contains(key))
            {
                return key;
            }
        }

        return "";
    }

    /**
     * Gets a timestamped youtube link given a link and a time.
     * @param link The youtube link to be timestamped.
     * @param time The timestamp of the link. Ex: "1:25:31"
     * @return The timestamped youtube link
     */
    public static string GetYouTubeLink(string link, string time)
    {
        string[] splitLink = link.Trim().Split("/");
        string videoId = splitLink[splitLink.Length - 1];

        //Converts 5:47 to [5, 47] and 1:18:36 to [1, 18, 36]
        int[] t = time.Split(':').Select(int.Parse).ToArray();

        //Converts [5, 47] to 347 and [1, 18, 36] to 4716.
        int seconds = (t.Length == 2) ? t[1] + (t[0] * 60) : t[2] + (t[1] * 60) + (t[0] * 3600);

        return $"https://youtu.be/{videoId}&t={seconds} , ";
    }
    
    /**
     * Gets the artists picture file path as a valid string. 
     * The "." and "'" chars can't appear in the path. 
     * @param artist Artist whose picture 
     * @return The filepath for the artists image as a string.
     */
    public static string GetArtistPic(string artist)
    {
        return artist.Replace(".", "").Replace("'", "").Trim() + ".jpg";
    }

    /**
     * Joins the other artists as a string by commas.
     * @param otherArtists A string list containing the names of the other artists.
     * @return A string of the other artists joined by commas.
     */
    public static string GetOtherArtistsAsString(List<string> otherArtists)
    {
        return string.Join(", ", otherArtists);
    }

    /**
     * If the songWithKeys doesn't contain any instrument keys and 
     * the currentInstruments attribute doesn't contain an acoustic 
     * guitar, add it by default. If the currentInstruments attribute 
     * does have instrument keys return a blank string.
     * @param songWithKeys is the song title with keys
     * @param currentInstruments is the currentInstruments string that contains instrument keys.
     * @return Acoustic guitar string if condition met, else a blank string
     */
    public static string AddDefaultAcousticGuitar(string songWithKeys, string currentInstruments)
    {
        if (
            !songWithKeys.Contains("(Electric riff)") &&
            !songWithKeys.Contains("(Electric Song)") &&
            !songWithKeys.Contains("(Classical Guitar)") &&
            !songWithKeys.Contains("(Mandolin)") &&
            !songWithKeys.Contains("(H)") &&
            !songWithKeys.Contains("Electric Riff Session #") &&
            !currentInstruments.Contains("Acoustic Guitar")
        ) 
        {
            return "Acoustic Guitar, ";
        }
    
        return "";
    }

    /**
     * Gets the instrument from a song title based on it's keys, which will be
     * added to the currentInstruments attribute if it's not already in it. 
     * The currentInstruments attribute will become the Instruments attribute 
     * of the song objects in the database.
     * @param songWithKeys The song with it's keys 
     * @param currentInstruments The attribute which contains all the songs 
     *        instruments so far.
     * @return The instrument string based on the key with comma. Ex: "Classical Guitar, "
     */
    public static string GetInstrumentsFromSong(string songWithKeys, string currentInstruments)
    {
        Dictionary<string, string> instrumentMapping = new Dictionary<string, string>
        {
            {"(BH)", "(BH) - Brickhouse Demo"},
            {"(B)", "(B) - Furch Blue Gc-SA"},
            {"(DX1R)", "(DX1R) - Martin DX1R"},
            {"(MDT)", "(MDT) - Maestro Double Top"},
            {"(M)", "(M) - Martin 00-15m"},
            {"(NST)", "(NST) - Norman ST68"},
            {"(OOM)", "(OOM) - Furch OOM-SR-DB"},
            {"(SAS)", "(SAS) - Seagull Artist Studio"},
            {"(SGI)", "(SGI) - SGI Avenir CW20"},
            {"(SDB)", "(SDB) - Stonebridge OM35ASR-DB"},
            {"(V)", "(V) - Furch Vintage 2 RS-SR"},
            {"(Mandolin)", "Mandolin"},
            {"(Electric Song)", "Electric Guitar"},
            {"(Classical Guitar)", "Classical Guitar"},
            {"(w/ Blues Slide)", "Blues Slide"},
            {"(Electric riff)", "Electric Guitar"},
            {"(Rein Rutnik Performance)", "Harmonica"},
            {"(12/Twelve-String)", "12-String Guitar"}
        };

        foreach (var keyInstrumentPair in instrumentMapping)
        {
            string key = keyInstrumentPair.Key;
            string instrument = keyInstrumentPair.Value;

            if (songWithKeys.Contains(key) && !currentInstruments.Contains(instrument))
            {
                return instrument + ", ";
            }
        }

        if (songWithKeys.Contains("Forget Her") || songWithKeys.Contains("Electric Riff Session"))
        {
             return "Electric Guitar, ";
        }
        
        return "";

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

    /**
     * Removes all keys from the line
     * @param line The line with keys
     * @return The line without keys
     */
    public static string RemoveKeys(string line)
    {
        return line
                .Replace("(BH)", "")
                .Replace("(B)", "")
                .Replace("(DX1R)", "")
                .Replace("(MDT)", "")
                .Replace("(M)", "")
                .Replace("(NST)", "")
                .Replace("(OOM)", "")
                .Replace("(SAS)", "")
                .Replace("(SGI)", "")
                .Replace("(SDB)", "")
                .Replace("(V)", "")
                .Replace("(Mandolin)", "")
                .Replace("(Electric Song)", "")
                .Replace("(Classical Guitar)", "")
                .Replace("(Partial)", "")
                .Replace("(Electric riff)", "")
                .Replace("(Rein Rutnik Performance)", "")
                .Replace("(Chords)", "")
                .Replace("(Short Reggae Version)", "")
                .Replace("(Instrumental)", "")
                .Replace("(12/Twelve-String)", "")
                .Replace("(Old)", "")
                .Replace("(New)", "")
                .Replace("(Audio Issues)", "")
                .Replace("(Live)", "")
                .Replace("(w/ Blues Slide)", "")
                .Replace("(Cont.)", "")
                .Trim();
    }
}