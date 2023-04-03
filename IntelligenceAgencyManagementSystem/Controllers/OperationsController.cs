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
    [Authorize(Roles="admin, chairman, commander, agent")]
    public class OperationsController : Controller
    {
        private readonly IaDbContext _context;

        public OperationsController(IaDbContext context)
        {
            _context = context;
        }

        public Task<IActionResult> Visualization()
        {
            return System.Threading.Tasks.Task.FromResult<IActionResult>(View());
        }

        // GET: Operations/5
        public async Task<IActionResult> Index(int? id)
        {
            if (id == null)
                return RedirectToAction("Index", "Departments");

            var department = _context.Departments.FirstOrDefault(department => department.Id == id);
            if (department == null)
                return NotFound();

            ViewBag.DepartmentId = id;
            ViewBag.DepartmentName = department.Name;

            // var iaDbContext = _context.Operations.Include(o => o.Department);
            var iaDbContext = _context.Operations
                .Where(operation => operation.DepartmentId == id)
                .Include(o => o.Department);
            return View(await iaDbContext.ToListAsync());
        }

        // GET: Operations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Operations == null)
                return NotFound();

            var operation = await _context.Operations
                .Include(o => o.Department)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (operation == null)
            {
                return NotFound();
            }

            return View(operation);
        }

        // GET: Operations/Create/5
        public IActionResult Create(int? id)
        {
            if (id == null)
                return RedirectToAction("Index", "Departments");
            
            ViewBag.DepartmentId = id;            
            return View();
        }

        // POST: Operations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Description,DepartmentId,DateStarted,DateEnded")] Operation operation)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    
                    if (operation.DateStarted != null && operation.DateEnded != null &&
                        operation.DateEnded < operation.DateStarted)
                        throw new Exception("Вкажіть вірні дати початку та завершення операції");
                    
                    _context.Add(operation);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index), new {id = operation.DepartmentId});
                }
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = e.Message;
            }
            ViewBag.DepartmentId = operation.DepartmentId;            
            return View(operation);
        }

        // GET: Operations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Operations == null)
            {
                return NotFound();
            }

            var operation = await _context.Operations.FindAsync(id);
            if (operation == null)
            {
                return NotFound();
            }

            ViewBag.Department = await _context.Departments.FindAsync(operation.DepartmentId);
            
            return View(operation);
        }

        // POST: Operations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,DepartmentId,DateStarted,DateEnded")] Operation operation)
        {
            if (id != operation.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (operation.DateStarted != null && operation.DateEnded != null &&
                        operation.DateEnded < operation.DateStarted)
                        throw new Exception("Вкажіть вірні дати початку та завершення операції");
                    
                    _context.Update(operation);
                    await _context.SaveChangesAsync();
                    
                    return RedirectToAction("Index", "Operations", new {id = operation.DepartmentId});
                }
                catch (Exception e)
                {
                    if (!OperationExists(operation.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        ViewBag.ErrorMessage = e.Message;
                    }
                }
                
            }
            
            ViewBag.Department = await _context.Departments.FindAsync(operation.DepartmentId);            
            return View(operation);
        }

        // GET: Operations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Operations == null)
            {
                return NotFound();
            }

            var operation = await _context.Operations
                .Include(o => o.Department)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (operation == null)
            {
                return NotFound();
            }

            return View(operation);
        }

        // POST: Operations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Operations == null)
            {
                return Problem("Entity set 'IaDbContext.Operations'  is null.");
            }
            var operation = await _context.Operations.FindAsync(id);
            if (operation != null)
            {
                _context.Operations.Remove(operation);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), operation == null ? 
                new {} : 
                new {id = operation.DepartmentId}
            );
        }

        private bool OperationExists(int id)
        {
          return (_context.Operations?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
