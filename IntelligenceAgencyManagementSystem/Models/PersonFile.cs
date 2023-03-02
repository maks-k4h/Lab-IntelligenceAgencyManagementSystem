using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IntelligenceAgencyManagementSystem;

public partial class PersonFile
{
    [Display(Name = "Ідентифікатор")]
    public int Id { get; set; }

    [Display(Name = "Імʼя")]
    [Required(ErrorMessage = "Необхідно вказати імʼя")]
    public string FirstName { get; set; } = null!;

    [Display(Name = "Прізвище")]
    public string? SecondName { get; set; }

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

    public virtual ICollection<AgentsToOp> AgentsToOps { get; } = new List<AgentsToOp>();

    [Display(Name = "Гендер")]
    public virtual Gender? Gender { get; set; }

    public virtual ICollection<MilitaryInformation> MilitaryInformations { get; } = new List<MilitaryInformation>();

    public virtual ICollection<TasksToPersonFile> TasksToPersonFiles { get; } = new List<TasksToPersonFile>();

    public virtual ICollection<WorkingInDepartment> WorkingInDepartments { get; } = new List<WorkingInDepartment>();
}
