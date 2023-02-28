using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using IntelligenceAgencyManagementSystem;

namespace IntelligenceAgencyManagementSystem.Controllers
{
    public class MilitaryInformationController : Controller
    {
        private readonly IaDbContext _context;

        public MilitaryInformationController(IaDbContext context)
        {
            _context = context;
        }

        // GET: MilitaryInformation/5
        public async Task<IActionResult> Index(int? id)
        {
            if (id == null || _context.PersonFiles == null || _context.MilitaryInformations == null)
                return NotFound();

            var personFile = _context.PersonFiles.Find(id);

            if (personFile == null)
                return NotFound();

            return View("Details", null);
        }

        // GET: MilitaryInformation/Create/5
        public IActionResult Create(int id)
        {
            if (_context.PersonFiles == null || 
                _context.MilitaryInformations == null)
                return NotFound();
            
            if (_context.MilitaryInformations.Any(information => information.PersonFile.Id == id))
                return RedirectToAction("Edit", new {id = id});
                
            var personFile = _context.PersonFiles.Find(id);

            if (personFile == null)
                return NotFound();

            ViewBag.PersonFile = personFile;
            
            return View();
        }

        // POST: MilitaryInformation/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PersonFileId,MilitaryRank,FullInformation")] MilitaryInformation militaryInformation)
        {
            if (ModelState.IsValid)
            {
                _context.Add(militaryInformation);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "PersonFiles", new
                {
                    id = militaryInformation.PersonFileId
                });
            }
            return View(militaryInformation);
        }

        // GET: MilitaryInformation/Edit/5
        // get by PersonFile.Id
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.MilitaryInformations == null || _context.PersonFiles == null)
            {
                return NotFound();
            }

            if (!_context.MilitaryInformations.Any(information => information.PersonFileId == id))
            {
                return RedirectToAction("Create", new {id = id});
            }

            var militaryInformation = _context.MilitaryInformations.First(information => information.PersonFileId == id);
            var personFile = (await _context.PersonFiles.FindAsync(id))!;

            ViewBag.PersonFile = personFile;
            
            return View(militaryInformation);
        }

        // POST: MilitaryInformation/Edit/5
        // Edit by MilitaryInformation.Id
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,PersonFileId,MilitaryRank,FullInformation")] MilitaryInformation militaryInformation)
        {
            if (id != militaryInformation.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(militaryInformation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MilitaryInformationExists(militaryInformation.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Details", "PersonFiles", new
                {
                    id = militaryInformation.PersonFileId
                });
            }
            return View(militaryInformation);
        }

        // GET: MilitaryInformation/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.MilitaryInformations == null)
            {
                return NotFound();
            }

            var militaryInformation = await _context.MilitaryInformations
                .FirstOrDefaultAsync(m => m.Id == id);
            if (militaryInformation == null)
            {
                return NotFound();
            }

            return View(militaryInformation);
        }

        // POST: MilitaryInformation/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.MilitaryInformations == null)
            {
                return Problem("Entity set 'IaDbContext.MilitaryInformations'  is null.");
            }
            var militaryInformation = await _context.MilitaryInformations.FindAsync(id);
            if (militaryInformation != null)
            {
                _context.MilitaryInformations.Remove(militaryInformation);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MilitaryInformationExists(int id)
        {
          return (_context.MilitaryInformations?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
