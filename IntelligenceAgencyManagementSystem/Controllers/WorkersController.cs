using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.JavaScript;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using IntelligenceAgencyManagementSystem;
using Microsoft.AspNetCore.Authorization;

namespace IntelligenceAgencyManagementSystem.Views
{
    [Authorize(Roles="admin, chairman, hr")]
    public class WorkersController : Controller
    {
        private readonly IaDbContext _context;

        public WorkersController(IaDbContext context)
        {
            _context = context;
        }

        // GET: Workers
        public async Task<IActionResult> Index(int? department)
        {
            var workers = _context.Workers.Where(a => true);
            
            if (department != null)
                workers = workers.Where(file => _context.WorkingInDepartments.Count(wid => wid.DepartmentId == department && wid.WorkerId == file.Id) > 0);

            ViewBag.Departments = new SelectList(_context.Departments, "Id", "Name");
            
            return View(await workers.Include(p => p.Gender).ToListAsync());
        }

        // GET: Workers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Workers == null)
            {
                return NotFound();
            }

            var worker = await _context.Workers
                .Include(p => p.Gender)
                .Include(p => p.MilitaryFiles)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (worker == null)
            {
                return NotFound();
            }

            var militaryFileQuery = _context.MilitaryFiles
                .Where(file => file.WorkerId == id);
            if (militaryFileQuery.Any())
                ViewBag.MilitaryFile = militaryFileQuery.First();
            else
                ViewBag.MilitaryFile = null!;

            ViewBag.WorkingInDepartment = await _context.WorkingInDepartments
                .Where(wid => wid.WorkerId == id)
                .Include(wid => wid.Role)
                .Include(wid => wid.Department)
                .ToListAsync();

            return View(worker);
        }

        // GET: Workers/Create
        public IActionResult Create()
        {
            ViewData["GenderId"] = new SelectList(_context.Genders, "Id", "Name");
            ViewData["MilitaryFileId"] = new SelectList(_context.MilitaryFiles, "Id", "Id");
            return View();
        }

        // POST: Workers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,SecondName,GenderId,BirthDate,DeathDate,FamilyStatus,Education,Experience,HealthInformation,LegalInformation")] Worker worker)
        {
            try
            {
                if (worker.BirthDate != null && worker.DeathDate != null && worker.DeathDate < worker.BirthDate)
                    throw new Exception("Введіть вірні дати");
                
                if (ModelState.IsValid)
                {
                    _context.Add(worker);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Edit", new
                    {
                        id = worker.Id, 
                        type="Details"
                    });
                }
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = e.Message;
            }
            ViewData["GenderId"] = new SelectList(_context.Genders, "Id", "Name", worker.GenderId);
            return View();
        }

        // GET: Workers/Edit/5/General
        public async Task<IActionResult> Edit(int? id, string? type)
        {
            if (id == null || _context.Workers == null)
            {
                return NotFound();
            }
            
            var worker = await _context.Workers.FindAsync(id);
            if (worker == null)
            {
                return NotFound();
            }

            if (type == null || type.Trim().ToLower() == "general")
            {
                // edit general information
                
                ViewData["GenderId"] = new SelectList(_context.Genders, "Id", "Name", worker.GenderId);
                return View("EditGeneral",worker);
            }
            
            if (type.Trim().ToLower() == "details")
            {
                // edit details
                
                return View("EditDetails",worker);
            }
            if (type.Trim().ToLower() == "military")
            {
                // edit military file

                return RedirectToAction("Create", "MilitaryFiles", worker.Id);
            }
            
            // unknown request
            return NotFound();
        }

        // POST: Workers/Edit/5/General
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, string? type, [Bind("Id,FirstName,SecondName,GenderId,BirthDate,DeathDate,FamilyStatus,Education,Experience,MilitaryFileId,HealthInformation,LegalInformation")] Worker worker)
        {
            if (id != worker.Id || type == null)
            {
                return NotFound();
            }

            type = type.ToLower().Trim();

            if (ModelState.IsValid && 
                (type != "general" || type != "details"))
            {
                try
                {
                    if (worker.BirthDate != null && worker.DeathDate != null && worker.DeathDate < worker.BirthDate)
                        throw new Exception("Введіть вірні дати");
                    
                    _context.Update(worker);
                    await _context.SaveChangesAsync();
                    
                    if (type.Trim().ToLower() == "general")
                        return RedirectToAction("Edit", new
                        {
                            id = id, 
                            type = "details"
                        });
                
                    if (type.Trim().ToLower() == "details")
                        return RedirectToAction("Create", "MilitaryFiles", new
                        {
                            id = id
                        });
                    
                }
                catch (Exception e)
                {
                    if (!WorkerExists(worker.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        ViewBag.ErrorMessage = e.Message;
                    }
                }
            }
            ViewData["GenderId"] = new SelectList(_context.Genders, "Id", "Name", worker.GenderId);
            return View("EditGeneral", worker);
        }

        // GET: Workers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Workers == null || _context.MilitaryFiles == null)
            {
                return NotFound();
            }

            var worker = await _context.Workers
                .Include(p => p.Gender)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (worker == null)
            {
                return NotFound();
            }
            
            var militaryFilesQuery = _context.MilitaryFiles
                .Where(file => file.WorkerId == id);
            if (militaryFilesQuery.Any())
                ViewBag.MilitaryFile = militaryFilesQuery.First();
            else
                ViewBag.MilitaryFile = null!;
            
            return View(worker);
        }

        // POST: Workers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Workers == null)
            {
                return Problem("Entity set 'IaDbContext.Workers  is null.");
            }
            var worker = await _context.Workers.FindAsync(id);
            if (worker != null)
            {
                // remove dependencies — military files
                var militaryFilesRequest = _context.MilitaryFiles
                    .Where(file => file.WorkerId == id);
                if (militaryFilesRequest.Any())
                    foreach (var militaryFile in militaryFilesRequest)
                    {
                        _context.MilitaryFiles.Remove(militaryFile);
                    }

                _context.Workers.Remove(worker);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WorkerExists(int id)
        {
          return (_context.Workers?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
