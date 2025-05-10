/** 
 * The MapData object model used for InstrumentMap.
 *
 * @author Michael Totaro
 */
public class MapData
{
    public string Artist { get; set; }
    public InstrumentSong InstrumentSong { get; set; }

    public MapData(string artist, InstrumentSong instrumentSong)
    {
        Artist = artist;
        InstrumentSong = instrumentSong;
    }
}
