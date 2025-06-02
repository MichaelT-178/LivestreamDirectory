/** 
 * The song used in the artists.json songs list.
 */
public class ArtistSong
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public required string CleanedTitle { get; set; }
    public required string Album { get; set; }
    public required string CleanedAlbum { get; set; }
}
