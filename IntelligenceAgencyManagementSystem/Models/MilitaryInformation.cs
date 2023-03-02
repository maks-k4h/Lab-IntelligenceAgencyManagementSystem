using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IntelligenceAgencyManagementSystem;

public partial class MilitaryInformation
{
    [Display(Name = "Ідентифікатор")]
    public int Id { get; set; }

    [Display(Name = "Звання")]
    public string? MilitaryRank { get; set; }

    [Display(Name = "Детальна інформація")]
    [Required(ErrorMessage = "Необхідно заповнити дане поле")]
    public string FullInformation { get; set; } = null!;

    public int PersonFileId { get; set; }

    public virtual PersonFile? PersonFile { get; set; }
}
