using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repository.Migrations
{
    public partial class LunchSetProductsTableCreated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LunchSetProducts",
                columns: table => new
                {
                    LunchSetId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LunchSetProducts", x => new { x.LunchSetId, x.ProductId });
                    table.ForeignKey(
                        name: "FK_LunchSetProducts_LunchSets_LunchSetId",
                        column: x => x.LunchSetId,
                        principalTable: "LunchSets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LunchSetProducts_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MealPackageProducts",
                columns: table => new
                {
                    MealPackageId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MealPackageProducts", x => new { x.MealPackageId, x.ProductId });
                    table.ForeignKey(
                        name: "FK_MealPackageProducts_MealPackages_MealPackageId",
                        column: x => x.MealPackageId,
                        principalTable: "MealPackages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MealPackageProducts_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LunchSetProducts_ProductId",
                table: "LunchSetProducts",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_MealPackageProducts_ProductId",
                table: "MealPackageProducts",
                column: "ProductId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LunchSetProducts");

            migrationBuilder.DropTable(
                name: "MealPackageProducts");
        }
    }
}
