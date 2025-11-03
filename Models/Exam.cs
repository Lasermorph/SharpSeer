using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SharpSeer.Models;

[Index("CohortId", Name = "UQ__Exams__4A2288FEE1096D4E", IsUnique = true)]
public class Exam
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string Name { get; set; } = null!;

    [Column("CohortID")]
    public int CohortId { get; set; }

    public bool IsGuarded { get; set; }

    public bool NeedsExternalExaminer { get; set; }

    public DateTime FirstExamDate { get; set; }

    public DateTime LastExamDate { get; set; }

    public DateTime? HandInDeadline { get; set; }

    public int ExamDurationMinutes { get; set; }

    public int ExamType { get; set; }

    [ForeignKey("CohortId")]
    [InverseProperty("Exam")]
    public virtual Cohort Cohort { get; set; } = null!;

    [ForeignKey("ExamId")]
    [InverseProperty("Exams")]
    public virtual ICollection<Teacher> Teachers { get; set; } = new List<Teacher>();

    public Exam()
    {
    }

    public Exam(string name, int cohortId, bool isGuarded, bool needsExternalExaminer, DateTime firstExamDate, DateTime lastExamDate, DateTime? handInDeadline, int examDurationMinutes, int examType)
    {
        Name = name;
        CohortId = cohortId;
        IsGuarded = isGuarded;
        NeedsExternalExaminer = needsExternalExaminer;
        FirstExamDate = firstExamDate;
        LastExamDate = lastExamDate;
        HandInDeadline = handInDeadline;
        ExamDurationMinutes = examDurationMinutes;
        ExamType = examType;
    }
}
