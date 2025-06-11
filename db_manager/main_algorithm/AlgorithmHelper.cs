using System.Text;
using System.Text.RegularExpressions;

/**
 *
 * This class helps perform functions in the main algorithm.
 *
 * Methods
 * GetAllKeysFromLines | Gets all the keys from the appearances as string list.
 * GetKeysJoinedAsString | Takes a key list and joins them as a string separated by '/' characters.
 * GetSongTitlePartialAndIssuesKey | Gets any partial or issue keys from the title.
 * GetYouTubeLink | Gets a timestamped youtube link.
 * GetOtherArtistsAsString | Joins the other artists as a string by commas.
 * AddDefaultAcousticGuitar | Adds acoustic guitar to instruments if conditions are met
 * AddDefaultElectricGuitar | Adds electric guitar to instruments if conditions are met
 * AddDefaultClassicalGuitar | Adds classical guitar to instruments if conditions are met
 * RemoveDuplicateGuitars | Removes duplicate acoustic, classical, and electric substrings
 * MoveAcousticGuitarToFront | Takes a string of instruments and moves Acoustic Guitar to the front.
 * GetInstrumentsFromSong | Gets the instruments from a song title based on it's keys.
 * CleanAppearance | Gets the instruments from a song title based on it's keys.
 * ExtractKeysFromAppearance | Gets the keys from an appearance as a string list
 *
 * @author Michael Totaro
 */
class AlgorithmHelper
{
    /** Path to the keys_to_keep file*/
    private static string keysToKeepPath = "./db_manager/json_files/keys_to_keep.json";

    /** String List of all the keys to not remove */
    private static List<string> keysToKeep = JSONHelper.GetKeyListFromFile(keysToKeepPath, "song_keys");

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
        int numOfReinPerformances = allAppearanceKeys.Count(s => s == "Rein Rutnik");
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
            return " (Rein Rutnik)";
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
     * Joins the other artists as a string by commas.
     * @param otherArtists A string array containing the names of the other artists.
     * @return A string of the other artists joined by commas.
     */
    public static string GetOtherArtistsAsString(string[] otherArtists)
    {
        return string.Join("+ ", otherArtists);
    }

    /**
     * 
     * Adds acoustic guitar to the string on instruments automatically if the songWithKeys
     * doesn't contain any excluded keys.
     *
     * Ex. You would NOT add the default acoustic guitar to the instrument string. Return ""
     * Last Goodbye (NST)
     * 
     * Ex. You would add default acoustic guitar the instrument string.
     * Glenn Tipton
     *
     * @param songWithKeys is the song title with keys
     * @param currentInstruments is the currentInstruments string that contains instrument keys.
     * @return Acoustic guitar string if condition met, else a blank string
     */
    public static string AddDefaultAcousticGuitar(string songWithKeys, string currentInstruments)
    {
        string[] excludedKeys = {
            "(Electric riff", "(Electric Song", "(Classical Guitar", "(Mandolin", "(H)",
            "Electric Riff Session #", "DM75", "Strat", "GPPCB", "GSDG", "GPRG", "GLTC", "FBG", "DX1R",
            "MDT", "M15M", "NST", "OOM", "SAS", "SGI", "SOM", "FV2", "12-String Guitar", "MHD",
            "FVD", "BSG", "BSGI", "MFF", "OOMV1", "FBD", "GRSG", "GMNC", "GMLN", "GMLHB",
            "GFCHN", "BHG", "FOSG", "FG", "FSD", "FOB", "BDC", "NSPBU", "NBDA", "NBBU",
            "NSCG", "NSTCW", "SD22"
        };

        if (excludedKeys.All(key => !songWithKeys.Contains(key)) && !currentInstruments.Contains("Acoustic Guitar"))
        {
            return "Acoustic Guitar, (main) - Stonebridge (Furch) OM32SM, ";
        }

        return "";
    }


