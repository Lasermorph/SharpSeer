using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SharpSeer.Models;

public partial class Exam
{
   public enum ExamTypeEnum
    {
        [Display(Name = "Skriftlig")]
        Skriftlig = 1,
        [Display(Name = "Mundtlig")]
        Mundtlig = 2,
        [Display(Name = "Projekt")]
        Projekt = 3,
        [Display(Name = "Skriftlig Re-Examen")]
        Skriftlig_Re_Examen = 4,
        [Display(Name = "Mundtlig Re-Examen")]
        Mundtlig_Re_Examen = 5,
        [Display(Name = "Projekt Re-Examen")]
        Projekt_Re_Examen = 6,
        [Display(Name = "Afsluttende Examen")]
        Afsluttende = 7,
        [Display(Name = "Afsluttende Re-Examen")]
        Afsluttende_Re_Examen = 8
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

    public int? EstimatedStudentCount { get; set; }

    private string m_examComment;

    public string? ExamComment
    { 
        get { return m_examComment; }
        set 
        { 
            if (value == null || value.Length < 256)
            m_examComment = value;
            
        }
    } 

    public virtual ICollection<Cohort> Cohorts { get; set; } = new List<Cohort>();

    public virtual ICollection<Teacher> Teachers { get; set; } = new List<Teacher>();
}
