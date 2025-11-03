using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SharpSeer.Models;

[Index("Email", Name = "UQ__Teachers__A9D10534E7FBAD77", IsUnique = true)]
public partial class Teacher
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string Name { get; set; } = null!;

    [StringLength(255)]
    [Unicode(false)]
    public string Email { get; set; } = null!;

    [StringLength(15)]
    [Unicode(false)]
    public string? PhoneNumber { get; set; }

    [ForeignKey("TeacherId")]
    [InverseProperty("Teachers")]
    public virtual ICollection<Exam> Exams { get; set; } = new List<Exam>();
}
