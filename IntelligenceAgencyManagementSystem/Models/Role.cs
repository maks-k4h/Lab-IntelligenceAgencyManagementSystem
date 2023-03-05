using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IntelligenceAgencyManagementSystem;

[Display(Name = "Посада")]
public partial class Role
{
    [Display(Name = "Ідентифікатор")]
    public int Id { get; set; }

    [Display(Name = "Назва")]
    [Required(ErrorMessage = "Необхідно вказати назву посади")]
    public string Title { get; set; } = null!;

    public virtual ICollection<WorkingInDepartment> WorkingInDepartments { get; } = new List<WorkingInDepartment>();
}
