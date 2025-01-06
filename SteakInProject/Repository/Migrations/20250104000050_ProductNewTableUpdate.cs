using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repository.Migrations
{
    public partial class ProductNewTableUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FoodCategoryProduct");

            migrationBuilder.DropTable(
                name: "ProductSpecialCategory");

            migrationBuilder.DropIndex(
                name: "IX_ProductCuisines_ProductId",
                table: "ProductCuisines");

            migrationBuilder.AddColumn<int>(
                name: "FoodCategoryId",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ProductCuisineId",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SpecialCategoryId",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Products_FoodCategoryId",
                table: "Products",
                column: "FoodCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_SpecialCategoryId",
                table: "Products",
                column: "SpecialCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductCuisines_ProductId",
                table: "ProductCuisines",
                column: "ProductId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_FoodCategories_FoodCategoryId",
                table: "Products",
                column: "FoodCategoryId",
                principalTable: "FoodCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_SpecialCategories_SpecialCategoryId",
                table: "Products",
                column: "SpecialCategoryId",
                principalTable: "SpecialCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_FoodCategories_FoodCategoryId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_SpecialCategories_SpecialCategoryId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_FoodCategoryId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_SpecialCategoryId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_ProductCuisines_ProductId",
                table: "ProductCuisines");

            migrationBuilder.DropColumn(
                name: "FoodCategoryId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ProductCuisineId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "SpecialCategoryId",
                table: "Products");

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
                name: "IX_ProductCuisines_ProductId",
                table: "ProductCuisines",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_FoodCategoryProduct_ProductsId",
                table: "FoodCategoryProduct",
                column: "ProductsId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductSpecialCategory_SpecialCategoriesId",
                table: "ProductSpecialCategory",
                column: "SpecialCategoriesId");
        }
    }
}
