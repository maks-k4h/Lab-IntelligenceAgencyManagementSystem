using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IntelligenceAgencyManagementSystem;

[Display(Name = "Персонаж")]
public partial class CoverRole
{
    [Display(Name = "Ідентифікатор")]
    public int Id { get; set; }

    [Display(Name = "Імʼя")]
    [Required(ErrorMessage = "Необхідно вказати імʼя")]
    public string FirstName { get; set; } = null!;

    [Display(Name = "Прізвище")]
    [Required(ErrorMessage = "Необхідно вказати прізвище")]
    public string SecondName { get; set; } = null!;

    public string FullName => FirstName + " " + SecondName;

    [Display(Name = "Ідентифікатор гендеру")]
    public int? GenderId { get; set; }

    [Display(Name = "Дата народження")]
    [RegularExpression(Constants.DmyDatePattern, ErrorMessage = "Введіть коректну дату")]
    public DateOnly? BirthDate { get; set; }

    [Display(Name = "Дата смерті")]
    [RegularExpression(Constants.DmyDatePattern, ErrorMessage = "Введіть коректну дату")]
    public DateOnly? DeathDate { get; set; }

    [Display(Name = "Дата активації")]
    [RegularExpression(Constants.DmyDatePattern, ErrorMessage = "Введіть коректну дату")]
    public DateOnly? DateActivated { get; set; }

    [Display(Name = "Дата деактивіції")]
    [RegularExpression(Constants.DmyDatePattern, ErrorMessage = "Введіть коректну дату")]
    public DateOnly? DateDeactivated { get; set; }

    [Display(Name = "Легенда")]
    public string? Legend { get; set; }

    [Display(Name = "Активність")]
    public string? ActivitySummary { get; set; }

    public virtual ICollection<WorkersToOp> WorkersToOps { get; } = new List<WorkersToOp>();

    [Display(Name = "Гендер")]
    public virtual Gender? Gender { get; set; }
}
