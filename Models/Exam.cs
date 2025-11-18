using System;
using System.Collections.Generic;

namespace SharpSeer.Models;

public partial class Exam
{
    public enum ExamTypeEnum
    {
        Skriftlig = 1,
        Mundtlig = 2,
        Projekt = 3,
    }
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int ExamType { get; set; }

    public bool IsGuarded { get; set; }

    public bool NeedExternalExaminer { get; set; }

    public DateTime FirstExamDate { get; set; }

    public DateTime LastExamDate { get; set; }

    public DateTime? HandInDeadline { get; set; }

    public int DurationInMinutes { get; set; }

    public virtual ICollection<Cohort> Cohorts { get; set; } = new List<Cohort>();

    public virtual ICollection<Teacher> Teachers { get; set; } = new List<Teacher>();
}
