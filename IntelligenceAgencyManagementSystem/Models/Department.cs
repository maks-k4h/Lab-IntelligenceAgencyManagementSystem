using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IntelligenceAgencyManagementSystem;

[Display(Name = "Департамент")]
public partial class Department
{
    [Display(Name="Ідентифікатор")]
    public int Id { get; set; }

    [Display(Name="Назва")]
    [Required(ErrorMessage = "Необхідно вказати назву департаменту")]
    public string Name { get; set; } = null!;

    [Display(Name="Опис")]
    public string? Description { get; set; }

    [Display(Name="Дата створення")]
    [Required(ErrorMessage = "Необхідно вказати дату створення департаменту")]
    public DateOnly DateCreated { get; set; }

    [Display(Name="Дата закриття")]
    public DateOnly? DateClosed { get; set; }

    [Display(Name="Операції")]
    public virtual ICollection<Operation> Operations { get; } = new List<Operation>();

    public virtual ICollection<WorkingInDepartment> WorkingInDepartments { get; } = new List<WorkingInDepartment>();
}
