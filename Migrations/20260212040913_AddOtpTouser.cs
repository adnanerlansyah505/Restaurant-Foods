using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantFoods.Migrations
{
    /// <inheritdoc />
    public partial class AddOtpTouser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "otp_code",
                table: "users",
                type: "nvarchar(255)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "otp_expire_at",
                table: "users",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "otp_code",
                table: "users");

            migrationBuilder.DropColumn(
                name: "otp_expire_at",
                table: "users");
        }
    }
}
