using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IntelligenceAgencyManagementSystem;

public partial class Worker
{
    [Display(Name = "Ідентифікатор")]
    public int Id { get; set; }

    [Display(Name = "Імʼя")]
    [Required(ErrorMessage = "Необхідно вказати імʼя")]
    public string FirstName { get; set; } = null!;

    [Display(Name = "Прізвище")]
    public string? SecondName { get; set; }

    [NotMapped]
    public string? FullName => FirstName + " " + SecondName;

    [Display(Name = "Ідентифікатор гендеру")]
    public int? GenderId { get; set; }

    [Display(Name = "Дата народження")]
    public DateOnly? BirthDate { get; set; }

    [Display(Name = "Дата смерті")]
    public DateOnly? DeathDate { get; set; }

    [Display(Name = "Сімейний статус")]
    public string? FamilyStatus { get; set; }

    [Display(Name = "Освіта")]
    public string? Education { get; set; }

    [Display(Name = "Досвід")]
    public string? Experience { get; set; }

    [Display(Name = "Стан здоровʼя")]
    public string? HealthInformation { get; set; }

    [Display(Name = "Юридична інформація")]
    public string? LegalInformation { get; set; }

    public virtual ICollection<WorkersToOp> AgentsToOps { get; } = new List<WorkersToOp>();

    [Display(Name = "Гендер")]
    public virtual Gender? Gender { get; set; }

    public virtual ICollection<MilitaryFile> MilitaryFiles { get; } = new List<MilitaryFile>();

    public virtual ICollection<TasksToWorkers> TasksToWorker { get; } = new List<TasksToWorkers>();

    public virtual ICollection<WorkingInDepartment> WorkingInDepartments { get; } = new List<WorkingInDepartment>();
}
