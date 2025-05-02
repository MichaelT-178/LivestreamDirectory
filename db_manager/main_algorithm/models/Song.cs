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
    public string Artist { get; set; }
    public string Album { get; set; }
    public string OtherArtists { get; set; }
    public string Instruments { get; set; }
    public string Pic { get; set; }
    public string Search { get; set; }
    public string Appearances { get; set; }
    public string Links { get; set; }

    public Song(int id, string title, string artist, string album, string otherArtists, string instruments, 
                string artistPic, string search, string appearances, string links)
    {
        Id = id;
        Title = title;
        Artist = artist;
        Album = album;
        OtherArtists = otherArtists;
        Instruments = instruments;
        Pic = artistPic;
        Search = search;
        Appearances = appearances;
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