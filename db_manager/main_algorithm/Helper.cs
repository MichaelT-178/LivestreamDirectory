using System.Diagnostics;
using System.Text.RegularExpressions;

/**
 * Helper methods for common operations in the program.
 *
 * Methods
 * GetAnsweredYes | Checks if user answered yes to a prompt 
 * ListContainsSongWithCorrectQuotes | Checks if a song list contains a song with with the correct quotes.
 * GetSongAndArtist | Get list of Song and artist from all-timestamps line.
 * ReplaceWithCorrectQuotes | Replaces stylized quotes with standard ASCII quotes.
 * GetSortedAlphabetList | Gets a list sorted alphabetically.
 * GetSongListWithoutKeys | Removes all the keys for every song in a song list 
 * IsAscii | Returns whether or not a string contains only Ascii characters 
 * RemoveKeys | Removes all keys from a song title.
 * OpenInWebBrowser | Opens a url in the webbrowser
 * ToCamelCase | Converts a kebab case string to camel case
 * 
 * @author Michael Totaro
 */
class Helper
{

    /**
     * Checks if the user answered yes to a prompt. Ignores case.
     * @param prompt The prompt the user responded to.
     */
    public static bool GetAnsweredYes(string prompt)
    {
        return string.Equals(prompt, "Y", StringComparison.OrdinalIgnoreCase) ||
               string.Equals(prompt, "Yes", StringComparison.OrdinalIgnoreCase);
    }

    /**
     * Checks if a song list contains a song with with the correct quotes.
     * @param songList the song list to be checked 
     * @param song The song whose quotes will be replaced.
     * @return Whether the songlist contains the song with the correct quotes.
     */
    public static bool ListContainsSongWithCorrectQuotes(List<string> songList, string song)
    {
        return songList.Contains(ReplaceWithCorrectQuotes(song.Trim()));
    }

    /**
     * Splits a line from the all-timestamps file into a 
     * list of size containing the song and artist. If line 
     * doesn't contain a song and artist null is returned.
     * @param Line from all-timestamps file
     * @return String list with song as first element and 
     *         artist as the second. Null if line doesn't
     *         contain both or one of those things.
     */
    public static string[]? GetSongAndArtist(string line)
    {
        // ["3:07:03", " Do", "It", "Again", "by", "Steely", "Dan"]
        string[] timeSplit = line.Split(new string[] { " " }, StringSplitOptions.None);


        //If line not a blank line or Livestream marker. Ex: Livestream 163. Return null.
        if (timeSplit.Length < 3)
        {
            return null;
        }

        //Do It Again by Steely Dan. Life by the Drop by Stevie Ray Vaughan
        string lineNoTimestamp = line.Replace(timeSplit[0], "").Trim();

        int lastIndexOfBy = lineNoTimestamp.LastIndexOf(" by ");

        if (lastIndexOfBy != -1)
        {
            //Do It Again. Life by the Drop
            string song = lineNoTimestamp[..lastIndexOfBy].Trim();

            //Steely Dan. Stevie Ray Vaughan
            string artist = lineNoTimestamp[(lastIndexOfBy + 3)..].Trim();

            string[] songAndArtist = { song, artist };
            return songAndArtist;
        }

        return null;
    }

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
     * Takes the repertoire list which contains songs formatted
     * as *song* by *artist* and sorts the list alphabetically.
     * Adds a space between groups of alphabetically sorted song.
     * Ex: In the file there will be a space between "WoodStock by Joni Mitchell"
     * and "Yellow by Coldplay"
     * @param songList unsorted repertoire list
     * @return The songList sorted alphabetically.
     */
    public static List<string> GetSortedAlphabetList(List<string> songList)
    {
        string letter = "A";
        List<string> alphabetList = new List<string>();

        songList.Sort();

        foreach (string song in songList)
        {
            string firstChar = song.Substring(0, 1);

            if (!firstChar.Equals(letter, StringComparison.OrdinalIgnoreCase) && alphabetList.Count > 0)
            {
                letter = firstChar.ToUpper();
                alphabetList.Add("");
            }

            alphabetList.Add(song);
        }

        return alphabetList;
    }

    /**
     * Removes all the keys for every song in a song list 
     * @param songList List of song titles with their keys
     * @return The songList where the titles don't have their keys
     */
    public static List<string> GetSongListWithoutKeys(List<string> songList)
    {
        List<string> songListNoKeys = new List<string>();

        foreach (string song in songList)
        {
            if (!songListNoKeys.Contains(RemoveKeys(song)))
            {
                songListNoKeys.Add(RemoveKeys(song));
            }
        }

        return songListNoKeys;
    }

