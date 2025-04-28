using System.Text;
using System.Text.RegularExpressions;

/**
 *
 * This class helps perform functions in the main algorithm.
 *
 * Methods
 * GetAllKeysFromLines | Gets all the keys from the appearances as string list.
 * GetKeysJoinedAsString | Takes a key list and joins them as a string seperated by '/' characters.
 * GetSongTitlePartialAndIssuesKey | Gets any partial or issue keys from the title.
 * GetYouTubeLink | Gets a timestamped youtube link.
 * GetArtistPic | Gets the artists picture file path as a valid string.
 * GetOtherArtistsAsString | Joins the other artists as a string by commas.
 * AddDefaultAcousticGuitar | Adds acoustic guitar to instruments if necessary
 * GetInstrumentsFromSong | Gets the instrument from a song title based on it's keys.
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
     * Gets all the keys from the current livestream appearance 
     * and songs returns them in one string list. 
     * 
     * @param currLs The current livestream or solo video appearance 
     * @param song The song title which has keys.
     * @return A string list of the keys for 
     */
    public static List<string> GetAllKeysFromLines(string currLs, string song)
        {
        string pattern = @"\((.*?)\)";
        Regex regex = new(pattern);

        List<string> keysFromCurrLs = [];
        List<string> keysFromSong = [];

        foreach (Match match in regex.Matches(currLs))
        {
            keysFromCurrLs.Add(match.Groups[1].Value);
        }

        foreach (Match match in regex.Matches(song))
        {
            keysFromSong.Add(match.Groups[1].Value);
        }

        List<string> resultKeys = [];

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
    public static string GetKeysJoinedAsString(List<string> keyList)
    {   
        // Guard clause. If list is empty, just return empty string.
        if (keyList.Count == 0) return "";

        string resultString = "";

        foreach (string key in keyList)
        {
            string keyNoParenthesis = key.Replace("(", "").Replace(")", "");
            keyNoParenthesis += (keyNoParenthesis == "Blocked in US") ? " " : "";

            resultString += keyNoParenthesis + "/";
        }

        if (resultString.Count(c => c == '/') == 1) 
        {
            resultString = resultString.Replace("Blocked in US ", "Blocked in US");
        }

        //[..^1] removes the last "/" character from resultString. Add parenthesis around string
        return " (" + resultString[..^1] + ")";

    }

    /**
     * Gets any partial or issue keys from the title. These keys will be 
     * displayed in the title in the UI search results. This works by 
     * extracting and counting keys from the appearances attribute string.
     * @param appearances List containing all appearances and their keys.
     * @return Partial or issue keys to add and display in the title.
     */
    public static string GetSongTitlePartialAndIssuesKey(string appearances)
    {

        string[] specialKeys = { "(Electric riff/Blues Slide)" };
    
        foreach (var specialKey in specialKeys)
        {
            if (appearances.Contains(specialKey))
            {
                return " " + specialKey;
            }
        }

        int numOfAppearances = appearances.Split(",").Length;
        List<string> appearanceKeys = GetAllKeysFromLines(appearances, "");
        
        List<string> allAppearanceKeys = [];
        
        foreach (string appKey in appearanceKeys)
        {
            List<string> appKeysList = appKey.Split("/").ToList();

            foreach (string aKey in appKeysList)
            {
                allAppearanceKeys.Add(aKey.Replace("(", "").Replace(")", "").Trim());
            }
        }

        int numOfElectricRiffs = allAppearanceKeys.Count(s => s == "Electric riff");
        int numOfAudioIssues = allAppearanceKeys.Count(s => s == "Audio Issues");
        int numOfReinPerformances = allAppearanceKeys.Count(s => s == "Rein Rutnik Performance");
        int numOfInstrumental = allAppearanceKeys.Count(s => s == "Instrumental");
        int numOfPartial = allAppearanceKeys.Count(s => s == "Partial");

        if (
            numOfElectricRiffs == 0 &&
            numOfAudioIssues == 0 && 
            numOfReinPerformances == 0 && 
            numOfInstrumental == 0 && 
            numOfPartial == 0
        )
        {
            return "";
        }

        if (numOfElectricRiffs == numOfAppearances)
        {
            return " (Electric riff)";
        }
        else if (numOfReinPerformances == numOfAppearances)
        {
            return " (Rein Rutnik Performance)";
        }
        else if (numOfAudioIssues == numOfAppearances)
        {
            return " (Audio Issues)";
        }
        else if (numOfInstrumental == numOfAppearances)
        {
            return " (Instrumental)";
        }
        else if ((numOfPartial + numOfInstrumental + numOfElectricRiffs) == numOfAppearances)
        {
            return " (Partial)";
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
     * @param otherArtists A string array containing the names of the other artists.
     * @return A string of the other artists joined by commas.
     */
    public static string GetOtherArtistsAsString(string[] otherArtists)
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
            !songWithKeys.Contains("GPPCB") &&
            !songWithKeys.Contains("GSDG") &&
            !songWithKeys.Contains("GPRG") &&
            !songWithKeys.Contains("GLTC") &&
            !currentInstruments.Contains("Acoustic Guitar")
        ) 
        {
            return "Acoustic Guitar, ";
        }
    
        return "";
    }

    public static string GetInstrumentsFromSong(string songWithKeys, string currentInstruments)
    {
        Dictionary<string, string> instrumentMapping = new Dictionary<string, string>
        {
            {"(Electric Song)", "Electric Guitar"},
            {"(Electric riff)", "Electric Guitar"},
            {"(Classical Guitar)", "Classical Guitar"},
            {"(BH)", "(BH) - Brickhouse Demo"},
            {"(B)", "(B) - Furch Blue Gc-SA"},
            {"(DX1R)", "(DX1R) - Martin DX1R"},
            {"(MDT)", "(MDT) - Maestro Double Top"},
            {"(M)", "(M) - Martin 00-15m"},
            {"(NST)", "(NST) - Norman ST68"},
            {"(OOM)", "(OOM) - Furch OOM-SR-DB"},
            {"(SAS)", "(SAS) - Seagull Artist Studio"},
            {"(SGI)", "(SGI) - SGI Avenir CW20"},
            {"(SOM)", "(SOM) - Stonebridge OM35ASR-DB"},
            {"(V)", "(V) - Furch Vintage 2 RS-SR"},
            {"(Mandolin)", "Mandolin"},
            {"(Blues Slide)", "Blues Slide"},
            {"(Rein Rutnik Performance)", "Harmonica"},
            {"(12-String)", "12-String Guitar"},

            {"(GPPCB)", "(GPPCB) - Godin Progression Plus Cherry Burst RN"},
            {"(GSDG)", "(GSDG) - Godin Stadium '59 Desert Green RN"},
            {"(GPRG)", "(GPRG) - Godin Passion RG-3 Indigo Burst RN"},
            {"(GLTC)", "(GLTC) - Godin 5th Ave Uptown GT LTD Trans Cream"},
            {"(BSGI)", "(BSGI) - Boucher SG-52-I"},
            {"(Electric riff/Blues Slide)", "Electric Guitar, Blues Slide"}
        };

        StringBuilder instrumentsToAdd = new StringBuilder();

        Regex regex = new(@"\(([^)]*)\)");

        foreach (Match match in regex.Matches(songWithKeys))
        {
            string keyContent = match.Groups[1].Value; // e.g., "BH/BSGI"

            // Split by '/' in case it's multiple keys
            string[] individualKeys = keyContent.Split('/');

            foreach (string singleKey in individualKeys)
            {
                string formattedKey = $"({singleKey})";

                if (instrumentMapping.ContainsKey(formattedKey) && !currentInstruments.Contains(instrumentMapping[formattedKey]))
                {
                    instrumentsToAdd.Append(instrumentMapping[formattedKey] + ", ");
                    currentInstruments += instrumentMapping[formattedKey]; // So no duplicates inside the same run
                }
            }
        }

        if (songWithKeys.Contains("Forget Her") || songWithKeys.Contains("Electric Riff Session"))
        {
            if (!currentInstruments.Contains("Electric Guitar"))
            {
                instrumentsToAdd.Append("Electric Guitar, ");
            }
        }

        return instrumentsToAdd.ToString();
    }


    // /**
    //  * Gets the instrument from a song title based on it's keys, which will be
    //  * added to the currentInstruments attribute if it's not already in it. 
    //  * The currentInstruments attribute will become the Instruments attribute 
    //  * of the song objects in the database.
    //  * @param songWithKeys The song with it's keys 
    //  * @param currentInstruments The attribute which contains all the songs 
    //  *        instruments so far.
    //  * @return The instrument string based on the key with comma. Ex: "Classical Guitar, "
    //  */
    // public static string GetInstrumentsFromSong(string songWithKeys, string currentInstruments)
    // {
    //     Dictionary<string, string> instrumentMapping = new Dictionary<string, string>
    //     {
    //         {"(BH)", "(BH) - Brickhouse Demo"},
    //         {"(B)", "(B) - Furch Blue Gc-SA"},
    //         {"(DX1R)", "(DX1R) - Martin DX1R"},
    //         {"(MDT)", "(MDT) - Maestro Double Top"},
    //         {"(M)", "(M) - Martin 00-15m"},
    //         {"(NST)", "(NST) - Norman ST68"},
    //         {"(OOM)", "(OOM) - Furch OOM-SR-DB"},
    //         {"(SAS)", "(SAS) - Seagull Artist Studio"},
    //         {"(SGI)", "(SGI) - SGI Avenir CW20"},
    //         {"(SOM)", "(SOM) - Stonebridge OM35ASR-DB"},
    //         {"(V)", "(V) - Furch Vintage 2 RS-SR"},
    //         {"(Mandolin)", "Mandolin"},
    //         {"(Electric Song)", "Electric Guitar"},
    //         {"(Classical Guitar)", "Classical Guitar"},
    //         {"(Blues Slide)", "Blues Slide"},
    //         {"(Electric riff)", "Electric Guitar"},
    //         {"(Rein Rutnik Performance)", "Harmonica"},
    //         {"(12-String)", "12-String Guitar"}
    //     };

    //     foreach (var keyInstrumentPair in instrumentMapping)
    //     {
    //         string key = keyInstrumentPair.Key;
    //         string instrument = keyInstrumentPair.Value;

    //         if (songWithKeys.Contains(key) && !currentInstruments.Contains(instrument))
    //         {
    //             return instrument + ", ";
    //         }
    //     }

    //     if (songWithKeys.Contains("Forget Her") || songWithKeys.Contains("Electric Riff Session"))
    //     {
    //          return "Electric Guitar, ";
    //     }
        
    //     return "";
    // }

}