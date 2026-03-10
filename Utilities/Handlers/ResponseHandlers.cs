using System.Text.Json.Serialization;

namespace RestaurantFoods.Utilities.Handlers;

public class ResponseHandlers<TEntity>
{
    [JsonPropertyName("code")]
    public int Code { get; set; }
    
    [JsonPropertyName("status")]
    public string Status { get; set; } = string.Empty;
    
    [JsonPropertyName("message")]
    public string Message { get; set; } = string.Empty;

    [JsonPropertyName("data")]
    public TEntity? Data { get; set; }
    
    [JsonPropertyName("errorss")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Dictionary<string, List<string>>? Errors { get; set; }

    [JsonPropertyName("meta")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public object? Meta { get; set; }
}
