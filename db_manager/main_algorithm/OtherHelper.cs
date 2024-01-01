using System.Text;

/**
 * Creates the other attribute. 
 *
 * Methods
 * GetInfo | Creates the other attribute for a song
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
    public static string GetInfo(string title, string artist)
    {

        StringBuilder other = new StringBuilder();

        if (title.Contains("“") || title.Contains("”"))
        {
            other.Append(title + ", ");
        }
        
        if (!Helper.IsAscii(title)) 
        {
            other.Append(Helper.ReplaceNonAsciiChars(title) + ", ");
        }


        if (title.Contains("'") || title.Contains("\"") || title.Contains("&") || 
            title.Contains("-") || title.Contains(",")) 
        {
            other.Append(
                title.Replace("'","")
                     .Replace("\\\"","")
                     .Replace(" & ", " and ")
                     .Replace("-", " ")
                     .Replace(",", "")
                     .Replace(".","") + ", "
                );
        }
        
        if (artist.Contains(".") || artist.Contains("'") || artist.Contains("’")) 
        {
            other.Append(
                artist.Replace(".", "")
                      .Replace("'", "")
                      .Replace("’", "")
                      .Replace("‘", "'") + ", "
                );
        }

        if (!Helper.IsAscii(artist)) 
        {
            other.Append(Helper.ReplaceNonAsciiChars(artist) + ", ");
        }
        
        if (title.Contains("and"))
        {
            other.Append(title.Replace(" and ", " & ") + ", ");
        }

        if (title.Contains("'") || title.Contains("."))
        {
            other.Append(title.Replace("'", "’").Replace(".", "") + ", "); 
        }
        
        if (title.ToLower().Contains(" you ")) 
        {
            other.Append(
                title.Replace(" You ", " u ")
                     .Replace(" you ", " u ")
                     .Replace(",", "")
                     .Replace("'", "") + ", "
                );
        }
        
        if (artist.Contains("-") || artist.Contains(",")) 
        { 
            other.Append(artist.Replace("-", " ").Replace(",", " ") + ", "); 
        }
        
        if (title.Contains("'")) 
        {
            other.Append(title.Replace("'", "‘") + ", ");
        }
        
        if (artist.Contains("'")) 
        {
            other.Append(artist.Replace("'", "‘") + ", ");
        }

        //Song and artist specific other attributes. Add then return.
        if (artist.Equals("Black Sabbath")) { other.Append("Ozzy Osbourne, "); return other.ToString(); }
        if (artist.Equals("Ozzy Osbourne")) { other.Append("Black Sabbath, "); return other.ToString(); }

        if (artist.Equals("P!nk")) { other.Append("Pink, " ); return other.ToString(); }
        if (artist.Contains("Red Hot Chili")) { other.Append("The Red Hot Chili Peppers, "); return other.ToString(); }
        if (artist.Contains("Young") && !artist.Equals("Neil Young")) { other.Append("Neil Young, "); return other.ToString(); }
        if (title.Contains("Starbird")) { other.Append("Star bird, "); return other.ToString(); }
        if (artist.Contains("Allman")) { other.Append("The Allman Brothers, "); return other.ToString(); }
        if (artist.Contains("Nelly") || artist.Contains("Flo Rida")) { other.Append("Rap, "); return other.ToString(); }
        if (artist.Equals("Extreme")) { other.Append("The Extreme, "); return other.ToString(); }
        if (artist.Equals("The Police")) { other.Append("Sting, "); return other.ToString(); }

        if (title.Contains("Grey")) { other.Append(title.Replace("Grey", "Gray") + ", "); }
        if (artist.Contains("Grey")) { other.Append(artist.Replace("Grey", "Gray") + ", "); return other.ToString(); }

        if (title.Contains("Man Of Constant Sorrow")) { other.Append("I Am A Man of Constant Sorrow, "); return other.ToString(); }
        if (title.Equals("Vincent")) { other.Append("Vincent (Starry, Starry Night), "); return other.ToString(); }

        if (title.Contains("Xmas")) other.Append("Happy Christmas, "); 
        if (title.Contains("Xmas")) { other.Append("Merry Christmas, " ); return other.ToString(); }

        if (artist.Contains("Simon & Gar")) { other.Append("Simon and Garfunkel, "); return other.ToString(); }
        
        if (artist.Contains("Bublé")) other.Append("Bubble, ");
        if (artist.Contains("Bublé")) { other.Append("Buble, "); return other.ToString(); }
        if (artist.Contains("Simon & Gar")) { other.Append("Paul Simon, "); return other.ToString(); }
        if (artist.Equals("AC")) { other.Append("ACDC, "); return other.ToString(); }
        if (artist.Equals("Dire Straits")) { other.Append("The Dire Straits, "); return other.ToString(); }
        if (artist.Equals("Joe Walsh")) { other.Append("The Eagles, " ); return other.ToString(); }
        if (artist.Equals("Elliott Smith")) { other.Append("Elliot , "); return other.ToString(); }
        if (title.Equals("Trouble So Hard")) { other.Append("Natural Blues by Moby, "); return other.ToString(); }
        if (title.Equals("Natural Blues")) { other.Append("Trouble So Hard by Vera Hall, "); return other.ToString(); }
        if (title.Equals("Satisfied Mind")) { other.Append("A Satisfied Mind, "); return other.ToString(); }
        
        return other.ToString();
    }
}