    /**
     * 
     * Adds electric guitar to the string on instruments automatically if the songWithKeys
     * doesn't contain any excluded keys.
     *
     * Ex. You would NOT add the default electric guitar to the instrument string. Return ""
     * Satch Boogie (Electric Song/DM75)
     * 
     * Ex. You would add default electric guitar the instrument string.
     * Mr Sandman (Electric Song)
     *
     * @param songWithKeys is the song title with keys
     * @param currentInstruments is the currentInstruments string that contains instrument keys.
     * @return Electric guitar string if condition met, else a blank string
     */
    public static string AddDefaultElectricGuitar(string songWithKeys, string currentInstruments)
    {
        if (
            (songWithKeys.Contains("Electric Song") ||
            songWithKeys.Contains("Electric riff")) &&
            !songWithKeys.Contains("DM75") &&
            !songWithKeys.Contains("GPPCB") &&
            !songWithKeys.Contains("GSDG") &&
            !songWithKeys.Contains("GPRG") &&
            !songWithKeys.Contains("GLTC") &&
            !songWithKeys.Contains("Strat") &&
            !currentInstruments.Contains("Electric Guitar") &&
            !currentInstruments.Contains("Fender Telecaster")
        )
        {
            return "Electric Guitar, Fender Telecaster, ";
        }

        return "";
    }


    /**
     * 
     * Adds classical guitar to the string on instruments automatically if the songWithKeys
     * doesn't contain any excluded keys.
     *
     * Ex. You would NOT add the default classical guitar to the instrument string. Return ""
     * La Patrie Etude (Classical Guitar/LPE)
     * 
     * Ex. You would add default classical guitar the instrument string.
     * Rupert Street (Classical Guitar)
     *
     * @param songWithKeys is the song title with keys
     * @param currentInstruments is the currentInstruments string that contains instrument keys.
     * @return Classical guitar string if condition met, else a blank string
     */
    public static string AddDefaultClassicalGuitar(string songWithKeys, string currentInstruments)
    {
        if (
            songWithKeys.Contains("Classical Guitar") &&
            !songWithKeys.Contains("LPE") &&
            !currentInstruments.Contains("Classical Guitar") &&
            !currentInstruments.Contains("Asturias Standard S")
        )
        {
            return "Classical Guitar, Asturias Standard S, ";
        }

        return "";
    }


    /**
     * 
     * Remove duplicate "Acoustic Guitar, ", "Electric Guitar, ", and "Classical Guitar, " substrings 
     * from the input string. Then return the input string.
     *
     * Ex. 
     * Acoustic Guitar, (main) - Stonebridge (Furch), Acoustic Guitar, (MHD) - Martin HD-28
     * 
     * Becomes
     * Acoustic Guitar, (main) - Stonebridge (Furch), (MHD) - Martin HD-28
     *
     * @param input A string of instruments.
     * @return Instrument string with duplicate instruments removed
     */
    public static string RemoveDuplicateGuitars(string input)
    {
        string[] substrings = { "Classical Guitar, ", "Electric Guitar, ", "Acoustic Guitar, " };

        foreach (string substring in substrings)
        {
            int firstIndex = input.IndexOf(substring);

            if (firstIndex != -1)
            {
                // Keep first occurrence, remove all the other ones
                int startIndex = firstIndex + substring.Length;
                string before = input.Substring(0, startIndex);

                string after = input.Substring(startIndex).Replace(substring, "");
                input = before + after;
            }
        }

        return input;
    }


