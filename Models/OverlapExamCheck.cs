using System;
using System.Collections.Generic;

namespace SharpSeer.Models;

public partial class OverlapExamCheck
{
    public int Id { get; set; }

    public int ExamId { get; set; }

    public int? TeacherId { get; set; }

    public int? CohortId { get; set; }

    public virtual Cohort? Cohort { get; set; }

    public virtual Exam Exam { get; set; } = null!;

    public virtual Teacher? Teacher { get; set; }
}
