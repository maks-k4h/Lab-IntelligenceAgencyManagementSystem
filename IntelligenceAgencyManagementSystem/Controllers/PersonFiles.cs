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
    public class PersonFiles : Controller
    {
        private readonly IaDbContext _context;

        public PersonFiles(IaDbContext context)
        {
            _context = context;
        }

        // GET: PersonFiles
        public async Task<IActionResult> Index()
        {
            var iaDbContext = _context.PersonFiles.Include(p => p.Gender).Include(p => p.MilitaryInformation);
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
                .Include(p => p.MilitaryInformation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (personFile == null)
            {
                return NotFound();
            }

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
        public async Task<IActionResult> Create([Bind("Id,FirstName,SecondName,GenderId,BirthDate,DeathDate,FamilyStatus,Education,Experience,MilitaryInformationId,HealthInformation,LegalInformation")] PersonFile personFile)
        {
            if (ModelState.IsValid)
            {
                _context.Add(personFile);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["GenderId"] = new SelectList(_context.Genders, "Id", "Id", personFile.GenderId);
            ViewData["MilitaryInformationId"] = new SelectList(_context.MilitaryInformations, "Id", "Id", personFile.MilitaryInformationId);
            return View(personFile);
        }

        // GET: PersonFiles/Edit/5
        public async Task<IActionResult> Edit(int? id)
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
            ViewData["GenderId"] = new SelectList(_context.Genders, "Id", "Name", personFile.GenderId);
            ViewData["MilitaryInformationId"] = new SelectList(_context.MilitaryInformations, "Id", "Id", personFile.MilitaryInformationId);
            return View(personFile);
        }

        // POST: PersonFiles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,SecondName,GenderId,BirthDate,DeathDate,FamilyStatus,Education,Experience,MilitaryInformationId,HealthInformation,LegalInformation")] PersonFile personFile)
        {
            if (id != personFile.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
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
                return RedirectToAction(nameof(Index));
            }
            ViewData["GenderId"] = new SelectList(_context.Genders, "Id", "Id", personFile.GenderId);
            ViewData["MilitaryInformationId"] = new SelectList(_context.MilitaryInformations, "Id", "Id", personFile.MilitaryInformationId);
            return View(personFile);
        }

        // GET: PersonFiles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.PersonFiles == null)
            {
                return NotFound();
            }

            var personFile = await _context.PersonFiles
                .Include(p => p.Gender)
                .Include(p => p.MilitaryInformation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (personFile == null)
            {
                return NotFound();
            }

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
