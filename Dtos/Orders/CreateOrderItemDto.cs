
namespace RestaurantFoods.Dtos.Orders;

public class CreateOrderItemDto
{
    public Guid FoodId { get; set; }
    public int Quantity { get; set; }
}