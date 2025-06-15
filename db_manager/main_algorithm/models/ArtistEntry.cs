/** 
 * Artist object in artists.json file.
 */
public class ArtistEntry
{
    public required string Artist { get; set; }
    public required string CleanedArtist { get; set; }
    public required string Location { get; set; }
    public required int YearFormed { get; set; }
    public required string Genre { get; set; }
    public required string Country { get; set; }
    public required string Emoji { get; set; }
    public required List<ArtistSong> Songs { get; set; }
    public required List<AlbumArtist> Albums { get; set; }
}