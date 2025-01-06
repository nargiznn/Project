using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repository.Migrations
{
    public partial class ProductAndProductCuisineTableUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductCuisine_Cuisines_CuisineId",
                table: "ProductCuisine");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductCuisine_Products_ProductId",
                table: "ProductCuisine");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductCuisine",
                table: "ProductCuisine");

            migrationBuilder.RenameTable(
                name: "ProductCuisine",
                newName: "ProductCuisines");

            migrationBuilder.RenameIndex(
                name: "IX_ProductCuisine_ProductId",
                table: "ProductCuisines",
                newName: "IX_ProductCuisines_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductCuisine_CuisineId",
                table: "ProductCuisines",
                newName: "IX_ProductCuisines_CuisineId");

            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "ChefImages",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductCuisines",
                table: "ProductCuisines",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "ProductImages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Path = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductImages_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChefImages_ProductId",
                table: "ChefImages",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductImages_ProductId",
                table: "ProductImages",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_ChefImages_Products_ProductId",
                table: "ChefImages",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductCuisines_Cuisines_CuisineId",
                table: "ProductCuisines",
                column: "CuisineId",
                principalTable: "Cuisines",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductCuisines_Products_ProductId",
                table: "ProductCuisines",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChefImages_Products_ProductId",
                table: "ChefImages");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductCuisines_Cuisines_CuisineId",
                table: "ProductCuisines");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductCuisines_Products_ProductId",
                table: "ProductCuisines");

            migrationBuilder.DropTable(
                name: "ProductImages");

            migrationBuilder.DropIndex(
                name: "IX_ChefImages_ProductId",
                table: "ChefImages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductCuisines",
                table: "ProductCuisines");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "ChefImages");

            migrationBuilder.RenameTable(
                name: "ProductCuisines",
                newName: "ProductCuisine");

            migrationBuilder.RenameIndex(
                name: "IX_ProductCuisines_ProductId",
                table: "ProductCuisine",
                newName: "IX_ProductCuisine_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductCuisines_CuisineId",
                table: "ProductCuisine",
                newName: "IX_ProductCuisine_CuisineId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductCuisine",
                table: "ProductCuisine",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductCuisine_Cuisines_CuisineId",
                table: "ProductCuisine",
                column: "CuisineId",
                principalTable: "Cuisines",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductCuisine_Products_ProductId",
                table: "ProductCuisine",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
