using System;
using System.Collections.Generic;

namespace IntelligenceAgencyManagementSystem;

public partial class PersonFile
{
    public int Id { get; set; }

    public string? FirstName { get; set; }

    public string? SecondName { get; set; }

    public int? GenderId { get; set; }

    public DateTime? BirthDate { get; set; }

    public DateTime? DeathDate { get; set; }

    public string? FamilyStatus { get; set; }

    public string? Education { get; set; }

    public string? Experience { get; set; }

    public int? MilitaryInformationId { get; set; }

    public string? HealthInformation { get; set; }

    public string? LegalInformation { get; set; }

    public virtual ICollection<AgentsToOp> AgentsToOps { get; } = new List<AgentsToOp>();

    public virtual Gender? Gender { get; set; }

    public virtual MilitaryInformation? MilitaryInformation { get; set; }

    public virtual ICollection<TasksToPersonFile> TasksToPersonFiles { get; } = new List<TasksToPersonFile>();

    public virtual ICollection<WorkingInDepartment> WorkingInDepartments { get; } = new List<WorkingInDepartment>();
}
