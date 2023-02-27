using System;
using System.Collections.Generic;

namespace IntelligenceAgencyManagementSystem;

public partial class TasksToPersonFile
{
    public int Id { get; set; }

    public int TaskId { get; set; }

    public int PersonFileId { get; set; }

    public virtual PersonFile PersonFile { get; set; } = null!;

    public virtual Task Task { get; set; } = null!;
}