    /**
     * Determines whether the string contains only Ascii chars.
     * @param str String to be checked 
     * @return True if only ascii string else false.
     */
    public static bool IsAscii(string str)
    {
        foreach (char c in str)
        {
            int codePoint = c;

            if (codePoint > 127)
            {
                return false; // Found a non-ASCII character
            }
        }

        return true; // All characters are ASCII
    }

    /**
     * Removes all keys from the line
     * @param line The line with keys
     * @return The line without keys
     */
    public static string RemoveKeys(string line)
    {
        string lineWithReplacedStrings = line
                .Replace("(BH)", "")
                .Replace("(FBG)", "")
                .Replace("(DX1R)", "")
                .Replace("(MDT)", "")
                .Replace("(M15M)", "")
                .Replace("(NST)", "")
                .Replace("(OOM)", "")
                .Replace("(SAS)", "")
                .Replace("(SGI)", "")
                .Replace("(SOM)", "")
                .Replace("(FV2)", "")
                .Replace("(Mandolin)", "")
                .Replace("(Electric Song)", "")
                .Replace("(Classical Guitar)", "")
                .Replace("(Partial)", "")
                .Replace("(w/ Blues Slide)", "")
                .Replace("(Electric riff)", "")
                .Replace("(Rein Rutnik)", "")
                .Replace("(Chords)", "")
                .Replace("(Short Reggae Version)", "")
                .Replace("(Instrumental)", "")
                .Replace("(12-String)", "")
                .Replace("(Old)", "")
                .Replace("(New)", "")
                .Replace("(Blocked in US)", "")
                .Replace("(Audio Issues)", "")
                .Replace("(Live)", "")
                .Replace("(Blues Slide)", "")
                .Replace("(Cont.)", "")
                .Replace("(Album Version)", "")
                .Replace("(Acoustic Session)", "")
                .Replace("(EP Version)", "")
                .Replace("(Pink Moon Album)", "")
                .Replace("(GPPCB)", "")
                .Replace("(GSDG)", "")
                .Replace("(GPRG)", "")
                .Replace("(GLTC)", "")
                .Replace("(LPE)", "")
                .Replace("(MHD)", "")
                .Replace("(FVD)", "")
                .Replace("(BSG)", "")
                .Replace("(MFF)", "")
                .Replace("(Electric Song/DM75)", "")
                .Replace("(Electric Song/Strat)", "")
                .Replace("(Electric riff/Blues Slide)", "")

                .Replace("(OOMV1)", "")
                .Replace("(FBD)", "")
                .Replace("(BSGI)", "")
                .Replace("(GRSG)", "")
                .Replace("(GMNC)", "")
                .Replace("(GMLN)", "")
                .Replace("(GMLHB)", "")
                .Replace("(GFCHN)", "")
                .Replace("(BHG)", "")
                .Replace("(FOSG)", "")
                .Replace("(FG)", "")
                .Replace("(FSD)", "")
                .Replace("(FOB)", "")
                .Replace("(BDC)", "")
                .Replace("(NSPBU)", "")
                .Replace("(NBDA)", "")
                .Replace("(NBBU)", "")
                .Replace("(NSCG)", "")
                .Replace("(NSTCW)", "")
                .Replace("(SD22)", "")
                .Replace("(main)", "")

                .Trim();

        // Replace (Clipped 10)
        return Regex.Replace(lineWithReplacedStrings, @"\s*\(\bLS [1-9][0-9]{0,2} Clip\b\)", "");
        //return Regex.Replace(lineWithReplacedStrings, @"\(\bLS [1-9][0-9]{0,2} Clip\b\)", "");
    }

    /**
     * Opens a url in the webbrowser 
     * @param url The url to be opened
     */
    public static void OpenInWebBrowser(string url)
    {
        Process.Start(new ProcessStartInfo
        {
            FileName = url,
            UseShellExecute = true
        });
    }

    /**
     * Converts a kebab case string to camel case
     *
     * Ex. 
     * the-rolling-stones -> theRollingStones
     *
     * @param kebab The original kebab case string
     * @return the came case string
     */
    public static string ToCamelCase(string kebab)
    {
        if (string.IsNullOrWhiteSpace(kebab))
        {
            return kebab;
        }

        var parts = kebab.Split('-');
        var camel = parts[0].ToLower();

        for (int i = 1; i < parts.Length; i++)
        {
            if (parts[i].Length > 0)
            {
                camel += char.ToUpper(parts[i][0]) + parts[i].Substring(1);
            }
        }

        return camel;
    }
    
}