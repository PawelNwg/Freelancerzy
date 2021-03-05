using Microsoft.EntityFrameworkCore.Migrations;

namespace freelancerzy.Migrations
{
    public partial class addofferreport2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "offerReport_offer_fk",
                table: "OfferReport");

            migrationBuilder.DropForeignKey(
                name: "offerReport_reason_FK",
                table: "OfferReport");

            migrationBuilder.DropForeignKey(
                name: "offerReport_ReportingUser_FK",
                table: "OfferReport");

            migrationBuilder.AddForeignKey(
                name: "offerReport_offer_fk",
                table: "OfferReport",
                column: "OfferId",
                principalTable: "offer",
                principalColumn: "offerid");

            migrationBuilder.AddForeignKey(
                name: "offerReport_reason_FK",
                table: "OfferReport",
                column: "ReasonId",
                principalTable: "OfferReportReason",
                principalColumn: "ReasonId");

            migrationBuilder.AddForeignKey(
                name: "offerReport_ReportingUser_FK",
                table: "OfferReport",
                column: "ReportingUserId",
                principalTable: "PageUser",
                principalColumn: "userid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "offerReport_offer_fk",
                table: "OfferReport");

            migrationBuilder.DropForeignKey(
                name: "offerReport_reason_FK",
                table: "OfferReport");

            migrationBuilder.DropForeignKey(
                name: "offerReport_ReportingUser_FK",
                table: "OfferReport");

            migrationBuilder.AddForeignKey(
                name: "offerReport_offer_fk",
                table: "OfferReport",
                column: "OfferId",
                principalTable: "offer",
                principalColumn: "offerid",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "offerReport_reason_FK",
                table: "OfferReport",
                column: "ReasonId",
                principalTable: "OfferReportReason",
                principalColumn: "ReasonId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "offerReport_ReportingUser_FK",
                table: "OfferReport",
                column: "ReportingUserId",
                principalTable: "PageUser",
                principalColumn: "userid",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
