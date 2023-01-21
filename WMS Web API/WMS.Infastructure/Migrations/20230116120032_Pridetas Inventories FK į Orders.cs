using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WMS.Infastructure.Migrations
{
    public partial class PridetasInventoriesFKįOrders : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OrderId",
                table: "Inventories",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: 1,
                column: "Created",
                value: new DateTime(2023, 1, 16, 14, 0, 31, 416, DateTimeKind.Local).AddTicks(2604));

            migrationBuilder.CreateIndex(
                name: "IX_Inventories_OrderId",
                table: "Inventories",
                column: "OrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Inventories_Orders_OrderId",
                table: "Inventories",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Inventories_Orders_OrderId",
                table: "Inventories");

            migrationBuilder.DropIndex(
                name: "IX_Inventories_OrderId",
                table: "Inventories");

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "Inventories");

            migrationBuilder.UpdateData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: 1,
                column: "Created",
                value: new DateTime(2023, 1, 14, 18, 58, 6, 578, DateTimeKind.Local).AddTicks(9175));
        }
    }
}
