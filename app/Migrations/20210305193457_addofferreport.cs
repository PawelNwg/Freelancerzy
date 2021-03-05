using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;


namespace freelancerzy.Migrations
{
    public partial class addofferreport : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OfferReportReason",
                columns: table => new
                {
                    ReasonId = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("OfferReportReasonPK", x => x.ReasonId);
                });

            migrationBuilder.CreateTable(
                name: "OfferReport",
                columns: table => new
                {
                    ReportId = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    OfferId = table.Column<int>(type: "int(11)", nullable: false),
                    ReportingUserId = table.Column<int>(type: "int(11)", nullable: false),
                    ReportDate = table.Column<DateTime>(type: "datetime(3)", nullable: false),
                    ReasonId = table.Column<int>(type: "int(11)", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValueSql: "true")
                },
                constraints: table =>
                {
                    table.PrimaryKey("OfferReportPK", x => x.ReportId);
                    table.ForeignKey(
                        name: "offerReport_offer_fk",
                        column: x => x.OfferId,
                        principalTable: "offer",
                        principalColumn: "offerid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "offerReport_reason_FK",
                        column: x => x.ReasonId,
                        principalTable: "OfferReportReason",
                        principalColumn: "ReasonId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "offerReport_ReportingUser_FK",
                        column: x => x.ReportingUserId,
                        principalTable: "PageUser",
                        principalColumn: "userid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OfferReport_OfferId",
                table: "OfferReport",
                column: "OfferId");

            migrationBuilder.CreateIndex(
                name: "IX_OfferReport_ReasonId",
                table: "OfferReport",
                column: "ReasonId");

            migrationBuilder.CreateIndex(
                name: "IX_OfferReport_ReportingUserId",
                table: "OfferReport",
                column: "ReportingUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OfferReport");

            migrationBuilder.DropTable(
                name: "OfferReportReason");
        }
    }
}
