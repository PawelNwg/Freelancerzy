﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using freelancerzy.Models;

namespace freelancerzy.Migrations
{
    [DbContext(typeof(cb2020freedbContext))]
    [Migration("20210305193457_addofferreport")]
    partial class addofferreport
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("freelancerzy.Models.Category", b =>
                {
                    b.Property<int>("Categoryid")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("categoryid")
                        .HasColumnType("int(11)");

                    b.Property<string>("CategoryName")
                        .IsRequired()
                        .HasColumnType("varchar(30)")
                        .HasMaxLength(30)
                        .IsUnicode(false);

                    b.HasKey("Categoryid");

                    b.ToTable("category");
                });

            modelBuilder.Entity("freelancerzy.Models.Credentials", b =>
                {
                    b.Property<int>("Userid")
                        .HasColumnName("userid")
                        .HasColumnType("int(11)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnName("password")
                        .HasColumnType("varchar(200)")
                        .HasMaxLength(200)
                        .IsUnicode(false);

                    b.HasKey("Userid")
                        .HasName("PRIMARY");

                    b.HasIndex("Userid")
                        .IsUnique()
                        .HasName("credentials__idx");

                    b.ToTable("credentials");
                });

            modelBuilder.Entity("freelancerzy.Models.Message", b =>
                {
                    b.Property<int>("Messageid")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("messageid")
                        .HasColumnType("int(11)");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("varchar(250)")
                        .HasMaxLength(250)
                        .IsUnicode(false);

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("varchar(20)")
                        .HasMaxLength(20)
                        .IsUnicode(false);

                    b.Property<int>("UserFromId")
                        .HasColumnType("int(11)");

                    b.Property<int>("UserToId")
                        .HasColumnType("int(11)");

                    b.HasKey("Messageid");

                    b.HasIndex("UserFromId")
                        .HasName("message_user_fk");

                    b.HasIndex("UserToId")
                        .HasName("message_user_fkv2");

                    b.ToTable("message");
                });

            modelBuilder.Entity("freelancerzy.Models.Messagereport", b =>
                {
                    b.Property<int>("Reportid")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("reportid")
                        .HasColumnType("int(11)");

                    b.Property<int>("MessageId")
                        .HasColumnType("int(11)");

                    b.Property<int>("ReasonId")
                        .HasColumnType("int(11)");

                    b.HasKey("Reportid")
                        .HasName("PRIMARY");

                    b.HasIndex("MessageId")
                        .HasName("messagereport_message_fk");

                    b.HasIndex("ReasonId")
                        .HasName("messagereport_reason_fk");

                    b.ToTable("messagereport");
                });

            modelBuilder.Entity("freelancerzy.Models.Offer", b =>
                {
                    b.Property<int>("Offerid")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("offerid")
                        .HasColumnType("int(11)");

                    b.Property<int>("CategoryId")
                        .HasColumnType("int(11)");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime(3)");

                    b.Property<string>("Description")
                        .HasColumnType("varchar(1500)")
                        .HasMaxLength(1500)
                        .IsUnicode(false);

                    b.Property<DateTime>("ExpirationDate")
                        .HasColumnType("datetime(3)");

                    b.Property<bool>("IsReported")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValueSql("false");

                    b.Property<DateTime?>("LastModificationDate")
                        .HasColumnType("datetime(3)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("varchar(40)")
                        .HasMaxLength(40)
                        .IsUnicode(false);

                    b.Property<int>("UserId")
                        .HasColumnType("int(11)");

                    b.Property<int>("ViewCounter")
                        .HasColumnType("int(11)");

                    b.Property<decimal?>("Wage")
                        .HasColumnName("wage")
                        .HasColumnType("decimal(10,2)");

                    b.HasKey("Offerid");

                    b.HasIndex("CategoryId")
                        .HasName("offer_category_fk");

                    b.HasIndex("UserId")
                        .HasName("offer_user_fk");

                    b.ToTable("offer");
                });

            modelBuilder.Entity("freelancerzy.Models.OfferReport", b =>
                {
                    b.Property<int>("ReportId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int(11)")
                        .HasAnnotation("SqlServer:IdentityIncrement", 1)
                        .HasAnnotation("SqlServer:IdentitySeed", 1)
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("IsActive")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValueSql("true");

                    b.Property<int>("OfferId")
                        .HasColumnType("int(11)");

                    b.Property<int>("ReasonId")
                        .HasColumnType("int(11)");

                    b.Property<DateTime?>("ReportDate")
                        .IsRequired()
                        .HasColumnType("datetime(3)");

                    b.Property<int>("ReportingUserId")
                        .HasColumnType("int(11)");

                    b.HasKey("ReportId")
                        .HasName("OfferReportPK");

                    b.HasIndex("OfferId");

                    b.HasIndex("ReasonId");

                    b.HasIndex("ReportingUserId");

                    b.ToTable("OfferReport");
                });

            modelBuilder.Entity("freelancerzy.Models.OfferReportReason", b =>
                {
                    b.Property<int>("ReasonId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int(11)")
                        .HasAnnotation("SqlServer:IdentityIncrement", 1)
                        .HasAnnotation("SqlServer:IdentitySeed", 1)
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("varchar(50)")
                        .HasMaxLength(50);

                    b.HasKey("ReasonId")
                        .HasName("OfferReportReasonPK");

                    b.ToTable("OfferReportReason");
                });

            modelBuilder.Entity("freelancerzy.Models.PageUser", b =>
                {
                    b.Property<int>("Userid")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("userid")
                        .HasColumnType("int(11)");

                    b.Property<string>("EmailAddress")
                        .IsRequired()
                        .HasColumnType("varchar(40)")
                        .HasMaxLength(40)
                        .IsUnicode(false);

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("varchar(20)")
                        .HasMaxLength(20)
                        .IsUnicode(false);

                    b.Property<string>("Phonenumber")
                        .HasColumnName("phonenumber")
                        .HasColumnType("varchar(12)")
                        .HasMaxLength(12);

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasColumnType("varchar(30)")
                        .HasMaxLength(30)
                        .IsUnicode(false);

                    b.Property<int>("TypeId")
                        .HasColumnType("int(11)");

                    b.Property<bool>("emailConfirmation")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValueSql("false");

                    b.Property<DateTime>("registrationDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasDefaultValueSql("NOW()");

                    b.HasKey("Userid")
                        .HasName("PRIMARY");

                    b.HasIndex("EmailAddress")
                        .IsUnique()
                        .HasName("user__unv2");

                    b.HasIndex("TypeId")
                        .HasName("user_usertype_fk");

                    b.ToTable("PageUser");
                });

            modelBuilder.Entity("freelancerzy.Models.Permission", b =>
                {
                    b.Property<int>("Permissionid")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("permissionid")
                        .HasColumnType("int(11)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(25)")
                        .HasMaxLength(25)
                        .IsUnicode(false);

                    b.HasKey("Permissionid");

                    b.ToTable("permission");
                });

            modelBuilder.Entity("freelancerzy.Models.Permissionuser", b =>
                {
                    b.Property<int>("Userid")
                        .HasColumnName("userid")
                        .HasColumnType("int(11)");

                    b.Property<int>("Permissionid")
                        .HasColumnName("permissionid")
                        .HasColumnType("int(11)");

                    b.HasKey("Userid", "Permissionid")
                        .HasName("PRIMARY");

                    b.HasIndex("Permissionid")
                        .HasName("permissionuser_permission_fk");

                    b.ToTable("permissionuser");
                });

            modelBuilder.Entity("freelancerzy.Models.Reason", b =>
                {
                    b.Property<int>("Reasonid")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("reasonid")
                        .HasColumnType("int(11)");

                    b.Property<string>("Description")
                        .HasColumnName("description")
                        .HasColumnType("varchar(150)")
                        .HasMaxLength(150)
                        .IsUnicode(false);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(20)")
                        .HasMaxLength(20)
                        .IsUnicode(false);

                    b.HasKey("Reasonid");

                    b.ToTable("reason");
                });

            modelBuilder.Entity("freelancerzy.Models.Useraddress", b =>
                {
                    b.Property<int>("Userid")
                        .HasColumnName("userid")
                        .HasColumnType("int(11)");

                    b.Property<int?>("ApartmentNumber")
                        .HasColumnType("int(11)");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnType("varchar(30)")
                        .HasMaxLength(30)
                        .IsUnicode(false);

                    b.Property<int>("Number")
                        .HasColumnType("int(11)");

                    b.Property<string>("Street")
                        .IsRequired()
                        .HasColumnName("street")
                        .HasColumnType("varchar(30)")
                        .HasMaxLength(30)
                        .IsUnicode(false);

                    b.Property<string>("ZipCode")
                        .HasColumnType("varchar(6)")
                        .HasMaxLength(6)
                        .IsUnicode(false);

                    b.HasKey("Userid")
                        .HasName("PRIMARY");

                    b.HasIndex("Userid")
                        .IsUnique()
                        .HasName("useraddress__idx");

                    b.ToTable("useraddress");
                });

            modelBuilder.Entity("freelancerzy.Models.Usertype", b =>
                {
                    b.Property<int>("Typeid")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("typeid")
                        .HasColumnType("int(11)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(20)")
                        .HasMaxLength(20)
                        .IsUnicode(false);

                    b.HasKey("Typeid")
                        .HasName("PRIMARY");

                    b.ToTable("usertype");
                });

            modelBuilder.Entity("freelancerzy.Models.Credentials", b =>
                {
                    b.HasOne("freelancerzy.Models.PageUser", "User")
                        .WithOne("Credentials")
                        .HasForeignKey("freelancerzy.Models.Credentials", "Userid")
                        .HasConstraintName("credentials_user_fk")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("freelancerzy.Models.Message", b =>
                {
                    b.HasOne("freelancerzy.Models.PageUser", "UserFrom")
                        .WithMany("MessageUserFrom")
                        .HasForeignKey("UserFromId")
                        .HasConstraintName("message_user_fk")
                        .IsRequired();

                    b.HasOne("freelancerzy.Models.PageUser", "UserTo")
                        .WithMany("MessageUserTo")
                        .HasForeignKey("UserToId")
                        .HasConstraintName("message_user_fkv2")
                        .IsRequired();
                });

            modelBuilder.Entity("freelancerzy.Models.Messagereport", b =>
                {
                    b.HasOne("freelancerzy.Models.Message", "Message")
                        .WithMany("Messagereport")
                        .HasForeignKey("MessageId")
                        .HasConstraintName("messagereport_message_fk")
                        .IsRequired();

                    b.HasOne("freelancerzy.Models.Reason", "Reason")
                        .WithMany("Messagereport")
                        .HasForeignKey("ReasonId")
                        .HasConstraintName("messagereport_reason_fk")
                        .IsRequired();
                });

            modelBuilder.Entity("freelancerzy.Models.Offer", b =>
                {
                    b.HasOne("freelancerzy.Models.Category", "Category")
                        .WithMany("Offer")
                        .HasForeignKey("CategoryId")
                        .HasConstraintName("offer_category_fk")
                        .IsRequired();

                    b.HasOne("freelancerzy.Models.PageUser", "User")
                        .WithMany("Offer")
                        .HasForeignKey("UserId")
                        .HasConstraintName("offer_user_fk")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("freelancerzy.Models.OfferReport", b =>
                {
                    b.HasOne("freelancerzy.Models.Offer", "Offer")
                        .WithMany("OfferReports")
                        .HasForeignKey("OfferId")
                        .HasConstraintName("offerReport_offer_fk")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("freelancerzy.Models.OfferReportReason", "OfferReportReason")
                        .WithMany("OfferReports")
                        .HasForeignKey("ReasonId")
                        .HasConstraintName("offerReport_reason_FK")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("freelancerzy.Models.PageUser", "ReportingUser")
                        .WithMany("OfferReports")
                        .HasForeignKey("ReportingUserId")
                        .HasConstraintName("offerReport_ReportingUser_FK")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("freelancerzy.Models.PageUser", b =>
                {
                    b.HasOne("freelancerzy.Models.Usertype", "Type")
                        .WithMany("PageUser")
                        .HasForeignKey("TypeId")
                        .HasConstraintName("user_usertype_fk")
                        .IsRequired();
                });

            modelBuilder.Entity("freelancerzy.Models.Permissionuser", b =>
                {
                    b.HasOne("freelancerzy.Models.Permission", "Permission")
                        .WithMany("Permissionuser")
                        .HasForeignKey("Permissionid")
                        .HasConstraintName("permissionuser_permission_fk")
                        .IsRequired();

                    b.HasOne("freelancerzy.Models.PageUser", "User")
                        .WithMany("Permissionuser")
                        .HasForeignKey("Userid")
                        .HasConstraintName("permissionuser_user_fk")
                        .IsRequired();
                });

            modelBuilder.Entity("freelancerzy.Models.Useraddress", b =>
                {
                    b.HasOne("freelancerzy.Models.PageUser", "User")
                        .WithOne("Useraddress")
                        .HasForeignKey("freelancerzy.Models.Useraddress", "Userid")
                        .HasConstraintName("useraddress_user_fk")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}