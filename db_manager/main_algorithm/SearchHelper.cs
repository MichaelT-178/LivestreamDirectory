using System.Text;

/**
 * Creates the Search attribute. 
 *
 * Methods
 * GetInfo | Creates the Search attribute for a song
 * AppendNew | Checks that content is not already in Search reference, then adds it
 *
 * @author Michael Totaro
 */
class SearchHelper
{   
    /**
     * Creates the Search attribute. Adds information to the attribute
     * to helper the user find the song easier using the search bar.
     *
     * @search A reference to the StringBuilder Search attribute.
     * @param title The title of the song
     * @param artist The artist of the song
     * @param instruments The instruments of the song
     * @return The other attribute 
     */
    public static void GetInfo(ref StringBuilder search, string title, string artists, string instruments)
    {   

        /**
         * Checks content is not already in other, then adds it
         * @param content to be appended to other
         * @param endString String to be appended at end.
         */
        static void AppendNew(string content, StringBuilder search)
        {
            if (!search.ToString().Contains(content))
            {
                search.Append(content + ", ");
            }
        }

        if (title.Contains('\"'))
        {
            AppendNew(title.Replace("\"", "“").Replace("\"","”"), search);
        }
        
        if (!Helper.IsAscii(title)) 
        {   
            AppendNew(TextCleaner.NormalizeToAscii(title), search);
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
                     search
                );
        }
    
        if (artists.Contains('.') || artists.Contains('\''))
        {
            AppendNew(artists.Replace(".", "").Replace("'", "").Replace("+", "/"), search);
        }

        if (!Helper.IsAscii(artists)) 
        {
            AppendNew(TextCleaner.NormalizeToAscii(artists).Replace("+", "/"), search);
        }
        
        if (title.Contains("and"))
        {
            AppendNew(title.Replace(" and ", " & "), search);
        }

        if (title.Contains('\'') || title.Contains('.'))
        {
            AppendNew(title.Replace("'", "’").Replace(".", ""), search);
        }
        
        if (title.ToLower().Contains(" you ")) 
        {
            AppendNew(
                title.Replace(" You ", " u ")
                     .Replace(" you ", " u ")
                     .Replace(",", "")
                     .Replace("'", ""), 
                     search
                );
        }

        foreach (string artist in artists.Split("+"))
        {
            if (artist.Contains("-") || artist.Contains(",")) 
            { 
                AppendNew(artists.Replace("-", " ").Replace(",", " ").Replace("+", "/"), search);
            }
        }
        
        if (title.Contains("'")) 
        {
            AppendNew(title.Replace("'", "‘"), search);
        }
        
        if (artists.Contains("'")) 
        {
            AppendNew(artists.Replace("'", "‘").Replace("+", "/"), search);
        }
        
        
        if (instruments.Contains("12-String"))
        {
             AppendNew("Twelve", search);
             AppendNew("Twelve-String", search);
             AppendNew("Twelve String", search);
        }

        //Song and artist specific other attributes. Add then return.
        if (artists.Contains("Black Sabbath")) { AppendNew("Ozzy Osbourne", search); return; }
        if (artists.Contains("Ozzy Osbourne")) { AppendNew("Black Sabbath", search); return; }

        if (artists.Contains("Creedence Clearwater Revival")) { AppendNew("CCR", search); return; }

        if (artists.Contains("P!nk")) { AppendNew("Pink", search); return; }
        if (artists.Contains("Red Hot Chili")) { AppendNew("The Red Hot Chili Peppers", search); }
        if (artists.Contains("Red Hot Chili")) { AppendNew("RHCP", search); return; }
        if (artists.Contains("Young") && !artists.Equals("Neil Young")) { AppendNew("Neil Young", search); return; }
        if (artists.Contains("Guns N' Roses")) { AppendNew("GNR", search); }
        if (artists.Contains("Guns N' Roses")) { AppendNew("Guns and Roses", search); }
        if (artists.Contains("Guns N' Roses")) { AppendNew("Slash", search); return; }
        
        if (title.Equals("Angel")) { AppendNew("In the arms of the angel Fly away from here", search); return; }

