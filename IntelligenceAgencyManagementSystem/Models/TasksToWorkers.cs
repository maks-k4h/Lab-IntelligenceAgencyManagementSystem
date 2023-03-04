using System;
using System.Collections.Generic;

namespace IntelligenceAgencyManagementSystem;

public partial class TasksToWorkers
{
    public int Id { get; set; }

    public int TaskId { get; set; }

    public int WorkerId { get; set; }

    public virtual Worker Worker { get; set; } = null!;

    public virtual Task Task { get; set; } = null!;
}
