using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IntelligenceAgencyManagementSystem;

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

    public virtual ICollection<AgentsToOp> AgentsToOps { get; } = new List<AgentsToOp>();

    public virtual Department? Department { get; set; } = null;

    public virtual ICollection<Task> Tasks { get; } = new List<Task>();
}
