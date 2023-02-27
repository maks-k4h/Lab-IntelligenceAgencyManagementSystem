using System;
using System.Collections.Generic;

namespace IntelligenceAgencyManagementSystem;

public partial class TaskStatus
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<Task> Tasks { get; } = new List<Task>();
}
