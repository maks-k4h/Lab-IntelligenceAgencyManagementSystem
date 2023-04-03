using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using IntelligenceAgencyManagementSystem;
using Microsoft.AspNetCore.Authorization;

namespace IntelligenceAgencyManagementSystem.Controllers
{
    [Authorize(Roles="admin, chairman, commander")]
    public class WorkersToOperationsController : Controller
    {
        private readonly IaDbContext _context;

        public WorkersToOperationsController(IaDbContext context)
        {
            _context = context;
        }

        // GET: WorkersToOperations/5
        public async Task<IActionResult> Index(int id)
        {
            var operation = await _context.Operations.FirstOrDefaultAsync(op => op.Id == id);

            if (operation == null)
            {
                return NotFound();
            }

            ViewBag.Operation = operation;
            
            var iaDbContext = _context.WorkersToOps
                .Where(wto => wto.OperationId == id)
                .Include(w => w.CoverRole)
                .Include(w => w.Worker);
            return View(await iaDbContext.ToListAsync());
        }

        // GET: WorkersToOperations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.WorkersToOps == null)
            {
                return NotFound();
            }

            var workersToOp = await _context.WorkersToOps
                .Include(w => w.CoverRole)
                .Include(w => w.Operation)
                .Include(w => w.Worker)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (workersToOp == null)
            {
                return NotFound();
            }

            return View(workersToOp);
        }

        // GET: WorkersToOperations/Create/id
        // Create by the department's id
        public async Task<IActionResult> Create(int id)
        {
            var operation = await _context.Operations.FirstOrDefaultAsync(op => op.Id == id);

            if (operation == null)
            {
                return NotFound();
            }

            ViewBag.Operation = operation;
            
            ViewData["CoverRoleId"] = new SelectList(_context.CoverRoles, "Id", "FullName");
            ViewData["WorkerId"] = new SelectList(_context.Workers, "Id", "FullName");
            
            return View();
        }

        // POST: WorkersToOperations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CoverRoleId,OperationId,WorkerId")] WorkersToOp workersToOp)
        {
            // first check if the model is valid and if we already have such a record
            if (ModelState.IsValid && !_context.WorkersToOps.Any(wo => 
                    wo.WorkerId == workersToOp.WorkerId &&
                    wo.OperationId == workersToOp.OperationId &&
                    wo.CoverRoleId == workersToOp.CoverRoleId))
            {
                _context.Add(workersToOp);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new
                {
                    id = workersToOp.OperationId
                });
            }
            
            var operation = await _context.Operations.FirstOrDefaultAsync(op => op.Id == workersToOp.OperationId);

            if (operation == null)
            {
                return NotFound();
            }

            ViewBag.Operation = operation;
            
            ViewData["CoverRoleId"] = new SelectList(_context.CoverRoles, "Id", "FullName", workersToOp.CoverRoleId);
            ViewData["WorkerId"] = new SelectList(_context.Workers, "Id", "FullName", workersToOp.WorkerId);
            return View(workersToOp);
        }

        // GET: WorkersToOperations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.WorkersToOps == null)
            {
                return NotFound();
            }

            var workersToOp = await _context.WorkersToOps
                .Include(w => w.CoverRole)
                .Include(w => w.Operation)
                .Include(w => w.Worker)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (workersToOp == null)
            {
                return NotFound();
            }

            return View(workersToOp);
        }

        // POST: WorkersToOperations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Workers == null)
            {
                return Problem("Entity set 'IaDbContext.WorkersToOps'  is null.");
            }
            var workersToOp = await _context.WorkersToOps.FindAsync(id);
            int? operationId = workersToOp?.OperationId;
            if (workersToOp != null)
            {
                _context.WorkersToOps.Remove(workersToOp);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new
            {
                id = operationId
            });
        }

        private bool WorkersToOpExists(int id)
        {
          return (_context.WorkersToOps?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
