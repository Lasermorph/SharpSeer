using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SharpSeer.Models;

public partial class Exam
{
    public enum ExamTypeEnum
    {
        Skriftlig = 1,
        Mundtlig = 2,
        Projekt = 3,
    }

    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string Name { get; set; } = null!;

    public int ExamType { get; set; }

    public bool IsGuarded { get; set; }

    public bool NeedExternalExaminer { get; set; }

    public DateTime FirstExamDate { get; set; }

    public DateTime LastExamDate { get; set; }

    public DateTime? HandInDeadline { get; set; }

    public int DurationInMinutes { get; set; }
    
    [ForeignKey("ExamId")]
    [InverseProperty("Exams")]
    public virtual ICollection<Cohort> Cohorts { get; set; } = new List<Cohort>();

    [ForeignKey("ExamId")]
    [InverseProperty("Exams")]
    public virtual ICollection<Teacher> Teachers { get; set; } = new List<Teacher>();

}
