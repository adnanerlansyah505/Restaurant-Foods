using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantFoods.Migrations
{
    /// <inheritdoc />
    public partial class CategoryIdNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_foods_categories_categoryId",
                table: "foods");

            migrationBuilder.AlterColumn<Guid>(
                name: "categoryId",
                table: "foods",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_foods_categories_categoryId",
                table: "foods",
                column: "categoryId",
                principalTable: "categories",
                principalColumn: "guid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_foods_categories_categoryId",
                table: "foods");

            migrationBuilder.AlterColumn<Guid>(
                name: "categoryId",
                table: "foods",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_foods_categories_categoryId",
                table: "foods",
                column: "categoryId",
                principalTable: "categories",
                principalColumn: "guid",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
