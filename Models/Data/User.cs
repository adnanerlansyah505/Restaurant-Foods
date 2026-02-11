using System.ComponentModel.DataAnnotations.Schema;
using RestaurantFoods.Models;

namespace RestaurantFoods.Models.Data;

[Table("users")]
public class User : BaseEntity
{

    [Column("username", TypeName = "nvarchar(255)")]
    public required string Username { get; set; }

    [Column("email", TypeName = "nvarchar(255)")]
    public required string Email { get; set; }

    [Column("name", TypeName = "nvarchar(100)")]
    public required string Name { get; set; }

    [Column("email_verification_token", TypeName = "nvarchar(255)")]
    public string? EmailVerificationToken { get; set; }

    [Column("email_verification_expires")]
    public DateTime? EmailVerificationExpires { get; set; }
    
    [Column("is_email_verified")]
    public bool IsEmailVerified { get; set; } = false;

    [Column("status")]
    public bool Status { get; set; } = true;

    [Column("password", TypeName = "nvarchar(255)")]
    public required string Password { get; set; }
    
    // Foreign key
    [Column("role_id")]
    public Guid RoleId { get; set; }

    // Navigation property
    public Role Role { get; set; } = null!;
}