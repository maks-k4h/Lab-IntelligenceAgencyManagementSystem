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
    public class MilitaryFilesController : Controller
    {
        private readonly IaDbContext _context;

        public MilitaryFilesController(IaDbContext context)
        {
            _context = context;
        }

        // GET: MilitaryFiles/5
        // Get military files by worker id
        public async Task<IActionResult> Index(int? id)
        {
            return RedirectToAction("Details", "Workers", new
            {
                id = id
            });
        }

        // GET: MilitaryFiles/Create/5
        public IActionResult Create(int id)
        {
            if (_context.Workers == null || 
                _context.MilitaryFiles == null)
                return NotFound();
            
            if (_context.MilitaryFiles.Any(file => file.WorkerId == id))
                return RedirectToAction("Edit", new {id = id});
                
            var worker = _context.Workers.Find(id);

            if (worker == null)
                return NotFound();

            ViewBag.Worker = worker;
            
            return View();
        }

        // POST: MilitaryFiles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("WorkerId,MilitaryRank,FullInformation")] MilitaryFile militaryFile)
        {
            if (ModelState.IsValid)
            {
                _context.Add(militaryFile);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Workers", new
                {
                    id = militaryFile.WorkerId
                });
            }
            return View(militaryFile);
        }

        // GET: MilitaryFiles/Edit/5
        // get by Worker.Id
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.MilitaryFiles == null || _context.Workers == null)
            {
                return NotFound();
            }

            if (!_context.MilitaryFiles.Any(file => file.WorkerId == id))
            {
                return RedirectToAction("Create", new {id = id});
            }

            var militaryFile = _context.MilitaryFiles.First(file => file.WorkerId == id);
            var worker = (await _context.Workers.FindAsync(id))!;

            ViewBag.Worker = worker;
            
            return View(militaryFile);
        }

        // POST: MilitaryFiles/Edit/5
        // Edit by MilitaryFiles.Id
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,WorkerId,MilitaryRank,FullInformation")] MilitaryFile militaryFile)
        {
            if (id != militaryFile.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(militaryFile);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MilitaryFileExists(militaryFile.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Details", "Workers", new
                {
                    id = militaryFile.WorkerId
                });
            }
            return View(militaryFile);
        }

        // GET: MilitaryFiles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.MilitaryFiles == null)
            {
                return NotFound();
            }

            var militaryFiles = await _context.MilitaryFiles
                .FirstOrDefaultAsync(m => m.Id == id);
            if (militaryFiles == null)
            {
                return NotFound();
            }

            ViewBag.Worker = await _context.Workers.FindAsync(militaryFiles.WorkerId);

            return View(militaryFiles);
        }

        // POST: MilitaryFiles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.MilitaryFiles == null)
            {
                return Problem("Entity set 'IaDbContext.MilitaryFiles  is null.");
            }
            var militaryFiles = await _context.MilitaryFiles.FindAsync(id);
            if (militaryFiles != null)
            {
                var workerId = militaryFiles.WorkerId;
                _context.MilitaryFiles.Remove(militaryFiles);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Workers", new
                {
                    id = workerId
                });
            }

            return RedirectToAction("Index", "Workers");
        }

        private bool MilitaryFileExists(int id)
        {
          return (_context.MilitaryFiles?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
