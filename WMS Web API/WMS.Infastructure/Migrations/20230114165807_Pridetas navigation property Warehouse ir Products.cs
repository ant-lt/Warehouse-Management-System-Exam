using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WMS.Infastructure.Migrations
{
    public partial class PridetasnavigationpropertyWarehouseirProducts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: 1,
                column: "Created",
                value: new DateTime(2023, 1, 14, 18, 58, 6, 578, DateTimeKind.Local).AddTicks(9175));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: 1,
                column: "Created",
                value: new DateTime(2023, 1, 10, 18, 40, 9, 337, DateTimeKind.Local).AddTicks(1347));
        }
    }
}
