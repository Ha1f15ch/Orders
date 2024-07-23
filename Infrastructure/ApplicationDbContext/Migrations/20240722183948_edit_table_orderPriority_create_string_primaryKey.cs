using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApplicationDbContext.Migrations
{
    /// <inheritdoc />
    public partial class edit_table_orderPriority_create_string_primaryKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderPriority",
                schema: "meta",
                table: "OrderPriority");

            migrationBuilder.DropColumn(
                name: "Id",
                schema: "meta",
                table: "OrderPriority");

            migrationBuilder.DropColumn(
                name: "OrderPriorityId",
                schema: "dbo",
                table: "Order");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "meta",
                table: "OrderPriority",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                schema: "meta",
                table: "OrderPriority",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OrderPriority",
                schema: "dbo",
                table: "Order",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderPriority",
                schema: "meta",
                table: "OrderPriority",
                column: "Name");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderPriority",
                schema: "meta",
                table: "OrderPriority");

            migrationBuilder.DropColumn(
                name: "Description",
                schema: "meta",
                table: "OrderPriority");

            migrationBuilder.DropColumn(
                name: "OrderPriority",
                schema: "dbo",
                table: "Order");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "meta",
                table: "OrderPriority",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                schema: "meta",
                table: "OrderPriority",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "OrderPriorityId",
                schema: "dbo",
                table: "Order",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderPriority",
                schema: "meta",
                table: "OrderPriority",
                column: "Id");
        }
    }
}
