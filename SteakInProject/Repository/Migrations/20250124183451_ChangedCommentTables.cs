using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repository.Migrations
{
    public partial class ChangedCommentTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EventId",
                table: "CommentReplies",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CommentReplies_EventId",
                table: "CommentReplies",
                column: "EventId");

            migrationBuilder.AddForeignKey(
                name: "FK_CommentReplies_Events_EventId",
                table: "CommentReplies",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CommentReplies_Events_EventId",
                table: "CommentReplies");

            migrationBuilder.DropIndex(
                name: "IX_CommentReplies_EventId",
                table: "CommentReplies");

            migrationBuilder.DropColumn(
                name: "EventId",
                table: "CommentReplies");
        }
    }
}
