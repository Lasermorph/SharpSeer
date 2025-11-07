using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace SharpSeer.Models;

public partial class SharpSeerDbContext : DbContext
{
    private string ? connectionString;
    public SharpSeerDbContext()
    {

    }

    public SharpSeerDbContext(DbContextOptions<SharpSeerDbContext> options, IConfiguration config)
        : base(options)
    {
        connectionString = config.GetConnectionString("SharpSeerDB");
    }

    public virtual DbSet<Cohort> Cohorts { get; set; }

    public virtual DbSet<Exam> Exams { get; set; }

    public virtual DbSet<Teacher> Teachers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer(connectionString);
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cohort>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Cohorts__3214EC2782EC5191");
        });

        modelBuilder.Entity<Exam>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Exams__3214EC278F1EB990");

            entity.HasMany(d => d.Cohorts).WithMany(p => p.Exams)
                .UsingEntity<Dictionary<string, object>>(
                    "ExamCohort",
                    r => r.HasOne<Cohort>().WithMany()
                        .HasForeignKey("CohortId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__ExamCohor__Cohor__571DF1D5"),
                    l => l.HasOne<Exam>().WithMany()
                        .HasForeignKey("ExamId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__ExamCohor__ExamI__5629CD9C"),
                    j =>
                    {
                        j.HasKey("ExamId", "CohortId").HasName("PK__ExamCoho__CDD7092837E1C6D8");
                        j.ToTable("ExamCohorts");
                        j.IndexerProperty<int>("ExamId").HasColumnName("ExamID");
                        j.IndexerProperty<int>("CohortId").HasColumnName("CohortID");
                    });

            entity.HasMany(d => d.Teachers).WithMany(p => p.Exams)
                .UsingEntity<Dictionary<string, object>>(
                    "ExamTeacher",
                    r => r.HasOne<Teacher>().WithMany()
                        .HasForeignKey("TeacherId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__ExamTeach__Teach__534D60F1"),
                    l => l.HasOne<Exam>().WithMany()
                        .HasForeignKey("ExamId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__ExamTeach__ExamI__52593CB8"),
                    j =>
                    {
                        j.HasKey("ExamId", "TeacherId").HasName("PK__ExamTeac__77AA04338FA9F551");
                        j.ToTable("ExamTeachers");
                        j.IndexerProperty<int>("ExamId").HasColumnName("ExamID");
                        j.IndexerProperty<int>("TeacherId").HasColumnName("TeacherID");
                    });
        });

        modelBuilder.Entity<Teacher>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Teachers__3214EC2722ABD1B8");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
