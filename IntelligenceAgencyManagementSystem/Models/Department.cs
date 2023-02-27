using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Google.Protobuf.WellKnownTypes;

namespace IntelligenceAgencyManagementSystem;

public partial class Department
{
    [Display(Name = "Ключ")]
    public int Id { get; set; }

    [Display(Name = "Назва департаменту")]
    [Required(ErrorMessage = "Необхідно вказати назву департаменту")]
    public string? Name { get; set; } = null!;

    [Display(Name = "Опис")]
    public string? Description { get; set; }

    [Display(Name = "Дата створення")]
    [Required(ErrorMessage = "Необхідно вказати дату створення департаменту")]
    public DateTime DateCreated { get; set; }

    [Display(Name = "Дата закриття")]
    public DateTime? DateClosed { get; set; }

    public virtual ICollection<Operation> Operations { get; } = new List<Operation>();

    public virtual ICollection<WorkingInDepartment> WorkingInDepartments { get; } = new List<WorkingInDepartment>();
}
