using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IntelligenceAgencyManagementSystem;

[Display(Name = "Статус завдання")]
public partial class TaskStatus
{
    [Display(Name = "Ідентифікатор")]
    public int Id { get; set; }

    [Display(Name = "Статус")]
    [Required(ErrorMessage = "Необхідно вказати назву статусу")]
    public string Title { get; set; } = null!;

    [Display(Name = "Подробиці статусу")]
    public string? Description { get; set; }

    public virtual ICollection<Task> Tasks { get; } = new List<Task>();
}
