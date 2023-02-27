using System;
using System.Collections.Generic;

namespace IntelligenceAgencyManagementSystem;

public partial class Gender
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<CoverRole> CoverRoles { get; } = new List<CoverRole>();

    public virtual ICollection<PersonFile> PersonFiles { get; } = new List<PersonFile>();
}
