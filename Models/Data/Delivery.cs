using System.ComponentModel.DataAnnotations.Schema;
using RestaurantFoods.Models;
using RestaurantFoods.Utilities.Enum;

namespace RestaurantFoods.Models.Data;

[Table("deliveries")]
public class Delivery : BaseEntity
{
    [Column("delivery_address")]
    public required string DeliveryAddress { get; set; }

    [Column("delivery_status")]
    public required string DeliveryStatus { get; set; }
    
    [Column("delivery_time")]
    public required DateTime DeliveryTime { get; set; }

    [Column("driver_name")]
    public required string DriverName { get; set; }

    // Foreign key
    [Column("order_id")]
    public Guid OrderId { get; set; }
    public Order Order { get; set; } = null!;
}