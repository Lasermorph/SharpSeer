using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SharpSeer.Models;

[Index("Name", Name = "UQ__Cohorts__737584F66B6116DB", IsUnique = true)]
public partial class Cohort
{
    [Key]
    [Column("ID")]
    public int Id { get; set; } = 1;

    [StringLength(23)]
    [Unicode(false)]
    public string Name { get; set; } = null!;

    [StringLength(31)]
    [Unicode(false)]
    public string Major { get; set; } = null!;

    public int Term { get; set; }

    [InverseProperty("Cohort")]
    public virtual Exam? Exam { get; set; }

    public Cohort()
    {
    }
    public Cohort(string name, string major, int term)
    {
        Name = name;
        Major = major;
        Term = term;
    }
}
