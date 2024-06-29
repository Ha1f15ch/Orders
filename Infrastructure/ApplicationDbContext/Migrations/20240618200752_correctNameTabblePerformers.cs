using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApplicationDbContext.Migrations
{
    /// <inheritdoc />
    public partial class correctNameTabblePerformers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PerformerServicesMapping_Performsers_PerformerId",
                table: "PerformerServicesMapping");

            migrationBuilder.DropForeignKey(
                name: "FK_Performsers_Users_UserId",
                table: "Performsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Performsers",
                table: "Performsers");

            migrationBuilder.RenameTable(
                name: "Performsers",
                newName: "Performers");

            migrationBuilder.RenameIndex(
                name: "IX_Performsers_UserId",
                table: "Performers",
                newName: "IX_Performers_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Performers",
                table: "Performers",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Performers_Users_UserId",
                table: "Performers",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PerformerServicesMapping_Performers_PerformerId",
                table: "PerformerServicesMapping",
                column: "PerformerId",
                principalTable: "Performers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Performers_Users_UserId",
                table: "Performers");

            migrationBuilder.DropForeignKey(
                name: "FK_PerformerServicesMapping_Performers_PerformerId",
                table: "PerformerServicesMapping");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Performers",
                table: "Performers");

            migrationBuilder.RenameTable(
                name: "Performers",
                newName: "Performsers");

            migrationBuilder.RenameIndex(
                name: "IX_Performers_UserId",
                table: "Performsers",
                newName: "IX_Performsers_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Performsers",
                table: "Performsers",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PerformerServicesMapping_Performsers_PerformerId",
                table: "PerformerServicesMapping",
                column: "PerformerId",
                principalTable: "Performsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Performsers_Users_UserId",
                table: "Performsers",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
