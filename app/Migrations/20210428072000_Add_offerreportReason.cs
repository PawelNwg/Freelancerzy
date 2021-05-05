using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace freelancerzy.Migrations
{
    public partial class Add_offerreportReason : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OfferReportReason",
                table: "userreport");

            migrationBuilder.AddColumn<int>(
                name: "ReportReasonId",
                table: "userreport",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "UserReportReasons",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserReportReasons", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_userreport_ReportReasonId",
                table: "userreport",
                column: "ReportReasonId");

            migrationBuilder.AddForeignKey(
                name: "FK_userreport_UserReportReasons_ReportReasonId",
                table: "userreport",
                column: "ReportReasonId",
                principalTable: "UserReportReasons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_userreport_UserReportReasons_ReportReasonId",
                table: "userreport");

            migrationBuilder.DropTable(
                name: "UserReportReasons");

            migrationBuilder.DropIndex(
                name: "IX_userreport_ReportReasonId",
                table: "userreport");

            migrationBuilder.DropColumn(
                name: "ReportReasonId",
                table: "userreport");

            migrationBuilder.AddColumn<int>(
                name: "OfferReportReason",
                table: "userreport",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
