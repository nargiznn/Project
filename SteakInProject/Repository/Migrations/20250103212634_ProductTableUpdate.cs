using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repository.Migrations
{
    public partial class ProductTableUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChefImages_Products_ProductId",
                table: "ChefImages");

            migrationBuilder.DropTable(
                name: "MenuCategoryProduct");

            migrationBuilder.DropIndex(
                name: "IX_ChefImages_ProductId",
                table: "ChefImages");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "ChefImages");

            migrationBuilder.AddColumn<int>(
                name: "MenuCategoryId",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Products_MenuCategoryId",
                table: "Products",
                column: "MenuCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_MenuCategories_MenuCategoryId",
                table: "Products",
                column: "MenuCategoryId",
                principalTable: "MenuCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_MenuCategories_MenuCategoryId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_MenuCategoryId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "MenuCategoryId",
                table: "Products");

            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "ChefImages",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "MenuCategoryProduct",
                columns: table => new
                {
                    MenuCategoriesId = table.Column<int>(type: "int", nullable: false),
                    ProductsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenuCategoryProduct", x => new { x.MenuCategoriesId, x.ProductsId });
                    table.ForeignKey(
                        name: "FK_MenuCategoryProduct_MenuCategories_MenuCategoriesId",
                        column: x => x.MenuCategoriesId,
                        principalTable: "MenuCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MenuCategoryProduct_Products_ProductsId",
                        column: x => x.ProductsId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChefImages_ProductId",
                table: "ChefImages",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_MenuCategoryProduct_ProductsId",
                table: "MenuCategoryProduct",
                column: "ProductsId");

            migrationBuilder.AddForeignKey(
                name: "FK_ChefImages_Products_ProductId",
                table: "ChefImages",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id");
        }
    }
}
