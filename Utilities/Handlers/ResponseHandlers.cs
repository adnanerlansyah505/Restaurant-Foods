namespace RestaurantFoods.Utilities.Handlers;

public class ResponseHandlers<TEntity>
{
    public int Code { get; set; }
    public string Status { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public TEntity? Data { get; set; }
    public object? Meta { get; set; }
    public Dictionary<string, List<string>>? Errors { get; set; }
}
