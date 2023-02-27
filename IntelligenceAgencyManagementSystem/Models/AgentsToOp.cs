using System;
using System.Collections.Generic;

namespace IntelligenceAgencyManagementSystem;

public partial class AgentsToOp
{
    public int Id { get; set; }

    public int CoverRoleId { get; set; }

    public int OperationId { get; set; }

    public int PersonFileId { get; set; }

    public virtual CoverRole CoverRole { get; set; } = null!;

    public virtual Operation Operation { get; set; } = null!;

    public virtual PersonFile PersonFile { get; set; } = null!;
}
