﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IntelligenceAgencyManagementSystem;

public partial class Task
{
    public int Id { get; set; }

    public int OperationId { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public int StatusId { get; set; }

    public DateTime? DateStatusSet { get; set; }

    public virtual Operation? Operation { get; set; } = null!;

    public virtual TaskStatus? Status { get; set; } = null!;

    public virtual ICollection<TasksToWorkers> TasksToWorkers { get; } = new List<TasksToWorkers>();
}
