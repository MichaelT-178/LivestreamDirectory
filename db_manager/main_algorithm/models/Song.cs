/** 
 * The Song object model to be added to the database.
 * All fields all required. Empty strings are allowed and 
 * somewhat common.
 *
 * @author Michael Totaro
 */
public class Song
{   
    public int Id { get; set; }
    public string Title { get; set; }
    public string CleanedTitle { get; set; }
    public string Artist { get; set; }
    public string CleanedArtist { get; set; }
    public string Album { get; set; }
    public string CleanedAlbum { get; set; }
    public string Other_Artists { get; set; }
    public string Instruments { get; set; }
    public int Year { get; set; }
    public string Search { get; set; }
    public string Appearances { get; set; }
    public string Links { get; set; }

    public Song(int id, string title, string cleanedTitle, string artist, string cleanedArtist, 
                string album, string cleanedAlbum, string otherArtists, string instruments, int year,
                string search, string appearances, string links)
    {
        Id = id;
        Title = title;
        CleanedTitle = cleanedTitle;
        Artist = artist;
        CleanedArtist = cleanedArtist;
        Album = album;
        CleanedAlbum = cleanedAlbum;
        Other_Artists = otherArtists;
        Instruments = instruments;
        Year = year;
        Search = search;
        Appearances = appearances;
        Links = links;
    }
}