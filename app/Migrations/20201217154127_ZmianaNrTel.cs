using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace freelancerzy.Migrations
{
    public partial class ZmianaNrTel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "registrationDate",
                table: "PageUser",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(2020, 12, 17, 16, 41, 26, 784, DateTimeKind.Local).AddTicks(3865),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValue: new DateTime(2020, 12, 17, 16, 14, 20, 927, DateTimeKind.Local).AddTicks(9213));

            migrationBuilder.AlterColumn<string>(
                name: "phonenumber",
                table: "PageUser",
                unicode: false,
                maxLength: 12,
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int(11)",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "registrationDate",
                table: "PageUser",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(2020, 12, 17, 16, 14, 20, 927, DateTimeKind.Local).AddTicks(9213),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValue: new DateTime(2020, 12, 17, 16, 41, 26, 784, DateTimeKind.Local).AddTicks(3865));

            migrationBuilder.AlterColumn<int>(
                name: "phonenumber",
                table: "PageUser",
                type: "int(11)",
                nullable: true,
                oldClrType: typeof(string),
                oldUnicode: false,
                oldMaxLength: 12,
                oldNullable: true);
        }
    }
}
