using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.Data.EntityFrameworkCore.Metadata;

namespace freelancerzy.Migrations
{
    public partial class test : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "category",
                columns: table => new
                {
                    categoryid = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    CategoryName = table.Column<string>(unicode: false, maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_category", x => x.categoryid);
                });

            migrationBuilder.CreateTable(
                name: "permission",
                columns: table => new
                {
                    permissionid = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(unicode: false, maxLength: 25, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_permission", x => x.permissionid);
                });

            migrationBuilder.CreateTable(
                name: "reason",
                columns: table => new
                {
                    reasonid = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(unicode: false, maxLength: 20, nullable: false),
                    description = table.Column<string>(unicode: false, maxLength: 150, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_reason", x => x.reasonid);
                });

            migrationBuilder.CreateTable(
                name: "usertype",
                columns: table => new
                {
                    typeid = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(unicode: false, maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.typeid);
                });

            migrationBuilder.CreateTable(
                name: "PageUser",
                columns: table => new
                {
                    userid = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    TypeId = table.Column<int>(type: "int(11)", nullable: false),
                    FirstName = table.Column<string>(unicode: false, maxLength: 20, nullable: false),
                    Surname = table.Column<string>(unicode: false, maxLength: 30, nullable: false),
                    EmailAddress = table.Column<string>(unicode: false, maxLength: 40, nullable: false),
                    phonenumber = table.Column<int>(type: "int(11)", nullable: true),
                    emailConfirmation = table.Column<bool>(type: "boolean", nullable: false, defaultValueSql: "false"),
                    registrationDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValue: new DateTime(2020, 12, 17, 16, 14, 20, 927, DateTimeKind.Local).AddTicks(9213))
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.userid);
                    table.ForeignKey(
                        name: "user_usertype_fk",
                        column: x => x.TypeId,
                        principalTable: "usertype",
                        principalColumn: "typeid",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "credentials",
                columns: table => new
                {
                    userid = table.Column<int>(type: "int(11)", nullable: false),
                    password = table.Column<string>(unicode: false, maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.userid);
                    table.ForeignKey(
                        name: "credentials_user_fk",
                        column: x => x.userid,
                        principalTable: "PageUser",
                        principalColumn: "userid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "message",
                columns: table => new
                {
                    messageid = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    UserFromId = table.Column<int>(type: "int(11)", nullable: false),
                    UserToId = table.Column<int>(type: "int(11)", nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    Content = table.Column<string>(unicode: false, maxLength: 250, nullable: false),
                    Status = table.Column<string>(unicode: false, maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_message", x => x.messageid);
                    table.ForeignKey(
                        name: "message_user_fk",
                        column: x => x.UserFromId,
                        principalTable: "PageUser",
                        principalColumn: "userid",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "message_user_fkv2",
                        column: x => x.UserToId,
                        principalTable: "PageUser",
                        principalColumn: "userid",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "offer",
                columns: table => new
                {
                    offerid = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<int>(type: "int(11)", nullable: false),
                    CategoryId = table.Column<int>(type: "int(11)", nullable: false),
                    Title = table.Column<string>(unicode: false, maxLength: 40, nullable: false),
                    Description = table.Column<string>(unicode: false, maxLength: 1500, nullable: true),
                    CreationDate = table.Column<DateTime>(type: "datetime(3)", nullable: false),
                    LastModificationDate = table.Column<DateTime>(type: "datetime(3)", nullable: true),
                    ExpirationDate = table.Column<DateTime>(type: "datetime(3)", nullable: false),
                    ViewCounter = table.Column<int>(type: "int(11)", nullable: false),
                    wage = table.Column<decimal>(type: "decimal(10,0)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_offer", x => x.offerid);
                    table.ForeignKey(
                        name: "offer_category_fk",
                        column: x => x.CategoryId,
                        principalTable: "category",
                        principalColumn: "categoryid",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "offer_user_fk",
                        column: x => x.UserId,
                        principalTable: "PageUser",
                        principalColumn: "userid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "permissionuser",
                columns: table => new
                {
                    userid = table.Column<int>(type: "int(11)", nullable: false),
                    permissionid = table.Column<int>(type: "int(11)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => new { x.userid, x.permissionid });
                    table.ForeignKey(
                        name: "permissionuser_permission_fk",
                        column: x => x.permissionid,
                        principalTable: "permission",
                        principalColumn: "permissionid",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "permissionuser_user_fk",
                        column: x => x.userid,
                        principalTable: "PageUser",
                        principalColumn: "userid",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "useraddress",
                columns: table => new
                {
                    userid = table.Column<int>(type: "int(11)", nullable: false),
                    street = table.Column<string>(unicode: false, maxLength: 30, nullable: false),
                    Number = table.Column<int>(type: "int(11)", nullable: false),
                    ApartmentNumber = table.Column<int>(type: "int(11)", nullable: true),
                    ZipCode = table.Column<string>(unicode: false, maxLength: 6, nullable: true),
                    City = table.Column<string>(unicode: false, maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.userid);
                    table.ForeignKey(
                        name: "useraddress_user_fk",
                        column: x => x.userid,
                        principalTable: "PageUser",
                        principalColumn: "userid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "messagereport",
                columns: table => new
                {
                    reportid = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    MessageId = table.Column<int>(type: "int(11)", nullable: false),
                    ReasonId = table.Column<int>(type: "int(11)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.reportid);
                    table.ForeignKey(
                        name: "messagereport_message_fk",
                        column: x => x.MessageId,
                        principalTable: "message",
                        principalColumn: "messageid",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "messagereport_reason_fk",
                        column: x => x.ReasonId,
                        principalTable: "reason",
                        principalColumn: "reasonid",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "credentials__idx",
                table: "credentials",
                column: "userid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "message_user_fk",
                table: "message",
                column: "UserFromId");

            migrationBuilder.CreateIndex(
                name: "message_user_fkv2",
                table: "message",
                column: "UserToId");

            migrationBuilder.CreateIndex(
                name: "messagereport_message_fk",
                table: "messagereport",
                column: "MessageId");

            migrationBuilder.CreateIndex(
                name: "messagereport_reason_fk",
                table: "messagereport",
                column: "ReasonId");

            migrationBuilder.CreateIndex(
                name: "offer_category_fk",
                table: "offer",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "offer_user_fk",
                table: "offer",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "user__unv2",
                table: "PageUser",
                column: "EmailAddress",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "user_usertype_fk",
                table: "PageUser",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "permissionuser_permission_fk",
                table: "permissionuser",
                column: "permissionid");

            migrationBuilder.CreateIndex(
                name: "useraddress__idx",
                table: "useraddress",
                column: "userid",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "credentials");

            migrationBuilder.DropTable(
                name: "messagereport");

            migrationBuilder.DropTable(
                name: "offer");

            migrationBuilder.DropTable(
                name: "permissionuser");

            migrationBuilder.DropTable(
                name: "useraddress");

            migrationBuilder.DropTable(
                name: "message");

            migrationBuilder.DropTable(
                name: "reason");

            migrationBuilder.DropTable(
                name: "category");

            migrationBuilder.DropTable(
                name: "permission");

            migrationBuilder.DropTable(
                name: "PageUser");

            migrationBuilder.DropTable(
                name: "usertype");
        }
    }
}
