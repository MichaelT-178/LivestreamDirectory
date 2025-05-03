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
    public required string Song { get; set; }  // non-nullable and required

    [JsonPropertyName("AlbumTitle")]
    public string? AlbumTitle { get; set; }    // nullable 

    [JsonPropertyName("CleanedAlbumTitle")]
    public string? CleanedAlbumTitle { get; set; }    // nullable 

    [JsonPropertyName("Artist")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Artist { get; set; } // nullable

    public int Year { get; set; }
}
