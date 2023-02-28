﻿using System;
using System.Collections.Generic;

namespace IntelligenceAgencyManagementSystem;

public partial class Department
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public DateOnly DateCreated { get; set; }

    public DateOnly? DateClosed { get; set; }

    public virtual ICollection<Operation> Operations { get; } = new List<Operation>();

    public virtual ICollection<WorkingInDepartment> WorkingInDepartments { get; } = new List<WorkingInDepartment>();
}
