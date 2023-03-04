using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IntelligenceAgencyManagementSystem;

public partial class MilitaryFile
{
    [Display(Name = "Ідентифікатор")]
    public int Id { get; set; }

    [Display(Name = "Звання")]
    public string? MilitaryRank { get; set; }

    [Display(Name = "Детальна інформація")]
    [Required(ErrorMessage = "Необхідно заповнити дане поле")]
    public string FullInformation { get; set; } = null!;

    public int WorkerId { get; set; }

    public virtual Worker? Worker { get; set; }
}
