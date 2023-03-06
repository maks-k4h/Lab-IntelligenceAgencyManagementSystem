using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IntelligenceAgencyManagementSystem;

[Display(Name = "Співробітник")]
public partial class Worker
{
    [Display(Name = "Ідентифікатор")]
    public int Id { get; set; }

    [Display(Name = "Імʼя")]
    [Required(ErrorMessage = "Необхідно вказати імʼя")]
    public string FirstName { get; set; } = null!;

    [Display(Name = "Прізвище")]
    [Required(ErrorMessage = "Необхідно вказати прізвище")]
    public string SecondName { get; set; } = null!;

    [NotMapped]
    [Display(Name="Повне імʼя")]
    public string? FullName => FirstName + " " + SecondName;

    [Display(Name = "Ідентифікатор гендеру")]
    public int? GenderId { get; set; }

    [Display(Name = "Дата народження")]
    [Required(ErrorMessage = "Необхідно вказати дату народження")]
    public DateOnly BirthDate { get; set; }

    [Display(Name = "Дата смерті")]
    public DateOnly? DeathDate { get; set; }

    [Display(Name = "Сімейний статус")]
    public string? FamilyStatus { get; set; }

    [Display(Name = "Освіта")]
    public string? Education { get; set; }

    [Display(Name = "Попередній досвід")]
    public string? Experience { get; set; }

    [Display(Name = "Стан здоровʼя")]
    public string? HealthInformation { get; set; }

    [Display(Name = "Юридична інформація")]
    public string? LegalInformation { get; set; }

    public virtual ICollection<WorkersToOp> WorkersToOps { get; } = new List<WorkersToOp>();

    [Display(Name = "Гендер")]
    public virtual Gender? Gender { get; set; }

    [Display(Name="Військова інформація")]
    public virtual ICollection<MilitaryFile> MilitaryFiles { get; } = new List<MilitaryFile>();

    public virtual ICollection<TasksToWorkers> TasksToWorker { get; } = new List<TasksToWorkers>();

    [Display(Name="Трудова діяльність")]
    public virtual ICollection<WorkingInDepartment> WorkingInDepartments { get; } = new List<WorkingInDepartment>();
}
