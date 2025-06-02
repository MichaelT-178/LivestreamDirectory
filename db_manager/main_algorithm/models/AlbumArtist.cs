/** 
 * The album object used in the artists.json albums list.
 */
public class AlbumArtist
{
    public required string Title { get; set; }
    public required string CleanedTitle { get; set; }
    public int Year { get; set; }
}
