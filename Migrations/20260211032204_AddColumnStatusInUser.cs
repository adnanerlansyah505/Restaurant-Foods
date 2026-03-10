using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantFoods.Migrations
{
    /// <inheritdoc />
    public partial class AddColumnStatusInUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "status",
                table: "users",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "status",
                table: "users");
        }
    }
}
