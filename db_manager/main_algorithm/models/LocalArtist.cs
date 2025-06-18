/** 
 * Structure of artist objects in db_manager/json_files/artists.json
 */
public class LocalArtist
{
    public required string Artist { get; set; }
    public required string CleanedArtist { get; set; }
    public required string Location { get; set; } = "";
    public required int YearFormed { get; set; } = 0;
    public required string Genre { get; set; } = "";
    public required string Country { get; set; } = "";
    public required string CleanedCountry { get; set; } = "";
    public required string Emoji { get; set; } = "";
}
