using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repository.Migrations
{
    public partial class CustomerTableUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_SpecialCategories_SpecialCategoryId",
                table: "Products");

            migrationBuilder.AlterColumn<int>(
                name: "SpecialCategoryId",
                table: "Products",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "ReviewType",
                table: "Customers",
                type: "int",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_SpecialCategories_SpecialCategoryId",
                table: "Products",
                column: "SpecialCategoryId",
                principalTable: "SpecialCategories",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_SpecialCategories_SpecialCategoryId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ReviewType",
                table: "Customers");

            migrationBuilder.AlterColumn<int>(
                name: "SpecialCategoryId",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_SpecialCategories_SpecialCategoryId",
                table: "Products",
                column: "SpecialCategoryId",
                principalTable: "SpecialCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
