using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApplicationDbContext.Migrations
{
    /// <inheritdoc />
    public partial class correctTableForMappings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CategoryProfessionMappings_Categories_CategoryId",
                table: "CategoryProfessionMappings");

            migrationBuilder.DropForeignKey(
                name: "FK_CategoryProfessionMappings_Professions_ProfessionId",
                table: "CategoryProfessionMappings");

            migrationBuilder.DropTable(
                name: "ServiceCategoryMappings");

            migrationBuilder.DropIndex(
                name: "IX_CategoryProfessionMappings_CategoryId",
                table: "CategoryProfessionMappings");

            migrationBuilder.DropIndex(
                name: "IX_CategoryProfessionMappings_ProfessionId",
                table: "CategoryProfessionMappings");

            migrationBuilder.CreateTable(
                name: "ProfessionServiceMappings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ServiceId = table.Column<int>(type: "int", nullable: false),
                    ProfessionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProfessionServiceMappings", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProfessionServiceMappings");

            migrationBuilder.CreateTable(
                name: "ServiceCategoryMappings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    ServiceId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceCategoryMappings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ServiceCategoryMappings_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ServiceCategoryMappings_Services_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Services",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CategoryProfessionMappings_CategoryId",
                table: "CategoryProfessionMappings",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_CategoryProfessionMappings_ProfessionId",
                table: "CategoryProfessionMappings",
                column: "ProfessionId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceCategoryMappings_CategoryId",
                table: "ServiceCategoryMappings",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceCategoryMappings_ServiceId",
                table: "ServiceCategoryMappings",
                column: "ServiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_CategoryProfessionMappings_Categories_CategoryId",
                table: "CategoryProfessionMappings",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CategoryProfessionMappings_Professions_ProfessionId",
                table: "CategoryProfessionMappings",
                column: "ProfessionId",
                principalTable: "Professions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
