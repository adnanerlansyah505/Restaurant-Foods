using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantFoods.Migrations
{
    /// <inheritdoc />
    public partial class ChangeColumnNameOtpExpiredAt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "otp_expire_at",
                table: "users",
                newName: "otp_expired_at");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "otp_expired_at",
                table: "users",
                newName: "otp_expire_at");
        }
    }
}
