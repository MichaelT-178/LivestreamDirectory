/** 
 * The Song object model to be added to the database.
 * All fields all required. Empty strings are allowed and 
 * somewhat common.
 * @author Michael Totaro
 */
public class Song
{
    public string Title { get; set; }
    public string Artist { get; set; }
    public string OtherArtists { get; set; }
    public string Appearances { get; set; }
    public string Other { get; set; }
    public string Instruments { get; set; }
    public string Pic { get; set; }
    public string Links { get; set; }

    public Song(string title, string artist, string otherArtists, string appearances, string other, string instruments, string artistPic, string links)
    {
        Title = title;
        Artist = artist;
        OtherArtists = otherArtists;
        Appearances = appearances;
        Other = other;
        Instruments = instruments;
        Pic = artistPic;
        Links = links;
    }
}

/**
 * A container class that matches the structure of the 
 * database song_list.json file.
 */
public class SongsContainer
{
    public List<Song> Songs { get; set; } = new List<Song>();
}