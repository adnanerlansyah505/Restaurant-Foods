using System.ComponentModel.DataAnnotations.Schema;
using RestaurantFoods.Models;

namespace RestaurantFoods.Models.Data;

[Table("roles")]
public class Role : BaseEntity
{

    [Column("slug", TypeName = "nvarchar(255)")]
    public required string Slug { get; set; }

    [Column("name", TypeName = "nvarchar(100)")]
    public required string Name { get; set; }
    
    // Navigation property
    public ICollection<User> Users { get; set; } = new List<User>();
}