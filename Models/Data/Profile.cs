using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantFoods.Models.Data;

[Table("profiles")]
public class Profile : BaseEntity
{
    [Column("user_id")]
    public Guid UserId { get; set; }

    [Column("gender", TypeName = "nvarchar(20)")]
    public string? Gender { get; set; }

    [Column("birth_date")]
    public DateTime? BirthDate { get; set; }

    [Column("place_of_birth", TypeName = "nvarchar(100)")]
    public string? PlaceOfBirth { get; set; }

    [Column("phone_number", TypeName = "nvarchar(20)")]
    public string? PhoneNumber { get; set; }

    [Column("address", TypeName = "nvarchar(255)")]
    public string? Address { get; set; }

    // Navigation
    public User User { get; set; } = null!;
}