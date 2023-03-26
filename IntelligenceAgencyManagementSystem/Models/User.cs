namespace IntelligenceAgencyManagementSystem.Models;
using Microsoft.AspNetCore.Identity;

public class User : IdentityUser
{
    public int Year { get; set; }
}