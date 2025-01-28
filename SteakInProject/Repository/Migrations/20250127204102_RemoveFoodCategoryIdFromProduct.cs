using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repository.Migrations
{
    public partial class RemoveFoodCategoryIdFromProduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_FoodCategories_FoodCategoryId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_FoodCategoryId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "FoodCategoryId",
                table: "Products");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FoodCategoryId",
                table: "Products",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_FoodCategoryId",
                table: "Products",
                column: "FoodCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_FoodCategories_FoodCategoryId",
                table: "Products",
                column: "FoodCategoryId",
                principalTable: "FoodCategories",
                principalColumn: "Id");
        }
    }
}
