using System.ComponentModel.DataAnnotations.Schema;
using RestaurantFoods.Models;
using RestaurantFoods.Utilities.Enum;

namespace RestaurantFoods.Models.Data;

[Table("orders")]
public class Order : BaseEntity
{
    [Column("order_date")]
    public required DateTime OrderDate { get; set; }

    [Column("status")]
    public required TypeStatusOrder Status { get; set; }

    [Column("total_amount")]
    public required int TotalAmount { get; set; }

    // Foreign key
    [Column("user_id")]
    public Guid UserId { get; set; }
    // Navigation Property
    public User User { get; set; } = null!;

    // ✅ One Order -> Many OrderItems
    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    
    // ✅ One Order -> Many Payments
    public ICollection<Payment> Payments { get; set; } = new List<Payment>();
}