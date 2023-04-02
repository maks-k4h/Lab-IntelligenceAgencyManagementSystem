using System.Text.RegularExpressions;
using IntelligenceAgencyManagementSystem.Models;
using IntelligenceAgencyManagementSystem.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IntelligenceAgencyManagementSystem.Controllers;

[Authorize(Roles="admin")]
public class UserRolesController : Controller
{
    private RoleManager<IdentityRole> _roleManager;
    private UserManager<User> _userManager;
    private IdentityContext _context;

    public UserRolesController(RoleManager<IdentityRole> roleManager, UserManager<User> userManager, IdentityContext context)
    {
        _roleManager = roleManager;
        _userManager = userManager;
        _context = context;
    }
    
    
    public async Task<IActionResult> Index()
    {
        var roles = _roleManager.Roles.ToListAsync();

        var res = new List<(bool, IdentityRole)>();
        foreach (IdentityRole role in await roles)
        {
            bool used = _context.UserRoles.Count(r => r.RoleId == role.Id) > 0;
            res.Add((used, role));
        }
        
        return View(res);
    }

    public async Task<IActionResult> UserList()
    {
        return View(await _userManager.Users.ToListAsync());   
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(string roleName)
    {
        roleName = roleName.Normalize().ToLower();

        if (Regex.IsMatch(roleName, "[^a-z]"))
        {
            ViewBag.ErrorMessage = "Використовуйте тільки латиніські літери";
            ViewBag.RoleName = roleName;
            return View("Create");
        }
        
        if (await _roleManager.FindByNameAsync(roleName) == null)
        {
            await _roleManager.CreateAsync(new IdentityRole(roleName));
        }
        else
        {
            ViewBag.ErrorMessage = "Така роль уже існує";
            ViewBag.RoleName = roleName;
            return View("Create");
        }

        return RedirectToAction("Index");
    }

    [HttpGet]
    public async Task<IActionResult> Edit(string? userId)
    {
        User user = await _userManager.FindByIdAsync(userId);

        if (user != null)
        {
            var userRoles = await _userManager.GetRolesAsync(user);
            var allRoles = await _roleManager.Roles.ToListAsync();

            ChangeRoleViewModel model = new ChangeRoleViewModel
            {
                UserId = user.Id,
                UserEmail = user.Email,
                UserRoles = userRoles,
                AllRoles = allRoles
            };
            return View(model);
        }

        return NotFound();
    }

    [HttpPost]
    public async Task<IActionResult> Edit(string userId, List<string> roles)
    {
        User user = await _userManager.FindByIdAsync(userId);
        if (user != null)
        {
            var userRoles = await _userManager.GetRolesAsync(user);
            var allRoles = await _roleManager.Roles.ToListAsync();
            var addedRoles = roles.Except(userRoles);
            var removedRoles = userRoles.Except(roles);
            
            await _userManager.AddToRolesAsync(user, addedRoles);
            await _userManager.RemoveFromRolesAsync(user, removedRoles);

            return RedirectToAction("UserList");
        }

        return NotFound();
    }

    public async Task<IActionResult> Delete(string roleName)
    {
        var role = await _roleManager.FindByNameAsync(roleName);
        if (role != null)
        {
            await _roleManager.DeleteAsync(role);
        }
        
        return RedirectToAction("Index");
    }
}