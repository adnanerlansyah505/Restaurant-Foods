using System.ComponentModel.DataAnnotations.Schema;
using RestaurantFoods.Models;

namespace RestaurantFoods.Models.Data;

[Table("foods")]
public class Food : BaseEntity
{
    [Column("name", TypeName = "nvarchar(255)")]
    public required string Name { get; set; }

    [Column("description")]
    public string? Description { get; set; }

    [Column("price")]
    public required int Price { get; set; }

    [Column("cost")]
    public int? Cost { get; set; }

    [Column("categoryId")]
    public Guid? CategoryId { get; set; }

    [Column("isAvailable")]
    public bool isAvailable { get; set; } = true;

    // Navigation property
    public Category? Category { get; set; }

    // One Food -> Many OrderItems
    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}