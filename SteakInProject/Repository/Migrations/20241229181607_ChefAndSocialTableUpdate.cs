using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repository.Migrations
{
    public partial class ChefAndSocialTableUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChefImage_Chefs_ChefId",
                table: "ChefImage");

            migrationBuilder.DropForeignKey(
                name: "FK_ChefPosition_Chefs_ChefId",
                table: "ChefPosition");

            migrationBuilder.DropForeignKey(
                name: "FK_ChefPosition_Position_PositionId",
                table: "ChefPosition");

            migrationBuilder.DropForeignKey(
                name: "FK_Chefs_SocialMediaLink_SocialMediaId",
                table: "Chefs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SocialMediaLink",
                table: "SocialMediaLink");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Position",
                table: "Position");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ChefPosition",
                table: "ChefPosition");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ChefImage",
                table: "ChefImage");

            migrationBuilder.RenameTable(
                name: "SocialMediaLink",
                newName: "SocialMediaLinks");

            migrationBuilder.RenameTable(
                name: "Position",
                newName: "Positions");

            migrationBuilder.RenameTable(
                name: "ChefPosition",
                newName: "ChefPositions");

            migrationBuilder.RenameTable(
                name: "ChefImage",
                newName: "ChefImages");

            migrationBuilder.RenameIndex(
                name: "IX_ChefPosition_PositionId",
                table: "ChefPositions",
                newName: "IX_ChefPositions_PositionId");

            migrationBuilder.RenameIndex(
                name: "IX_ChefPosition_ChefId",
                table: "ChefPositions",
                newName: "IX_ChefPositions_ChefId");

            migrationBuilder.RenameIndex(
                name: "IX_ChefImage_ChefId",
                table: "ChefImages",
                newName: "IX_ChefImages_ChefId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SocialMediaLinks",
                table: "SocialMediaLinks",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Positions",
                table: "Positions",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ChefPositions",
                table: "ChefPositions",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ChefImages",
                table: "ChefImages",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ChefImages_Chefs_ChefId",
                table: "ChefImages",
                column: "ChefId",
                principalTable: "Chefs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ChefPositions_Chefs_ChefId",
                table: "ChefPositions",
                column: "ChefId",
                principalTable: "Chefs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ChefPositions_Positions_PositionId",
                table: "ChefPositions",
                column: "PositionId",
                principalTable: "Positions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Chefs_SocialMediaLinks_SocialMediaId",
                table: "Chefs",
                column: "SocialMediaId",
                principalTable: "SocialMediaLinks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChefImages_Chefs_ChefId",
                table: "ChefImages");

            migrationBuilder.DropForeignKey(
                name: "FK_ChefPositions_Chefs_ChefId",
                table: "ChefPositions");

            migrationBuilder.DropForeignKey(
                name: "FK_ChefPositions_Positions_PositionId",
                table: "ChefPositions");

            migrationBuilder.DropForeignKey(
                name: "FK_Chefs_SocialMediaLinks_SocialMediaId",
                table: "Chefs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SocialMediaLinks",
                table: "SocialMediaLinks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Positions",
                table: "Positions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ChefPositions",
                table: "ChefPositions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ChefImages",
                table: "ChefImages");

            migrationBuilder.RenameTable(
                name: "SocialMediaLinks",
                newName: "SocialMediaLink");

            migrationBuilder.RenameTable(
                name: "Positions",
                newName: "Position");

            migrationBuilder.RenameTable(
                name: "ChefPositions",
                newName: "ChefPosition");

            migrationBuilder.RenameTable(
                name: "ChefImages",
                newName: "ChefImage");

            migrationBuilder.RenameIndex(
                name: "IX_ChefPositions_PositionId",
                table: "ChefPosition",
                newName: "IX_ChefPosition_PositionId");

            migrationBuilder.RenameIndex(
                name: "IX_ChefPositions_ChefId",
                table: "ChefPosition",
                newName: "IX_ChefPosition_ChefId");

            migrationBuilder.RenameIndex(
                name: "IX_ChefImages_ChefId",
                table: "ChefImage",
                newName: "IX_ChefImage_ChefId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SocialMediaLink",
                table: "SocialMediaLink",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Position",
                table: "Position",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ChefPosition",
                table: "ChefPosition",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ChefImage",
                table: "ChefImage",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ChefImage_Chefs_ChefId",
                table: "ChefImage",
                column: "ChefId",
                principalTable: "Chefs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ChefPosition_Chefs_ChefId",
                table: "ChefPosition",
                column: "ChefId",
                principalTable: "Chefs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ChefPosition_Position_PositionId",
                table: "ChefPosition",
                column: "PositionId",
                principalTable: "Position",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Chefs_SocialMediaLink_SocialMediaId",
                table: "Chefs",
                column: "SocialMediaId",
                principalTable: "SocialMediaLink",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
