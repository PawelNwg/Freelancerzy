using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace freelancerzy.Migrations
{
    public partial class PrzywrocenieUserReport : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "userreport",
                columns: table => new
                {
                    ReportId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserReportedId = table.Column<int>(nullable: false),
                    UserReporterId = table.Column<int>(nullable: false),
                    ReportDate = table.Column<DateTime>(nullable: true),
                    ReasonId = table.Column<int>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    OfferReportReasonReasonId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.ReportId);
                    table.ForeignKey(
                        name: "FK_userreport_OfferReportReason_OfferReportReasonReasonId",
                        column: x => x.OfferReportReasonReasonId,
                        principalTable: "OfferReportReason",
                        principalColumn: "ReasonId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_userreport_PageUser_UserReportedId",
                        column: x => x.UserReportedId,
                        principalTable: "PageUser",
                        principalColumn: "userid");
                    table.ForeignKey(
                        name: "FK_userreport_PageUser_UserReporterId",
                        column: x => x.UserReporterId,
                        principalTable: "PageUser",
                        principalColumn: "userid");
                });

            migrationBuilder.CreateIndex(
                name: "IX_userreport_OfferReportReasonReasonId",
                table: "userreport",
                column: "OfferReportReasonReasonId");

            migrationBuilder.CreateIndex(
                name: "IX_userreport_UserReportedId",
                table: "userreport",
                column: "UserReportedId");

            migrationBuilder.CreateIndex(
                name: "IX_userreport_UserReporterId",
                table: "userreport",
                column: "UserReporterId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "userreport");
        }
    }
}
