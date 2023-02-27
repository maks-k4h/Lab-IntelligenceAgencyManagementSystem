using System;
using System.Collections.Generic;

namespace IntelligenceAgencyManagementSystem;

public partial class WorkingInDepartment
{
    public int Id { get; set; }

    public int PersonFileId { get; set; }

    public int DepartmentId { get; set; }

    public int RoleId { get; set; }

    public string? Description { get; set; }

    public DateTime DateStarted { get; set; }

    public DateTime? DateEnded { get; set; }

    public virtual Department Department { get; set; } = null!;

    public virtual PersonFile PersonFile { get; set; } = null!;

    public virtual Role Role { get; set; } = null!;
}
