/**
 * Helper methods for common operations in the program.
 *
 * Methods
 * GetAnsweredYes | Checks if user answered yes to a prompt 
 * ListContainsSongWithoutCommas | Check if list contain strings without commas 
 * GetSongAndArtist | Get list of Song and artist from all-timestamps line.
 * ReplaceNonAsciiChars | Replaces non-ascii charcters with valid ascii characters.
 * IsAscii | Returns whether or not a string contains only Ascii characters 
 * RemoveKeys | Removes all keys from a song title.
 * 
 *
 *
 *
 *
 *
 * @author Michael Totaro
 */
class Helper {

    /**
     * Checks if the user answered yes to a prompt. Ignores case.
     * @param prompt The prompt the user responded to.
     */
    public static bool GetAnsweredYes(string prompt)
    {
        return string.Equals(prompt, "Y", StringComparison.OrdinalIgnoreCase) ||
               string.Equals(prompt, "Yes", StringComparison.OrdinalIgnoreCase);
    }


    public static bool ListContainsSongWithoutCommas(List<string> songList, string song)
    {
        return songList.Contains(song.Trim().Replace("’", "'").Replace("’", "'").Replace("‘", "'"));
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
     * Replaces relevant non-ascii characters from a string with 
     * valid ascii characters.
     * @param song. The title which will have the chars removed.
     * @return String without ascii characters.
     */
    public static string ReplaceNonAsciiChars(string song)
    {
        return song
                .Replace("É", "E")
                .Replace("í", "i")
                .Replace("é", "e")
                .Replace("á","a")
                .Replace("à", "a")
                .Replace("Á", "A")
                .Replace("ü", "u");
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
                .Replace("(Album Version)", "")
                .Replace("(Acoustic Session)", "")
                .Replace("(EP Version)", "")
                .Replace("(Pink Moon Album)", "")
                .Trim();
    }
    
}