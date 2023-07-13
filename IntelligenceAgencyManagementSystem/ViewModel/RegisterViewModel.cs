using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace IntelligenceAgencyManagementSystem.ViewModel;

public class RegisterViewModel
{
    [Display(Name = "Email")]
    [DataType(DataType.EmailAddress, ErrorMessage = "Невірний формат")]
    [Required(ErrorMessage = "Вкажіть email")]
    public string Email { get; set; }

    [Display(Name = "Рік народження")]
    [Required(ErrorMessage = "Необхідно вказати рік народження")]
    [RegularExpression(Constants.YearPattern, ErrorMessage = "Введіть коректну дату")]
    public int Year { get; set; }
    
    [Display(Name = "Пароль")]
    [Required(ErrorMessage = "Вкажіть пароль")]
    public string Password { get; set; }
    
    [Display(Name = "Підтвердження паролю")]
    [DataType(DataType.Password)]
    [Required(ErrorMessage = "Пароль необхідно підтвердити")]
    [Compare("Password", ErrorMessage = "Паролі не співпадають")]
    public string PasswordConfirm { get; set; }
}