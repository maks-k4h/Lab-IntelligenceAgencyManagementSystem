using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IntelligenceAgencyManagementSystem;

public partial class Gender
{
    [Display(Name = "Ідентифікатор")]
    public int Id { get; set; }

    [Display(Name = "Гендер")]
    public string Name { get; set; } = null!;

    public virtual ICollection<CoverRole> CoverRoles { get; } = new List<CoverRole>();

    public virtual ICollection<PersonFile> PersonFiles { get; } = new List<PersonFile>();
}
