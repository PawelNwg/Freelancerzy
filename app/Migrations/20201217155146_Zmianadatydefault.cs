using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace freelancerzy.Migrations
{
    public partial class Zmianadatydefault : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "registrationDate",
                table: "PageUser",
                type: "datetime",
                nullable: false,
                defaultValueSql: "NOW()",
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValue: new DateTime(2020, 12, 17, 16, 41, 26, 784, DateTimeKind.Local).AddTicks(3865));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "registrationDate",
                table: "PageUser",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(2020, 12, 17, 16, 41, 26, 784, DateTimeKind.Local).AddTicks(3865),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValueSql: "NOW()");
        }
    }
}
