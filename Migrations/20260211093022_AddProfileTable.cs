using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantFoods.Migrations
{
    /// <inheritdoc />
    public partial class AddProfileTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "profiles",
                columns: table => new
                {
                    guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    gender = table.Column<string>(type: "nvarchar(20)", nullable: true),
                    birth_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    place_of_birth = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    phone_number = table.Column<string>(type: "nvarchar(20)", nullable: true),
                    address = table.Column<string>(type: "nvarchar(255)", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_profiles", x => x.guid);
                    table.ForeignKey(
                        name: "FK_profiles_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "guid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_profiles_user_id",
                table: "profiles",
                column: "user_id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "profiles");
        }
    }
}
