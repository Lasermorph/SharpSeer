using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace SharpSeer.Models;

public partial class VedelslundDkDbSharpSeerDbContext : DbContext
{
    public VedelslundDkDbSharpSeerDbContext()
    {
    }

    public VedelslundDkDbSharpSeerDbContext(DbContextOptions<VedelslundDkDbSharpSeerDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Cohort> Cohorts { get; set; }

    public virtual DbSet<Exam> Exams { get; set; }

    public virtual DbSet<OverlapExamCheck> OverlapExamChecks { get; set; }

    public virtual DbSet<Teacher> Teachers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=mssql3.unoeuro.com;Initial Catalog=vedelslund_dk_db_sharp_seer_db;User ID=vedelslund_dk;Password=p4k9whbaxz5FGD6RBgfe;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cohort>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Cohorts__3214EC27C7D3E5AE");

            entity.HasIndex(e => e.Name, "UC_CohortName").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Major)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Exam>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Exams__3214EC2797C7D7E3");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasMany(d => d.Cohorts).WithMany(p => p.Exams)
                .UsingEntity<Dictionary<string, object>>(
                    "ExamCohort",
                    r => r.HasOne<Cohort>().WithMany()
                        .HasForeignKey("CohortId")
                        .HasConstraintName("FK__ExamCohor__CohortID"),
                    l => l.HasOne<Exam>().WithMany()
                        .HasForeignKey("ExamId")
                        .HasConstraintName("FK__ExamCohor__ExamID"),
                    j =>
                    {
                        j.HasKey("ExamId", "CohortId").HasName("PK__ExamCoho__CDD7092843B24DBB");
                        j.ToTable("ExamCohorts");
                        j.IndexerProperty<int>("ExamId").HasColumnName("ExamID");
                        j.IndexerProperty<int>("CohortId").HasColumnName("CohortID");
                    });

            entity.HasMany(d => d.Teachers).WithMany(p => p.Exams)
                .UsingEntity<Dictionary<string, object>>(
                    "ExamTeacher",
                    r => r.HasOne<Teacher>().WithMany()
                        .HasForeignKey("TeacherId")
                        .HasConstraintName("FK__ExamTeach__TeacherID"),
                    l => l.HasOne<Exam>().WithMany()
                        .HasForeignKey("ExamId")
                        .HasConstraintName("FK__ExamTeach__ExamID"),
                    j =>
                    {
                        j.HasKey("ExamId", "TeacherId").HasName("PK__ExamTeac__77AA0433C824FF4E");
                        j.ToTable("ExamTeachers");
                        j.IndexerProperty<int>("ExamId").HasColumnName("ExamID");
                        j.IndexerProperty<int>("TeacherId").HasColumnName("TeacherID");
                    });
        });

        modelBuilder.Entity<OverlapExamCheck>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__overlap___3213E83FBB2764EC");

            entity.ToTable("overlap_exam_check");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CohortId).HasColumnName("cohortID");
            entity.Property(e => e.ExamId).HasColumnName("examID");
            entity.Property(e => e.TeacherId).HasColumnName("teacherID");

            entity.HasOne(d => d.Cohort).WithMany(p => p.OverlapExamChecks)
                .HasForeignKey(d => d.CohortId)
                .HasConstraintName("FK__overlap_e__cohor__76969D2E");

            entity.HasOne(d => d.Exam).WithMany(p => p.OverlapExamChecks)
                .HasForeignKey(d => d.ExamId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__overlap_e__examI__74AE54BC");

            entity.HasOne(d => d.Teacher).WithMany(p => p.OverlapExamChecks)
                .HasForeignKey(d => d.TeacherId)
                .HasConstraintName("FK__overlap_e__teach__75A278F5");
        });

        modelBuilder.Entity<Teacher>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Teachers__3214EC27913FCC04");

            entity.HasIndex(e => e.Email, "UQ__Teachers__A9D10534F1F7AF2F").IsUnique();

            entity.HasIndex(e => e.NameId, "UQ__Teachers__EE1C17C064D2A295").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.IsExternal).HasDefaultValue(false);
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.NameId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("NameID");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(15)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
