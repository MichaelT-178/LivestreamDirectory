/** 
 * Artist object in artists.json file.
 */
public class ArtistEntry
{
    public required string Artist { get; set; }
    public required string CleanedArtist { get; set; }
    public required List<ArtistSong> Songs { get; set; }
    public required List<AlbumArtist> Albums { get; set; }
}