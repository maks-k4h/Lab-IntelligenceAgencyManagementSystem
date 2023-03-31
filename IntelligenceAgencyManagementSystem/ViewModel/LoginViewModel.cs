using System.ComponentModel.DataAnnotations;

namespace IntelligenceAgencyManagementSystem.ViewModel;

public class LoginViewModel
{
    [Microsoft.Build.Framework.Required]
    [Display(Name = "Email")]
    public string Email { get; set; }
    
    [Required]
    [DataType(DataType.Password)]
    [Display(Name = "Пароль")]
    public string Password { get; set; }
    
    [Display(Name = "Запамʼятати?")]
    public bool RememberMe { get; set; }
    
    public string? ReturnUrl { get; set; }
}