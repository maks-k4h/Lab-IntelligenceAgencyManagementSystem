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
    public class TaskStatusesController : Controller
    {
        private readonly IaDbContext _context;

        public TaskStatusesController(IaDbContext context)
        {
            _context = context;
        }

        // GET: TaskStatuses
        public async Task<IActionResult> Index()
        {
              return _context.TaskStatuses != null ? 
                          View(await _context.TaskStatuses.ToListAsync()) :
                          Problem("Entity set 'IaDbContext.TaskStatuses'  is null.");
        }

        // GET: TaskStatuses/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TaskStatuses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Description")] TaskStatus taskStatus)
        {
            if (ModelState.IsValid)
            {
                _context.Add(taskStatus);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(taskStatus);
        }

        // GET: TaskStatuses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.TaskStatuses == null)
            {
                return NotFound();
            }

            var taskStatus = await _context.TaskStatuses.FindAsync(id);
            if (taskStatus == null)
            {
                return NotFound();
            }
            return View(taskStatus);
        }

        // POST: TaskStatuses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description")] TaskStatus taskStatus)
        {
            if (id != taskStatus.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(taskStatus);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TaskStatusExists(taskStatus.Id))
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
            return View(taskStatus);
        }

        // GET: TaskStatuses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.TaskStatuses == null)
            {
                return NotFound();
            }

            var taskStatus = await _context.TaskStatuses
                .FirstOrDefaultAsync(m => m.Id == id);
            if (taskStatus == null)
            {
                return NotFound();
            }

            return View(taskStatus);
        }

        // POST: TaskStatuses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.TaskStatuses == null)
            {
                return Problem("Entity set 'IaDbContext.TaskStatuses'  is null.");
            }
            var taskStatus = await _context.TaskStatuses.FindAsync(id);
            if (taskStatus != null)
            {
                _context.TaskStatuses.Remove(taskStatus);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TaskStatusExists(int id)
        {
          return (_context.TaskStatuses?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
