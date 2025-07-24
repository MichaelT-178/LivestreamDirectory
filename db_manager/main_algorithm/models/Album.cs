using System.Text.Json.Serialization;

/** 
 * The Album object model found in albums.json.
 *
 * @author Michael Totaro
 */
public class Album
{
    public int id { get; set; }

    [JsonPropertyName("Song")]
    public required string Song { get; set; }

    [JsonPropertyName("CleanedSong")]
    public required string CleanedSong { get; set; }

    [JsonPropertyName("RepeatSong")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool RepeatSong { get; set; }

    [JsonPropertyName("AlbumTitle")]
    public string? AlbumTitle { get; set; }

    [JsonPropertyName("CleanedAlbumTitle")]
    public string? CleanedAlbumTitle { get; set; }

    [JsonPropertyName("NonNumberCleanedAlbumTitle")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? NonNumberCleanedAlbumTitle { get; set; }

    [JsonPropertyName("Artist")]
    public required string Artist { get; set; }

    [JsonPropertyName("CleanedArtist")]
    public required string CleanedArtist { get; set; }

    [JsonPropertyName("Category")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Category { get; set; }

    public int Year { get; set; }

}
