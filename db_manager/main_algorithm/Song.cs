/** 
 * The Song object model to be added to the database.
 * All fields all required. Empty strings are allowed and 
 * somewhat common.
 * @author Michael Totaro
 */
public class Song
{
    public required string Title { get; set; }
    public required string Artist { get; set; }
    public required string OtherArtists { get; set; }
    public required string Appearances { get; set; }
    public required string Other { get; set; }
    public required string Instruments { get; set; }
    public required string Pic { get; set; }
    public required string Links { get; set; }
}
