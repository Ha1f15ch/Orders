using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApplicationDbContext.Migrations
{
    /// <inheritdoc />
    public partial class addPriKey_OrderStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderStatus",
                schema: "meta",
                table: "OrderStatus");

            migrationBuilder.DropColumn(
                name: "Id",
                schema: "meta",
                table: "OrderStatus");

            migrationBuilder.DropColumn(
                name: "OrderStatusId",
                schema: "dbo",
                table: "Order");

            migrationBuilder.RenameColumn(
                name: "ShortName",
                schema: "meta",
                table: "OrderStatus",
                newName: "Description");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "meta",
                table: "OrderStatus",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "PerformerId",
                schema: "dbo",
                table: "Order",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                schema: "dbo",
                table: "Order",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DeletedDate",
                schema: "dbo",
                table: "Order",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<string>(
                name: "OrderStatus",
                schema: "dbo",
                table: "Order",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderStatus",
                schema: "meta",
                table: "OrderStatus",
                column: "Name");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderStatus",
                schema: "meta",
                table: "OrderStatus");

            migrationBuilder.DropColumn(
                name: "OrderStatus",
                schema: "dbo",
                table: "Order");

            migrationBuilder.RenameColumn(
                name: "Description",
                schema: "meta",
                table: "OrderStatus",
                newName: "ShortName");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "meta",
                table: "OrderStatus",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                schema: "meta",
                table: "OrderStatus",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<int>(
                name: "PerformerId",
                schema: "dbo",
                table: "Order",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                schema: "dbo",
                table: "Order",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DeletedDate",
                schema: "dbo",
                table: "Order",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OrderStatusId",
                schema: "dbo",
                table: "Order",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderStatus",
                schema: "meta",
                table: "OrderStatus",
                column: "Id");
        }
    }
}
