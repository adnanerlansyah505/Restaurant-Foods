using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantFoods.Migrations
{
    /// <inheritdoc />
    public partial class AddColumnVANumberPayment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "va_number",
                table: "payments",
                type: "nvarchar(255)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "va_number",
                table: "payments");
        }
    }
}
