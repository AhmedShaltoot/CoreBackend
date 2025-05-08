using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace RFIDDAL.Models
{
    public partial class RFIDdbContext : DbContext
    {
        public RFIDdbContext()
        {
        }

        public RFIDdbContext(DbContextOptions<RFIDdbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AspNetRole> AspNetRoles { get; set; } = null!;
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; } = null!;
        public virtual DbSet<AspNetUserRole> AspNetUserRoles { get; set; } = null!;
        public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; } = null!;
        public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; } = null!;
        public virtual DbSet<MethodsMonitor> MethodsMonitors { get; set; } = null!;
        public virtual DbSet<AssetType> AssetTypes { get; set; } = null!;
        public virtual DbSet<CategoryType> CategoryTypes { get; set; } = null!;
        public virtual DbSet<Permission> Permissions { get; set; } = null!;
        public virtual DbSet<RolePermission> RolePermissions { get; set; } = null!;
        public virtual DbSet<Status> Statuses { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=88.198.107.225,1433\\\\\\\\MSSQLSERVER2019;Database=Nayef_RFID_db;User Id=siru_us;Password=SiruApp@2024;Trusted_Connection=False;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AspNetRole>(entity =>
            {
                entity.Property(e => e.Id).HasMaxLength(128);

                entity.Property(e => e.Name).HasMaxLength(256);

                entity.Property(e => e.NormalizedName).HasMaxLength(256);
            });

            modelBuilder.Entity<AspNetUser>(entity =>
            {
                entity.ToTable("AspNetUsers", "dbo");

                entity.Property(e => e.Id).HasMaxLength(128);

                entity.Property(e => e.DeletedDate).HasColumnType("datetime");

                entity.Property(e => e.Email).HasMaxLength(256);

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.RoleName).HasMaxLength(50);

                entity.Property(e => e.UserId).ValueGeneratedOnAddOrUpdate();

                entity.Property(e => e.UserName)
                                    .IsRequired()
                                    .HasMaxLength(256);
                //entity.HasMany(d => d.Roles)
                //    .WithMany(p => p.Users)
                //    .UsingEntity<Dictionary<string, object>>(
                //        "AspNetUserRole",
                //        l => l.HasOne<AspNetRole>().WithMany().HasForeignKey("RoleId").HasConstraintName("FK_dbo.AspNetUserRoles_dbo.AspNetRoles_RoleId"),
                //        r => r.HasOne<AspNetUser>().WithMany().HasForeignKey("UserId").HasConstraintName("FK_dbo.AspNetUserRoles_dbo.AspNetUsers_UserId"),
                //        j =>
                //        {
                //            j.HasKey("UserId", "RoleId").HasName("PK_dbo.AspNetUserRoles");

                //            j.ToTable("AspNetUserRoles");

                //            j.IndexerProperty<string>("UserId").HasMaxLength(128);

                //            j.IndexerProperty<string>("RoleId").HasMaxLength(128);
                //        });
            });
            modelBuilder.Entity<AspNetUserRole>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.RoleId })
                    .HasName("PK_dbo.AspNetUserRoles");

                entity.ToTable("AspNetUserRoles", "dbo");

                entity.Property(e => e.UserId).HasMaxLength(128);

                entity.Property(e => e.RoleId).HasMaxLength(128);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetUserRoles)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK_dbo.AspNetUserRoles_dbo.AspNetRoles_RoleId");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserRoles)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_dbo.AspNetUserRoles_dbo.AspNetUsers_UserId");
            });

            modelBuilder.Entity<AspNetUserClaim>(entity =>
            {
                entity.Property(e => e.UserId).HasMaxLength(128);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserClaims)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_dbo.AspNetUserClaims_dbo.AspNetUsers_UserId");
            });

            modelBuilder.Entity<AspNetUserLogin>(entity =>
            {
                entity.HasKey(e => new { e.LoginProvider, e.ProviderKey, e.UserId })
                    .HasName("PK_dbo.AspNetUserLogins");

                entity.Property(e => e.LoginProvider).HasMaxLength(128);

                entity.Property(e => e.ProviderKey).HasMaxLength(128);

                entity.Property(e => e.UserId).HasMaxLength(128);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserLogins)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_dbo.AspNetUserLogins_dbo.AspNetUsers_UserId");
            });

            modelBuilder.Entity<AssetType>(entity =>
            {
                entity.Property(e => e.AssetTypeName).HasMaxLength(150);

                entity.Property(e => e.CreationDate).HasColumnType("datetime");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.LastUpdatedDate).HasColumnType("datetime");

                entity.Property(e => e.ParentAssetTypeName).HasMaxLength(150);

                entity.Property(e => e.UniversityName).HasMaxLength(150);
            });


            modelBuilder.Entity<CategoryType>(entity =>
            {
                entity.Property(e => e.CategoryTypeName).HasMaxLength(100);

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");
            });
            modelBuilder.Entity<MethodsMonitor>(entity =>
            {
                entity.HasKey(e => e.MonitorId)
                    .HasName("PK_MethodsMonitor_1");

                entity.ToTable("MethodsMonitor");

                entity.Property(e => e.ExcutionTime).HasMaxLength(50);

                entity.Property(e => e.RunningDate).HasColumnType("datetime");
            });
            modelBuilder.Entity<Permission>(entity =>
            {
                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.PageName).HasMaxLength(150);

                entity.Property(e => e.PageNameAr)
                    .HasMaxLength(150)
                    .HasColumnName("PageName_ar");

                entity.Property(e => e.PermissionName).HasMaxLength(150);

                entity.Property(e => e.PermissionNameAr)
                    .HasMaxLength(150)
                    .HasColumnName("PermissionName_ar");
            });

            modelBuilder.Entity<RolePermission>(entity =>
            {
                entity.HasKey(e => new { e.PermissionId, e.RoleId })
                    .HasName("PK_dbo.RolePermissions");

                entity.Property(e => e.RoleId).HasMaxLength(128);

                entity.HasOne(d => d.Permission)
                    .WithMany(p => p.RolePermissions)
                    .HasForeignKey(d => d.PermissionId)
                    .HasConstraintName("FK_dbo.RolePermissions_dbo.Permissions_PermissionId");
            });

            modelBuilder.Entity<Status>(entity =>
            {
                entity.Property(e => e.StatusNameAr)
                    .HasMaxLength(100)
                    .HasColumnName("StatusName_Ar");

                entity.Property(e => e.StatusNameEn)
                    .HasMaxLength(100)
                    .HasColumnName("StatusName_En");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
