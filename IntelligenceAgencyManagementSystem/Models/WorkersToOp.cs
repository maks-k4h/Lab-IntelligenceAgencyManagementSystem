using System;
using System.Collections.Generic;

namespace IntelligenceAgencyManagementSystem;

public partial class WorkersToOp
{
    public int Id { get; set; }

    public int CoverRoleId { get; set; }

    public int OperationId { get; set; }

    public int WorkerId { get; set; }

    public virtual CoverRole? CoverRole { get; set; } = null!;

    public virtual Operation? Operation { get; set; } = null!;

    public virtual Worker? Worker { get; set; } = null!;
}
