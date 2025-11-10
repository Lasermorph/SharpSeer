using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SharpSeer.Models;

public partial class Cohort
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string Name { get; set; } = null!;

    [StringLength(100)]
    [Unicode(false)]
    public string Major { get; set; } = null!;

    public int Term { get; set; }

    [ForeignKey("CohortId")]
    [InverseProperty("Cohorts")]
    public virtual ICollection<Exam> Exams { get; set; } = new List<Exam>();

    public Cohort()
    {
    }
}
