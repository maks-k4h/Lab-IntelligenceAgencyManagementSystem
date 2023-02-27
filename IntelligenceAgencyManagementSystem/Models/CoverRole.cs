using System;
using System.Collections.Generic;

namespace IntelligenceAgencyManagementSystem;

public partial class CoverRole
{
    public int Id { get; set; }

    public string FirstName { get; set; } = null!;

    public string SecondName { get; set; } = null!;

    public int? GenderId { get; set; }

    public DateTime? BirthDate { get; set; }

    public DateTime? DeathDate { get; set; }

    public DateTime? DateActivated { get; set; }

    public DateTime? DateDeactivated { get; set; }

    public string? Legend { get; set; }

    public string? ActivitySummary { get; set; }

    public virtual ICollection<AgentsToOp> AgentsToOps { get; } = new List<AgentsToOp>();

    public virtual Gender? Gender { get; set; }
}
