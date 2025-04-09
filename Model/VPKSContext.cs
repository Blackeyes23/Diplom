using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Diplom.Model
{
    public partial class VPKSContext : DbContext
    {
        public VPKSContext()
        {
        }

        public VPKSContext(DbContextOptions<VPKSContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Answers> Answers { get; set; }
        public virtual DbSet<Documents> Documents { get; set; }
        public virtual DbSet<Gruppa> Gruppa { get; set; }
        public virtual DbSet<Role> Role { get; set; }
        public virtual DbSet<Subjects> Subjects { get; set; }
        public virtual DbSet<TestCategory> TestCategory { get; set; }
        public virtual DbSet<TestGroups> TestGroups { get; set; }
        public virtual DbSet<TestResults> TestResults { get; set; }
        public virtual DbSet<Tests> Tests { get; set; }
        public virtual DbSet<Users> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=VPKS;Trusted_Connection=False;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Answers>(entity =>
            {
                entity.Property(e => e.Text)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.HasOne(d => d.Test)
                    .WithMany(p => p.Answers)
                    .HasForeignKey(d => d.TestId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Answers__TestId__6FE99F9F");
            });

            modelBuilder.Entity<Documents>(entity =>
            {
                entity.Property(e => e.FilePath)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.UploadDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Uploader)
                    .WithMany(p => p.Documents)
                    .HasForeignKey(d => d.UploaderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Documents__Uploa__4AB81AF0");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Subjects>(entity =>
            {
                entity.HasIndex(e => e.Name)
                    .HasName("UQ__Subjects__737584F6B8279B1D")
                    .IsUnique();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<TestCategory>(entity =>
            {
                entity.HasIndex(e => e.Name)
                    .HasName("UQ__TestCate__737584F6FC2A9C1D")
                    .IsUnique();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<TestGroups>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.TestGroups)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("FK_TestGroups_TestCategory");
            });

            modelBuilder.Entity<TestResults>(entity =>
            {
                entity.Property(e => e.GivenAnswer)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.HasOne(d => d.Test)
                    .WithMany(p => p.TestResults)
                    .HasForeignKey(d => d.TestId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__TestResul__TestI__6383C8BA");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.TestResults)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TestResults_Users");
            });

            modelBuilder.Entity<Tests>(entity =>
            {
                entity.Property(e => e.CorrectAnswer)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.Question)
                    .IsRequired()
                    .HasMaxLength(1000);

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Tests)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("FK_Tests_TestCategory");

                entity.HasOne(d => d.Subject)
                    .WithMany(p => p.Tests)
                    .HasForeignKey(d => d.SubjectId)
                    .HasConstraintName("FK__Tests__SubjectId__5FB337D6");

                entity.HasOne(d => d.TestGroup)
                    .WithMany(p => p.Tests)
                    .HasForeignKey(d => d.TestGroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Tests_TestGroup");
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.Property(e => e.Login)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Surname)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.Gruppa)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.GruppaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Users_Gruppa");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Users_Role");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