    /**
     * Takes a string of instruments and moves Acoustic Guitar, and 
     * (main) - Stonebridge (Furch) OM32SM (if present) to the front.
     *
     * Ex.
     * Electric Guitar, Fender Telecaster, Acoustic Guitar, (main) - Stonebridge (Furch) OM32SM
     *
     * Becomes
     * Acoustic Guitar, (main) - Stonebridge (Furch) OM32SM, Electric Guitar, Fender Telecaster
     *
     * @param input A string of instruments.
     * @return Input string with Acoustic guitar at the front.
     */
    public static string MoveAcousticGuitarToFront(string input)
    {
        if (input.StartsWith("Acoustic Guitar"))
        {
            return input;
        }

        if (string.IsNullOrWhiteSpace(input))
        {
            return input;
        }

        // Split input by commas
        List<string> parts = [.. input.Split(',')];

        List<string> reordered = new List<string>();
        List<string> others = new List<string>();

        string? acousticGuitar = null;
        string? mainStonebridge = null;

        foreach (var part in parts)
        {
            string trimmed = part.Trim();

            if (trimmed.Equals("Acoustic Guitar", StringComparison.OrdinalIgnoreCase))
            {
                acousticGuitar = "Acoustic Guitar";
            }
            else if (trimmed.Equals("(main) - Stonebridge (Furch) OM32SM", StringComparison.OrdinalIgnoreCase))
            {
                mainStonebridge = "(main) - Stonebridge (Furch) OM32SM";
            }
            else
            {
                others.Add(trimmed);
            }
        }

        if (acousticGuitar != null)
        {
            reordered.Add(acousticGuitar);
        }

        if (mainStonebridge != null)
        {
            reordered.Add(mainStonebridge);
        }

        reordered.AddRange(others);

        return string.Join(", ", reordered);
    }


