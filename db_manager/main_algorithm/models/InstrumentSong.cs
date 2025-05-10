/** 
 * The InstrumentSong object model used in InstrumentMap.
 *
 * @author Michael Totaro
 */
public class InstrumentSong
{
    public int Id { get; set; }

    // Going to California 
    public string SongTitle { get; set; } = "";

    // Ex. Led Zeppelin
    public string Artist { get; set; } = "";

    // Ex. Livestream 61
    public string Livestream { get; set; } = "";

    // 
    public string Link { get; set; } = "";
}