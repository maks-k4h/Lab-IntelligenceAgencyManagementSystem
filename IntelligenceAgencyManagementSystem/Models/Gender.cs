﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IntelligenceAgencyManagementSystem;

[Display(Name="Гендер")]
public partial class Gender
{
    [Display(Name = "Ідентифікатор")]
    public int Id { get; set; }

    [Display(Name = "Назва")]
    public string Name { get; set; } = null!;

    public virtual ICollection<CoverRole> CoverRoles { get; } = new List<CoverRole>();

    public virtual ICollection<Worker> Workers { get; } = new List<Worker>();
}
