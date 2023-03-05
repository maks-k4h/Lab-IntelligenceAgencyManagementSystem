using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IntelligenceAgencyManagementSystem;

[Display(Name="Операція")]
public partial class Operation
{
    [Display(Name = "Ідентифікатор")]
    public int Id { get; set; }

    [Display(Name = "Назва")]
    [Required(ErrorMessage = "Операція повинна мати назву")]
    public string Name { get; set; } = null!;

    [Display(Name = "Опис")]
    public string? Description { get; set; }

    [Display(Name = "Ідентифікатор департаменту")]
    public int DepartmentId { get; set; }

    [Display(Name = "Дата початку")]
    public DateOnly? DateStarted { get; set; }

    [Display(Name = "Дата закінчення")]
    public DateOnly? DateEnded { get; set; }

    public virtual ICollection<WorkersToOp> WorkersToOps { get; } = new List<WorkersToOp>();

    [Display(Name="Департамент")]
    public virtual Department? Department { get; set; } = null;

    [Display(Name="Завдання")]
    public virtual ICollection<Task> Tasks { get; } = new List<Task>();
}
