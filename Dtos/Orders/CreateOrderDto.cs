
namespace RestaurantFoods.Dtos.Orders;

public class CreateOrderDto
{
    public List<CreateOrderItemDto> Items { get; set; } = new();
}