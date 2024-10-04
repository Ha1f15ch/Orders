using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApplicationDbContext.Migrations
{
    /// <inheritdoc />
    public partial class modify_Collection_ChatEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_ChatRooms_ChatId",
                schema: "dbo",
                table: "ChatRooms",
                column: "ChatId");

            migrationBuilder.AddForeignKey(
                name: "FK_ChatRooms_Chats_ChatId",
                schema: "dbo",
                table: "ChatRooms",
                column: "ChatId",
                principalSchema: "dbo",
                principalTable: "Chats",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChatRooms_Chats_ChatId",
                schema: "dbo",
                table: "ChatRooms");

            migrationBuilder.DropIndex(
                name: "IX_ChatRooms_ChatId",
                schema: "dbo",
                table: "ChatRooms");
        }
    }
}
