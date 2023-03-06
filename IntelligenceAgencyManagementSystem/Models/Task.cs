using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IntelligenceAgencyManagementSystem;

[Display(Name = "Завдання")]
public partial class Task
{
    [Display(Name = "Ідентифікатор")]
    public int Id { get; set; }

    [Display(Name = "Ідентифікатр операції")]
    [Required(ErrorMessage = "Необхідно вказати операцію")]
    public int OperationId { get; set; }

    [Display(Name = "Назва")]
    [Required(ErrorMessage = "Необхідно вказати назву")]
    public string Title { get; set; } = null!;

    [Display(Name = "Опис")]
    public string? Description { get; set; }

    [Display(Name = "Ідентифікатор статусу")]
    [Required(ErrorMessage = "Необхідно вказати статус")]
    public int StatusId { get; set; }

    [Display(Name = "Остання зміна")]
    public DateTime? DateStatusSet { get; set; }

    [Display(Name = "Операція")]
    public virtual Operation? Operation { get; set; } = null!;

    [Display(Name = "Статус")]
    public virtual TaskStatus? Status { get; set; } = null!;

    public virtual ICollection<TasksToWorkers> TasksToWorkers { get; } = new List<TasksToWorkers>();
}
