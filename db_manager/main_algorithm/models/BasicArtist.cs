/** 
 * The object used in GetDatabaseArtists
 */
public class BasicArtist
{
    public int Id { get; set; }
    public required string Artist { get; set; }
    public required string CleanedArtist { get; set; }
}