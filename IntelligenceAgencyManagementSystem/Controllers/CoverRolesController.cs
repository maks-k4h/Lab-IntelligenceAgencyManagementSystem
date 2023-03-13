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
    public class CoverRolesController : Controller
    {
        private readonly IaDbContext _context;

        public CoverRolesController(IaDbContext context)
        {
            _context = context;
        }

        // GET: CoverRoles
        public async Task<IActionResult> Index()
        {
            var iaDbContext = _context.CoverRoles.Include(c => c.Gender);
            return View(await iaDbContext.ToListAsync());
        }

        // GET: CoverRoles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.CoverRoles == null)
            {
                return NotFound();
            }

            var coverRole = await _context.CoverRoles
                .Include(c => c.Gender)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (coverRole == null)
            {
                return NotFound();
            }

            return View(coverRole);
        }

        // GET: CoverRoles/Create
        public IActionResult Create()
        {
            ViewData["GenderId"] = new SelectList(_context.Genders, "Id", "Name");
            return View();
        }

        // POST: CoverRoles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,SecondName,GenderId,BirthDate,DeathDate,DateActivated,DateDeactivated,Legend,ActivitySummary")] CoverRole coverRole)
        {
            try
            {
                if (coverRole.BirthDate != null && coverRole.DeathDate != null &&
                    coverRole.DeathDate < coverRole.BirthDate)
                    throw new Exception("Вкажіть правильні дати народження та смерті");
                
                if (coverRole.DateActivated != null && coverRole.DateDeactivated != null &&
                    coverRole.DateDeactivated < coverRole.DateActivated)
                    throw new Exception("Вкажіть правильні дати активності");
                
                if (ModelState.IsValid)
                {
                    _context.Add(coverRole);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = e.Message;
            }
            ViewData["GenderId"] = new SelectList(_context.Genders, "Id", "Name", coverRole.GenderId);
            return View(coverRole);
        }

        // GET: CoverRoles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.CoverRoles == null)
            {
                return NotFound();
            }

            var coverRole = await _context.CoverRoles.FindAsync(id);
            if (coverRole == null)
            {
                return NotFound();
            }
            ViewData["GenderId"] = new SelectList(_context.Genders, "Id", "Name", coverRole.GenderId);
            return View(coverRole);
        }

        // POST: CoverRoles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,SecondName,GenderId,BirthDate,DeathDate,DateActivated,DateDeactivated,Legend,ActivitySummary")] CoverRole coverRole)
        {
            if (id != coverRole.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (coverRole.BirthDate != null && coverRole.DeathDate != null &&
                        coverRole.DeathDate < coverRole.BirthDate)
                        throw new Exception("Вкажіть правильні дати народження та смерті");
                
                    if (coverRole.DateActivated != null && coverRole.DateDeactivated != null &&
                        coverRole.DateDeactivated < coverRole.DateActivated)
                        throw new Exception("Вкажіть правильні дати активності");
                    
                    _context.Update(coverRole);
                    await _context.SaveChangesAsync();
                    
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception e)
                {
                    if (!CoverRoleExists(coverRole.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        ViewBag.ErrorMessage = e.Message;
                    }
                }
            }
            ViewData["GenderId"] = new SelectList(_context.Genders, "Id", "Name", coverRole.GenderId);
            return View(coverRole);
        }

        // GET: CoverRoles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.CoverRoles == null)
            {
                return NotFound();
            }

            var coverRole = await _context.CoverRoles
                .Include(c => c.Gender)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (coverRole == null)
            {
                return NotFound();
            }

            ViewBag.CanDelete = !_context.WorkersToOps.Any(wop => wop.CoverRoleId == id);

            return View(coverRole);
        }

        // POST: CoverRoles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.CoverRoles == null)
            {
                return Problem("Entity set 'IaDbContext.CoverRoles'  is null.");
            }
            var coverRole = await _context.CoverRoles.FindAsync(id);
            if (coverRole != null)
            {
                _context.CoverRoles.Remove(coverRole);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CoverRoleExists(int id)
        {
          return (_context.CoverRoles?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
