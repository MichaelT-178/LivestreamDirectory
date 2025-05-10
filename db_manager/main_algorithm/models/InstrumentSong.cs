/** 
 * The InstrumentSong object model used in InstrumentMap.
 *
 * @author Michael Totaro
 */
public class InstrumentSong
{   

    // 28
    public int Id { get; set; }

    // Going to California 
    public string SongTitle { get; set; } = "";

    // Ex. Led Zeppelin
    public string Artist { get; set; } = "";

    // Ex. Livestream 61
    public string Livestream { get; set; } = "";

    // https://youtu.be/Oc4W2nrPzJU&t=4561
    public string Link { get; set; } = "";
    
    
    public InstrumentSong(int id, string songTitle, string artist, string livestream, string link)
    {
        Id = id;
        SongTitle = songTitle;
        Artist = artist;
        Livestream = livestream;
        Link = link;
    }
}