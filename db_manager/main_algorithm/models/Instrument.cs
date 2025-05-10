using System.Text.Json.Serialization;

/** 
 * The Instrument object model found in instruments.json.
 *
 * @author Michael Totaro
 */
public class Instrument
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = "";

    [JsonPropertyName("cleaned")]
    public string CleanedName { get; set; } = "";

    [JsonPropertyName("alias")]
    public string? Alias { get; set; }
}