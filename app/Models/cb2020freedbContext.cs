using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace freelancerzy.Models
{
    public partial class cb2020freedbContext : DbContext
    {
        public cb2020freedbContext()
        {
        }

        public cb2020freedbContext(DbContextOptions<cb2020freedbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Category> Category { get; set; }
        public virtual DbSet<Credentials> Credentials { get; set; }
        public virtual DbSet<Message> Message { get; set; }
        public virtual DbSet<Messagereport> Messagereport { get; set; }
        public virtual DbSet<Offer> Offer { get; set; }
        public virtual DbSet<PageUser> PageUser { get; set; }
        public virtual DbSet<Permission> Permission { get; set; }
        public virtual DbSet<Permissionuser> Permissionuser { get; set; }
        public virtual DbSet<Reason> Reason { get; set; }
        public virtual DbSet<Useraddress> Useraddress { get; set; }
        public virtual DbSet<Usertype> Usertype { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseMySQL("server=212.33.90.100;port=3306;user=cb2020freeuser;password=cb2020freePASS;database=cb2020freedb");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("category");

                entity.Property(e => e.Categoryid)
                    .HasColumnName("categoryid")
                    .HasColumnType("int(11)");

                entity.Property(e => e.CategoryName)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Credentials>(entity =>
            {
                entity.HasKey(e => e.Userid)
                    .HasName("PRIMARY");

                entity.ToTable("credentials");

                entity.HasIndex(e => e.Userid)
                    .HasName("credentials__idx")
                    .IsUnique();

                entity.Property(e => e.Userid)
                    .HasColumnName("userid")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasColumnName("password")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.HasOne(d => d.User)
                    .WithOne(p => p.Credentials)
                    .HasForeignKey<Credentials>(d => d.Userid)
                    .HasConstraintName("credentials_user_fk");
            });

            modelBuilder.Entity<Message>(entity =>
            {
                entity.ToTable("message");

                entity.HasIndex(e => e.UserFromId)
                    .HasName("message_user_fk");

                entity.HasIndex(e => e.UserToId)
                    .HasName("message_user_fkv2");

                entity.Property(e => e.Messageid)
                    .HasColumnName("messageid")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Content)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.UserFromId).HasColumnType("int(11)");

                entity.Property(e => e.UserToId).HasColumnType("int(11)");

                entity.HasOne(d => d.UserFrom)
                    .WithMany(p => p.MessageUserFrom)
                    .HasForeignKey(d => d.UserFromId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("message_user_fk");

                entity.HasOne(d => d.UserTo)
                    .WithMany(p => p.MessageUserTo)
                    .HasForeignKey(d => d.UserToId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("message_user_fkv2");
            });

            modelBuilder.Entity<Messagereport>(entity =>
            {
                entity.HasKey(e => e.Reportid)
                    .HasName("PRIMARY");

                entity.ToTable("messagereport");

                entity.HasIndex(e => e.MessageId)
                    .HasName("messagereport_message_fk");

                entity.HasIndex(e => e.ReasonId)
                    .HasName("messagereport_reason_fk");

                entity.Property(e => e.Reportid)
                    .HasColumnName("reportid")
                    .HasColumnType("int(11)");

                entity.Property(e => e.MessageId).HasColumnType("int(11)");

                entity.Property(e => e.ReasonId).HasColumnType("int(11)");

                entity.HasOne(d => d.Message)
                    .WithMany(p => p.Messagereport)
                    .HasForeignKey(d => d.MessageId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("messagereport_message_fk");

                entity.HasOne(d => d.Reason)
                    .WithMany(p => p.Messagereport)
                    .HasForeignKey(d => d.ReasonId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("messagereport_reason_fk");
            });

            modelBuilder.Entity<Offer>(entity =>
            {
                entity.ToTable("offer");

                entity.HasIndex(e => e.CategoryId)
                    .HasName("offer_category_fk");

                entity.HasIndex(e => e.UserId)
                    .HasName("offer_user_fk");

                entity.Property(e => e.Offerid)
                    .HasColumnName("offerid")
                    .HasColumnType("int(11)");

                entity.Property(e => e.CategoryId).HasColumnType("int(11)");

                entity.Property(e => e.CreationDate).HasColumnType("datetime(3)");

                entity.Property(e => e.Description)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.ExpirationDate).HasColumnType("datetime(3)");

                entity.Property(e => e.LastModificationDate).HasColumnType("datetime(3)");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(40)
                    .IsUnicode(false);

                entity.Property(e => e.UserId).HasColumnType("int(11)");

                entity.Property(e => e.ViewCounter).HasColumnType("int(11)");

                entity.Property(e => e.Wage)
                    .HasColumnName("wage")
                    .HasColumnType("decimal(10,0)");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Offer)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("offer_category_fk");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Offer)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("offer_user_fk");
            });

            modelBuilder.Entity<PageUser>(entity =>
            {
                entity.HasKey(e => e.Userid)
                    .HasName("PRIMARY");

                entity.HasIndex(e => e.EmailAddress)
                    .HasName("user__unv2")
                    .IsUnique();

                entity.HasIndex(e => e.TypeId)
                    .HasName("user_usertype_fk");

                entity.Property(e => e.Userid)
                    .HasColumnName("userid")
                    .HasColumnType("int(11)");

                entity.Property(e => e.EmailAddress)
                    .IsRequired()
                    .HasMaxLength(40)
                    .IsUnicode(false);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Phonenumber)
                    .HasColumnName("phonenumber")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Surname)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.TypeId).HasColumnType("int(11)");

                entity.HasOne(d => d.Type)
                    .WithMany(p => p.PageUser)
                    .HasForeignKey(d => d.TypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("user_usertype_fk");
            });

            modelBuilder.Entity<Permission>(entity =>
            {
                entity.ToTable("permission");

                entity.Property(e => e.Permissionid)
                    .HasColumnName("permissionid")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(25)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Permissionuser>(entity =>
            {
                entity.HasKey(e => new { e.Userid, e.Permissionid })
                    .HasName("PRIMARY");

                entity.ToTable("permissionuser");

                entity.HasIndex(e => e.Permissionid)
                    .HasName("permissionuser_permission_fk");

                entity.Property(e => e.Userid)
                    .HasColumnName("userid")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Permissionid)
                    .HasColumnName("permissionid")
                    .HasColumnType("int(11)");

                entity.HasOne(d => d.Permission)
                    .WithMany(p => p.Permissionuser)
                    .HasForeignKey(d => d.Permissionid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("permissionuser_permission_fk");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Permissionuser)
                    .HasForeignKey(d => d.Userid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("permissionuser_user_fk");
            });

            modelBuilder.Entity<Reason>(entity =>
            {
                entity.ToTable("reason");

                entity.Property(e => e.Reasonid)
                    .HasColumnName("reasonid")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Description)
                    .HasColumnName("description")
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Useraddress>(entity =>
            {
                entity.HasKey(e => e.Userid)
                    .HasName("PRIMARY");

                entity.ToTable("useraddress");

                entity.HasIndex(e => e.Userid)
                    .HasName("useraddress__idx")
                    .IsUnique();

                entity.Property(e => e.Userid)
                    .HasColumnName("userid")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ApartmentNumber).HasColumnType("int(11)");

                entity.Property(e => e.City)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Number).HasColumnType("int(11)");

                entity.Property(e => e.Street)
                    .IsRequired()
                    .HasColumnName("street")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.ZipCode)
                    .HasMaxLength(6)
                    .IsUnicode(false);

                entity.HasOne(d => d.User)
                    .WithOne(p => p.Useraddress)
                    .HasForeignKey<Useraddress>(d => d.Userid)
                    .HasConstraintName("useraddress_user_fk");
            });

            modelBuilder.Entity<Usertype>(entity =>
            {
                entity.HasKey(e => e.Typeid)
                    .HasName("PRIMARY");

                entity.ToTable("usertype");

                entity.Property(e => e.Typeid)
                    .HasColumnName("typeid")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
