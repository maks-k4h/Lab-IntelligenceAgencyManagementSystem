using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IntelligenceAgencyManagementSystem;

[Display(Name = "Трудова діяльність")]
public partial class WorkingInDepartment
{
    [Display(Name = "Ідентифікатор")]
    public int Id { get; set; }

    [Display(Name = "Ідентифікатор співробітника")]
    [Required(ErrorMessage = "Необхідно вказати співробітника")]
    public int WorkerId { get; set; }

    [Display(Name = "Ідентифікатор департаменту")]
    [Required(ErrorMessage = "Необхідно вказати департамент")]
    public int DepartmentId { get; set; }

    [Display(Name = "Ідентифікатор посади")]
    [Required(ErrorMessage = "Необхідно вказати посаду")]
    public int RoleId { get; set; }

    [Display(Name = "Опис")]
    public string? Description { get; set; }

    [Display(Name = "Дата початку")]
    [Required(ErrorMessage = "Необхідно вказати дату початку")]
    [RegularExpression(Constants.DatePattern, ErrorMessage = "Введіть коректну дату")]
    public DateOnly DateStarted { get; set; }

    [Display(Name = "Дата закінчення")]
    [RegularExpression(Constants.DatePattern, ErrorMessage = "Введіть коректну дату")]
    public DateOnly? DateEnded { get; set; }

    [Display(Name = "Департамент")]
    public virtual Department? Department { get; set; } = null!;

    [Display(Name = "Працівник")]
    public virtual Worker? Worker { get; set; } = null!;

    [Display(Name = "Посада")]
    public virtual Role? Role { get; set; } = null!;
}
