using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace SharpSeer.Models;

public partial class SharpSeerDBContext : DbContext
{
    public SharpSeerDBContext()
    {
    }

    public SharpSeerDBContext(DbContextOptions<SharpSeerDBContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Cohort> Cohorts { get; set; }

    public virtual DbSet<Exam> Exams { get; set; }

    public virtual DbSet<Teacher> Teachers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=SharpSeer;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cohort>(entity =>
        {
            Console.WriteLine("foo");
            entity.HasKey(e => e.Id).HasName("PK__Cohorts__3214EC274EE60854");
        });

        modelBuilder.Entity<Exam>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Exams__3214EC27BEA3130B");

            entity.HasOne(d => d.Cohort).WithOne(p => p.Exam)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Exams__CohortID__3D5E1FD2");

            entity.HasMany(d => d.Teachers).WithMany(p => p.Exams)
                .UsingEntity<Dictionary<string, object>>(
                    "Examiner",
                    r => r.HasOne<Teacher>().WithMany()
                        .HasForeignKey("TeacherId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__Examiners__Teach__4316F928"),
                    l => l.HasOne<Exam>().WithMany()
                        .HasForeignKey("ExamId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__Examiners__ExamI__4222D4EF"),
                    j =>
                    {
                        j.HasKey("ExamId", "TeacherId").HasName("PK__Examiner__77AA0451684B2945");
                        j.ToTable("Examiners");
                    });
        });

        modelBuilder.Entity<Teacher>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Teachers__3214EC272498E425");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