    /**
     * Gets the instruments from a song title based on it's keys.
     *
     * Ex. 1
     * Never Going Back Again (SGI)
     *
     * Returns
     * Acoustic Guitar, (SGI) - SGI Avenir CW20,
     * 
     *
     * Ex. 2
     * Furch OOM Vintage 1 (OOMV1) (BH)
     *
     * Returns
     * Acoustic Guitar, (OOMV1) - Furch OOM Vintage 1, (BH) - Brickhouse Demo, 
     * 
     *
     * @param songWithKeys The song title with the instrument keys
     * @param currentInstruments A string of all the instruments of a song.
     * @return String of instruments based off the songs keys
     */
    public static string GetInstrumentsFromSong(string songWithKeys, string currentInstruments)
    {
        Dictionary<string, string> instrumentMapping = new Dictionary<string, string>
        {
            {"(BH)", "(BH) - Brickhouse Demo"},
            {"(FBG)",  "Acoustic Guitar, (FBG) - Furch Blue Gc-SA"},
            {"(DX1R)", "Acoustic Guitar, (DX1R) - Martin DX1R"},
            {"(MDT)",  "Acoustic Guitar, (MDT) - Maestro Double Top"},
            {"(M15M)", "Acoustic Guitar, (M15M) - Martin 00-15m"},
            {"(NST)",  "Acoustic Guitar, (NST) - Norman ST68"},
            {"(OOM)",  "Acoustic Guitar, (OOM) - Furch OOM-SR-DB"},
            {"(SAS)",  "Acoustic Guitar, (SAS) - Seagull Artist Studio"},
            {"(SGI)",  "Acoustic Guitar, (SGI) - SGI Avenir CW20"},
            {"(SOM)",  "Acoustic Guitar, (SOM) - Stonebridge OM35ASR-DB"},
            {"(FV2)",  "Acoustic Guitar, (FV2) - Furch Vintage 2 RS-SR"},
            {"(Mandolin)", "Mandolin"},
            {"(Blues Slide)", "Blues Slide"},
            {"(Rein Rutnik)", "Harmonica"},
            {"(12-String)", "Acoustic Guitar, 12-String Guitar"},

            {"(DM75)", "Electric Guitar, (DM75) - DeArmond M75"},
            {"(Strat)", "Electric Guitar, (Strat) - Fender Stratocaster"},
            {"(GPPCB)", "Electric Guitar, (GPPCB) - Godin Progression Plus Cherry Burst RN"},
            {"(GSDG)", "Electric Guitar, (GSDG) - Godin Stadium '59 Desert Green RN"},
            {"(GPRG)", "Electric Guitar, (GPRG) - Godin Passion RG-3 Indigo Burst RN"},
            {"(GLTC)", "Electric Guitar, (GLTC) - Godin 5th Ave Uptown GT LTD Trans Cream"},

            {"(LPE)", "Classical Guitar, (LPE) - La Patrie Etude"},


            { "(MHD)", "Acoustic Guitar, (MHD) - Martin HD-28" },
            { "(FVD)", "Acoustic Guitar, (FVD) - Furch Vintage 2 D-SR" },
            { "(BSG)", "Acoustic Guitar, (BSG) - Boucher SG-52" },
            { "(MFF)", "(MFF) - Maestro Fan Fretted Singa Flamed maple / Adirondack" },

            { "(BSGI)" ,  "Acoustic Guitar, (BSGI) - Boucher SG-52-I" },
            { "(OOMV1)" , "Acoustic Guitar, (OOMV1) - Furch OOM Vintage 1" },
            { "(FBD)" ,   "Acoustic Guitar, (FBD) - Furch Blue D-CM" },
            { "(GRSG)" ,  "Acoustic Guitar, (GRSG) - Godin Rialto JR Satina Gray HG Q-Discrete" },
            { "(GMNC)" ,  "Acoustic Guitar, (GMNC) - Godin Metropolis Natural Cedar EQ" },
            { "(GMLN)" ,  "Acoustic Guitar, (GMLN) - Godin Metropolis LTD Natural HG EQ" },
            { "(GMLHB)" , "Acoustic Guitar, (GMLHB) - Godin Metropolis LTD Havana Burst HG EQ" },
            { "(GFCHN)" , "Acoustic Guitar, (GFCHN) - Godin Fairmount Concert Hall Natural HG EQ" },
            { "(BHG)" ,   "Acoustic Guitar, (BHG) - Boucher HG-56" },
            { "(FOSG)" ,  "Acoustic Guitar, (FOSG) - Furch OM22TSW-C-DB SGI" },
            { "(FG)" ,    "Acoustic Guitar, (FG) - Furch G25CR-C" },
            { "(FSD)" ,   "Acoustic Guitar, (FSD) - Furch SGI D22TSR" },
            { "(FOB)" ,   "Acoustic Guitar, (FOB) - Furch OM34TSR-DB B2" },
            { "(BDC)" ,   "Acoustic Guitar, (BDC) - Breedlove Discovery Concert CE" },
            { "(NSPBU)" , "Acoustic Guitar, (NSPBU) - Norman ST40 Parlor Burnt Umber" },
            { "(NBDA)" ,  "Acoustic Guitar, (NBDA) - Norman B15 Dark Almond" },
            { "(NBBU)" ,  "Acoustic Guitar, (NBBU) - Norman B20 Burnt Umber" },
            { "(NSCG)" ,  "Acoustic Guitar, (NSCG) - Norman ST40 CW GT Presys" },
            { "(NSTCW)" , "Acoustic Guitar, (NSTCW) - Norman ST68 CW" },
            { "(SD22)" ,  "Acoustic Guitar, (SD22) - Stonebridge D22SR" },

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

        if (songWithKeys.Contains("Forget Her"))
        {
            if (!currentInstruments.Contains("Electric Guitar"))
            {
                instrumentsToAdd.Append("Electric Guitar, ");
            }
        }

        return instrumentsToAdd.ToString();
    }

    /**
     * Gets the instruments from a song title based on it's keys.
     *
     * Livestream 47 (Electric riff/Blues Slide) - > Livestream 47
     *
     * @param appearance The appearance with it's parenthesis keys
     * @return The appearnce without it's parenthesis keys.
     */
    public static string CleanAppearance(string appearance)
    {
        int parenIndex = appearance.IndexOf('(');
        return parenIndex > 0 ? appearance.Substring(0, parenIndex).Trim() : appearance;
    }
    
    /**
     * Gets the keys from an appearance as a string list
     *
     * Livestream 47 (Electric riff/Blues Slide) - > ["Electric riff", "Blues Slide"]
     *
     * @param appearance The appearance with it's parenthesis keys
     * @return A string list of keys
     */
    public static List<string> ExtractKeysFromAppearance(string appearance)
    {
        var match = Regex.Match(appearance, @"\((.*?)\)");

        if (!match.Success)
        {
            return new List<string>();
        }

        string inner = match.Groups[1].Value;

        return inner.Split('/')
                    .Select(k => k.Trim())
                    .Where(k => !string.IsNullOrWhiteSpace(k))
                    .ToList();
    }



}