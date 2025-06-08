using System.Text.Json.Serialization;

/** 
 * The Instrument object model found in instruments.json.
 */
public class Instrument
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = "";

    [JsonPropertyName("nameNoKey")]
    public string NameNoKey { get; set; } = "";

    [JsonPropertyName("cleaned")]
    public string CleanedName { get; set; } = "";

    [JsonPropertyName("alias")]
    public string? Alias { get; set; }

    [JsonPropertyName("key")]
    public string? Key { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; } = "";

    [JsonPropertyName("appears")]
    public int Appears { get; set; } = 0;
}
