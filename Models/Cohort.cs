using System;
using System.Collections.Generic;

namespace SharpSeer.Models;

public partial class Cohort
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Major { get; set; } = null!;

    public int Term { get; set; }

    public virtual ICollection<Exam> Exams { get; set; } = new List<Exam>();
}