        if (title.Contains("Starbird")) { AppendNew("Star bird", search); return; }
        if (title.ToLower().Contains("running on empty")) { AppendNew("forrest gump", search); return; }
        if (title.Contains("River Man")) { AppendNew("riverman", search); return; }
        if (title.Contains("Hymne")) { AppendNew("Ode to Love", search); }
        if (title.Contains("Hymne")) { AppendNew("French", search); return; }
        if (artists.Contains("Allman")) { AppendNew("The Allman Brothers", search); return; }
        if (artists.Contains("Nelly") || artists.Contains("Flo Rida")) { AppendNew("Rap", search); return; }
        if (artists.Equals("Extreme")) { AppendNew("The Extreme", search); return; }
        if (artists.Equals("Steve Miller Band")) { AppendNew("The Steve Miller Band", search); return; }
        
        if (title.Contains("La Patrie Etude")) { AppendNew("Op. 35 No. 17 op 35 no 17", search); return; }
        if (title.Contains("Study No. 6 in D")) { AppendNew("Op. 35 No. 17 op 35 no 17", search); return; }
        
        if (title.Contains("Study in B-Minor")) { AppendNew("Op. 35 No. 22 op 35 no 22", search); return; }


        if (artists.Contains("Velvet Revolver")) { AppendNew("Guns N' Roses", search); }
        if (artists.Contains("Velvet Revolver")) { AppendNew("Slash", search); return; }
        
        if (artists.Contains("Stevie Ray Vaughan")) { AppendNew("SRV", search); return; }

        if (title.Contains("Grey")) { AppendNew(title.Replace("Grey", "Gray"), search); }
        if (artists.Contains("Grey")) { AppendNew(artists.Replace("Grey", "Gray"), search); return; }
        
        if (artists.Contains("Santana")) { AppendNew("Carlos Santana", search); return; }
        if (artists.Contains("Van Halen")) { AppendNew("Eddie Van Halen", search); return; }

        if (title.Contains("Man Of Constant Sorrow")) { AppendNew("I Am A Man of Constant Sorrow", search); return; }
        if (title.Equals("Vincent")) { AppendNew("Vincent (Starry, Starry Night)", search); return; }
        if (title.Equals("That's All")) { AppendNew("Phil Collins", search); return; }
        
        if (title.Equals("When I'm Up - I Can't Get Down")) { AppendNew("When I'm Up (I Can't Get Down)", search); return; }

        if (title.Contains("Spanish Romance")) { AppendNew("Jeux Interdits", search); return; }

        if (title.Contains("Xmas")) AppendNew("Happy Christmas", search); 
        if (title.Contains("Xmas")) { AppendNew("Merry Christmas", search); return; }
        if (title.Contains("Somebody That I Used")) { AppendNew("Somebody I Used", search); return; }

        if (artists.Contains("Simon & Gar")) { AppendNew("Simon and Garfunkel", search); return; }
        if (artists.Contains("Joe Walsh")) { AppendNew("The Eagles", search); return; }
        
        if (artists.Contains("Bublé")) AppendNew("Michael Bubble", search);
        if (artists.Contains("Simon & Gar")) { AppendNew("Paul Simon", search); return; }
        if (artists.Contains("AC/DC")) { AppendNew("ACDC", search); return; }
        if (artists.Contains("Dire Straits")) { AppendNew("The Dire Straits", search); return; }
        if (artists.Contains("Joe Walsh")) { AppendNew("The Eagles", search); return; }
        if (artists.Contains("Elliott Smith")) { AppendNew("Elliot ", search); return; } //The space in Elliot is on purpose
        if (title.Equals("Trouble So Hard")) { AppendNew("Natural Blues by Moby", search); return; }
        if (title.Equals("Natural Blues")) { AppendNew("Trouble So Hard by Vera Hall", search); return; }
        if (title.Equals("Satisfied Mind")) { AppendNew("A Satisfied Mind", search); return; }
        if (title.Contains("Autumn Leaves")) { AppendNew("Jazz", search); return; }
        if (title.Contains("D'yer Mak'er")) { AppendNew("Deyer", search); return; }
    }
}