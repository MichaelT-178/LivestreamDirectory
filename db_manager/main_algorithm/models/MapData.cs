/** 
 * The MapData object model used for InstrumentMap.
 *
 * @author Michael Totaro
 */
public class MapData
{
    public string InstrumentKey { get; set; }
    public InstrumentSong InstrumentSong { get; set; }

    public MapData(string instrumentKey, InstrumentSong instrumentSong)
    {
        InstrumentKey = instrumentKey;
        InstrumentSong = instrumentSong;
    }
}
