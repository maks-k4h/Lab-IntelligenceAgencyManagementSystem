using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using IntelligenceAgencyManagementSystem;

namespace IntelligenceAgencyManagementSystem.Models
{
    public class OperationsManagementController : Controller
    {
        private readonly IaDbContext _context;

        public OperationsManagementController(IaDbContext context)
        {
            _context = context;
        }

        // GET: OperationsManagement
        public async Task<IActionResult> Index(int? id)
        {
            if (id == null)
            {
                var iaDbContext = _context.Operations.Include(o => o.Department);
                return View(await iaDbContext.ToListAsync());
            }
            
            var operation = await _context.Operations
                .Include(o => o.Department)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (operation == null)
            {
                return NotFound();
            }

            return View("Details", operation);
            
        }
    }
}
