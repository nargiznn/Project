using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repository.Migrations
{
    public partial class CreateProductTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Ingredient = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FoodCategoryProduct",
                columns: table => new
                {
                    FoodCategoriesId = table.Column<int>(type: "int", nullable: false),
                    ProductsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FoodCategoryProduct", x => new { x.FoodCategoriesId, x.ProductsId });
                    table.ForeignKey(
                        name: "FK_FoodCategoryProduct_FoodCategories_FoodCategoriesId",
                        column: x => x.FoodCategoriesId,
                        principalTable: "FoodCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FoodCategoryProduct_Products_ProductsId",
                        column: x => x.ProductsId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateTable(
                name: "ProductSpecialCategory",
                columns: table => new
                {
                    ProductsId = table.Column<int>(type: "int", nullable: false),
                    SpecialCategoriesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductSpecialCategory", x => new { x.ProductsId, x.SpecialCategoriesId });
                    table.ForeignKey(
                        name: "FK_ProductSpecialCategory_Products_ProductsId",
                        column: x => x.ProductsId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductSpecialCategory_SpecialCategories_SpecialCategoriesId",
                        column: x => x.SpecialCategoriesId,
                        principalTable: "SpecialCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FoodCategoryProduct_ProductsId",
                table: "FoodCategoryProduct",
                column: "ProductsId");

            migrationBuilder.CreateIndex(
                name: "IX_MenuCategoryProduct_ProductsId",
                table: "MenuCategoryProduct",
                column: "ProductsId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductSpecialCategory_SpecialCategoriesId",
                table: "ProductSpecialCategory",
                column: "SpecialCategoriesId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FoodCategoryProduct");

            migrationBuilder.DropTable(
                name: "MenuCategoryProduct");

            migrationBuilder.DropTable(
                name: "ProductSpecialCategory");

            migrationBuilder.DropTable(
                name: "Products");
        }
    }
}
