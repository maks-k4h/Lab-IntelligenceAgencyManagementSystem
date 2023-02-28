using System;
using System.Collections.Generic;

namespace IntelligenceAgencyManagementSystem;

public partial class MilitaryInformation
{
    public int Id { get; set; }

    public string? MilitaryRank { get; set; }

    public string FullInformation { get; set; } = null!;

    public int PersonFileId { get; set; }

    public virtual PersonFile? PersonFile { get; set; }
}
