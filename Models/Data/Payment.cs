using System.ComponentModel.DataAnnotations.Schema;
using RestaurantFoods.Models;
using RestaurantFoods.Utilities.Enum;

namespace RestaurantFoods.Models.Data;

[Table("payments")]
public class Payment : BaseEntity
{
    [Column("payment_date")]
    public DateTime? PaymentDate { get; set; }

    [Column("payment_method")]
    public string? PaymentMethod { get; set; }
    
    [Column("payment_status")]
    public required PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Pending;

    [Column("va_number", TypeName = "nvarchar(255)")]
    public string? VaNumber { get; set; }

    [Column("amount_paid")]
    public required decimal AmountPaid { get; set; }
    
    [Column("transaction_reference")]
    public string? TransactionReference { get; set; }

    // Foreign key
    [Column("order_id")]
    public Guid OrderId { get; set; }
    public Order Order { get; set; } = null!;
}