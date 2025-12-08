using System;
using System.Collections.Generic;

namespace SharpSeer.Models;

public partial class Teacher
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? PhoneNumber { get; set; }

    public string NameId { get; set; } = null!;

    public bool? IsExternal { get; set; }

    public virtual ICollection<OverlapExamCheck> OverlapExamChecks { get; set; } = new List<OverlapExamCheck>();

    public virtual ICollection<Exam> Exams { get; set; } = new List<Exam>();

    public virtual ICollection<OverlapExamCheck> OverlapExamChecks { get; set; } = new List<OverlapExamCheck>();
}
