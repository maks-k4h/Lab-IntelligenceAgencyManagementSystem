using System;
using System.Collections.Generic;

namespace IntelligenceAgencyManagementSystem;

public partial class Operation
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public int DepartmentId { get; set; }

    public DateOnly? DateStarted { get; set; }

    public DateOnly? DateEnded { get; set; }

    public virtual ICollection<AgentsToOp> AgentsToOps { get; } = new List<AgentsToOp>();

    public virtual Department? Department { get; set; } = null;

    public virtual ICollection<Task> Tasks { get; } = new List<Task>();
}
