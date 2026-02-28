using System.ComponentModel.DataAnnotations.Schema;
using RestaurantFoods.Models;

namespace RestaurantFoods.Models.Data;

[Table("order_items")]
public class OrderItem : BaseEntity
{
    [Column("quantity")]
    public required int Quantity { get; set; }

    [Column("unit_price")]
    public required int UnitPrice { get; set; }

    [Column("subtotal")]
    public required int Subtotal { get; set; }
    
    // Foreign key
    [Column("order_id")]
    public Guid OrderId { get; set; }
    public Order Order { get; set; } = null!;

    // Foreign key
    [Column("food_id")]
    public Guid FoodId { get; set; }
    public Food Food { get; set; } = null!;

}