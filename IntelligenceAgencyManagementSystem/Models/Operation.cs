using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IntelligenceAgencyManagementSystem;

public partial class Operation
{
    [Display(Name = "Ключ")]
    public int Id { get; set; }
    
    [Display(Name = "Назва операції")]
    [Required(ErrorMessage = "Необхідно вказати назву операції")]
    public string Name { get; set; } = null!;
    
    [Display(Name = "Опис")]
    public string? Description { get; set; }

    [Display(Name = "Ключ Департаменту")]
    [Required(ErrorMessage = "Необхідно вказати департамент")]
    public int DepartmentId { get; set; }

    [Display(Name = "Дата початку")]
    public DateTime? DateStarted { get; set; }

    [Display(Name = "Дата завершення")]
    public DateTime? DateEnded { get; set; }

    public virtual ICollection<AgentsToOp> AgentsToOps { get; } = new List<AgentsToOp>();

    public virtual Department? Department { get; set; } = null;

    public virtual ICollection<Task> Tasks { get; } = new List<Task>();
}
