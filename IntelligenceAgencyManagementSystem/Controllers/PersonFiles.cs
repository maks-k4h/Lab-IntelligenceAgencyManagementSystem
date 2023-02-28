using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using IntelligenceAgencyManagementSystem;

namespace IntelligenceAgencyManagementSystem.Views
{
    public class PersonFilesController : Controller
    {
        private readonly IaDbContext _context;

        public PersonFilesController(IaDbContext context)
        {
            _context = context;
        }

        // GET: PersonFiles
        public async Task<IActionResult> Index()
        {
            var iaDbContext = _context.PersonFiles.Include(p => p.Gender);

            return View(await iaDbContext.ToListAsync());
        }

        // GET: PersonFiles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.PersonFiles == null)
            {
                return NotFound();
            }

            var personFile = await _context.PersonFiles
                .Include(p => p.Gender)
                .Include(p => p.MilitaryInformations)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (personFile == null)
            {
                return NotFound();
            }

            var militaryInformationQuery = _context.MilitaryInformations
                .Where(information => information.PersonFileId == id);
            if (militaryInformationQuery.Any())
                ViewBag.MilitaryInformation = militaryInformationQuery.First();
            else
                ViewBag.MilitaryInformation = null!;

            return View(personFile);
        }

        // GET: PersonFiles/Create
        public IActionResult Create()
        {
            ViewData["GenderId"] = new SelectList(_context.Genders, "Id", "Name");
            ViewData["MilitaryInformationId"] = new SelectList(_context.MilitaryInformations, "Id", "Id");
            return View();
        }

        // POST: PersonFiles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,SecondName,GenderId,BirthDate,DeathDate,FamilyStatus,Education,Experience,HealthInformation,LegalInformation")] PersonFile personFile)
        {
            if (ModelState.IsValid)
            {
                _context.Add(personFile);
                await _context.SaveChangesAsync();
                return RedirectToAction("Edit", new
                {
                    id = personFile.Id, 
                    type="Details"
                });
            }
            ViewData["GenderId"] = new SelectList(_context.Genders, "Id", "Id", personFile.GenderId);
            return View();
        }

        // GET: PersonFiles/Edit/5/General
        public async Task<IActionResult> Edit(int? id, string? type)
        {
            if (id == null || _context.PersonFiles == null)
            {
                return NotFound();
            }
            
            var personFile = await _context.PersonFiles.FindAsync(id);
            if (personFile == null)
            {
                return NotFound();
            }

            if (type == null || type.Trim().ToLower() == "general")
            {
                // edit general information
                
                ViewData["GenderId"] = new SelectList(_context.Genders, "Id", "Name", personFile.GenderId);
                return View("EditGeneral",personFile);
            }
            if (type != null && type.Trim().ToLower() == "details")
            {
                // edit details
                
                return View("EditDetails",personFile);
            }

            if (type != null && type.Trim().ToLower() == "military")
            {
                // edit military information

                return RedirectToAction("Create", "MilitaryInformation", personFile.Id);
            }
            
            // unknown request
            return NotFound();
        }

        // POST: PersonFiles/Edit/5/General
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, string? type, [Bind("Id,FirstName,SecondName,GenderId,BirthDate,DeathDate,FamilyStatus,Education,Experience,MilitaryInformationId,HealthInformation,LegalInformation")] PersonFile personFile)
        {
            if (id != personFile.Id || type == null)
            {
                return NotFound();
            }

            type = type.ToLower().Trim();

            if (ModelState.IsValid && 
                (type != "general" || type != "details"))
            {
                try
                {
                    _context.Update(personFile);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PersonFileExists(personFile.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                if (type.Trim().ToLower() == "general")
                    return RedirectToAction("Edit", new
                    {
                        id = id, 
                        type = "details"
                    });
                
                if (type.Trim().ToLower() == "details")
                    return RedirectToAction("Create", "MilitaryInformation", new
                    {
                        id = id
                    });
            }
            ViewData["GenderId"] = new SelectList(_context.Genders, "Id", "Id", personFile.GenderId);
            return View("EditGeneral", personFile);
        }

        // GET: PersonFiles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.PersonFiles == null || _context.MilitaryInformations == null)
            {
                return NotFound();
            }

            var personFile = await _context.PersonFiles
                .Include(p => p.Gender)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (personFile == null)
            {
                return NotFound();
            }
            
            var militaryInformationQuery = _context.MilitaryInformations
                .Where(information => information.PersonFileId == id);
            if (militaryInformationQuery.Any())
                ViewBag.MilitaryInformation = militaryInformationQuery.First();
            else
                ViewBag.MilitaryInformation = null!;
            
            return View(personFile);
        }

        // POST: PersonFiles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.PersonFiles == null)
            {
                return Problem("Entity set 'IaDbContext.PersonFiles'  is null.");
            }
            var personFile = await _context.PersonFiles.FindAsync(id);
            if (personFile != null)
            {
                // remove dependencies — military information
                var militaryInformationRequest = _context.MilitaryInformations
                    .Where(information => information.PersonFileId == id);
                if (militaryInformationRequest.Any())
                    foreach (var militaryInformation in militaryInformationRequest)
                    {
                        _context.MilitaryInformations.Remove(militaryInformation);
                    }

                _context.PersonFiles.Remove(personFile);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PersonFileExists(int id)
        {
          return (_context.PersonFiles?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
