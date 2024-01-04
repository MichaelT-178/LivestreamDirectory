using System.Text;

/**
 * Creates the other attribute. 
 *
 * Methods
 * GetInfo | Creates the other attribute for a song
 * AppendNew | Checks that content is not already in other, then adds it
 *
 * @author Michael Totaro
 */
class OtherHelper
{   
    /**
     * Creates the other attribute. Adds information to the attribute
     * to helper the user find the song easier using the search bar.
     *
     * @param title The title of the song
     * @param artist The artist of the song
     * @return The other attribute 
     */
    public static void GetInfo(ref StringBuilder other, string title, string artist)
    {   

        /**
         * Checks content is not already in other, then adds it
         * @param content to be appended to other
         * @param endString String to be appended at end.
         */
        void AppendNew(string content, StringBuilder other)
        {
            if (!other.ToString().Contains(content))
            {
                other.Append(content + ", ");
            }
        }

        if (title.Contains('\"'))
        {
            AppendNew(title.Replace("\"", "“").Replace("\"","”"), other);
        }
        
        if (!Helper.IsAscii(title)) 
        {   
            AppendNew(Helper.ReplaceNonAsciiChars(title), other);
        }


        if (title.Contains('\'') || title.Contains('"') || title.Contains('&') || 
            title.Contains('-') || title.Contains(',')) 
        {
            AppendNew(
                title.Replace("'","")
                     .Replace("\"","")
                     .Replace(" & ", " and ")
                     .Replace("-", " ")
                     .Replace(",", "")
                     .Replace(".",""), 
                     other
                );
        }
    
        if (artist.Contains('.') || artist.Contains('\''))
        {
            AppendNew(artist.Replace(".", "").Replace("'", ""), other);
        }

        if (!Helper.IsAscii(artist)) 
        {
            AppendNew(Helper.ReplaceNonAsciiChars(artist), other);
        }
        
        if (title.Contains("and"))
        {
            AppendNew(title.Replace(" and ", " & "), other);
        }

        if (title.Contains("'") || title.Contains("."))
        {
            AppendNew(title.Replace("'", "’").Replace(".", ""), other);
        }
        
        if (title.ToLower().Contains(" you ")) 
        {
            AppendNew(
                title.Replace(" You ", " u ")
                     .Replace(" you ", " u ")
                     .Replace(",", "")
                     .Replace("'", ""), 
                     other
                );
        }
        
        if (artist.Contains("-") || artist.Contains(",")) 
        { 
            AppendNew(artist.Replace("-", " ").Replace(",", " "), other);
        }
        
        if (title.Contains("'")) 
        {
            AppendNew(title.Replace("'", "‘"), other);
        }
        
        if (artist.Contains("'")) 
        {
            AppendNew(artist.Replace("'", "‘"), other);
        }

        //Song and artist specific other attributes. Add then return.
        if (artist.Equals("Black Sabbath")) { AppendNew("Ozzy Osbourne", other); return; }
        if (artist.Equals("Ozzy Osbourne")) { AppendNew("Black Sabbath", other); return; }

        if (artist.Equals("P!nk")) { AppendNew("Pink", other); return; }
        if (artist.Contains("Red Hot Chili")) { AppendNew("The Red Hot Chili Peppers", other); return; }
        if (artist.Contains("Young") && !artist.Equals("Neil Young")) { AppendNew("Neil Young", other); return; }
        if (title.Contains("Starbird")) { AppendNew("Star bird", other); return; }
        if (artist.Contains("Allman")) { AppendNew("The Allman Brothers", other); return; }
        if (artist.Contains("Nelly") || artist.Contains("Flo Rida")) { AppendNew("Rap", other); return; }
        if (artist.Equals("Extreme")) { AppendNew("The Extreme", other); return; }
        if (artist.Equals("The Police")) { AppendNew("Sting", other); return; }

        if (title.Contains("Grey")) { AppendNew(title.Replace("Grey", "Gray"), other); }
        if (artist.Contains("Grey")) { AppendNew(artist.Replace("Grey", "Gray"), other); return; }

        if (title.Contains("Man Of Constant Sorrow")) { AppendNew("I Am A Man of Constant Sorrow", other); return; }
        if (title.Equals("Vincent")) { AppendNew("Vincent (Starry, Starry Night)", other); return; }

        if (title.Contains("Xmas")) AppendNew("Happy Christmas", other); 
        if (title.Contains("Xmas")) { AppendNew("Merry Christmas", other); return; }

        if (artist.Contains("Simon & Gar")) { AppendNew("Simon and Garfunkel", other); return; }
        
        if (artist.Contains("Bublé")) AppendNew("Bubble", other);
        if (artist.Contains("Bublé")) { AppendNew("Buble", other); return; }
        if (artist.Contains("Simon & Gar")) { AppendNew("Paul Simon", other); return; }
        if (artist.Equals("AC")) { AppendNew("ACDC", other); return; }
        if (artist.Equals("Dire Straits")) { AppendNew("The Dire Straits", other); return; }
        if (artist.Equals("Joe Walsh")) { AppendNew("The Eagles", other); return; }
        if (artist.Equals("Elliott Smith")) { AppendNew("Elliot ", other); return; } //The space in Elliot is on purpose
        if (title.Equals("Trouble So Hard")) { AppendNew("Natural Blues by Moby", other); return; }
        if (title.Equals("Natural Blues")) { AppendNew("Trouble So Hard by Vera Hall", other); return; }
        if (title.Equals("Satisfied Mind")) { AppendNew("A Satisfied Mind", other); return; }
        if (title.Contains("Autumn Leaves")) { AppendNew("Jazz", other); return; }
        if (title.Contains("D'yer Mak'er")) { AppendNew("Deyer", other); return; }
    }
}