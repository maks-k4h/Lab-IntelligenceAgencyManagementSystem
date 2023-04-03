using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using IntelligenceAgencyManagementSystem;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;

namespace IntelligenceAgencyManagementSystem.Controllers
{
    [Authorize(Roles="admin, chairman, commander, agent")]
    public class TasksController : Controller
    {
        private readonly IaDbContext _context;

        public TasksController(IaDbContext context)
        {
            _context = context;
        }

        // GET: Tasks/2
        // Tasks by an operation's id
        public async Task<IActionResult> Index(int id)
        {
            var operation = await _context.Operations.FindAsync(id);

            if (operation == null)
            {
                return NotFound();
            }

            ViewBag.OperationName = operation.Name;
            ViewBag.OperationId = operation.Id;

            var tasks = _context.Tasks
                .Where(task => task.OperationId == id)
                .Include(task => task.Status);

            return View(tasks);
        }

        // GET: Tasks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Tasks == null)
            {
                return NotFound();
            }

            var task = await _context.Tasks
                .Include(t => t.Operation)
                .Include(t => t.Status)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (task == null)
            {
                return NotFound();
            }

            ViewBag.TasksToWorkers = _context.TasksToWorkers
                .Where(tw => tw.TaskId == id)
                .Include(tw => tw.Worker).ToList();

            return View(task);
        }

        // GET: Tasks/Create/5
        public async Task<IActionResult> Create(int id)
        {
            var operation = await _context.Operations.FindAsync(id);
            if (operation == null)
            {
                return NotFound();
            }

            ViewBag.OperationName = operation.Name;
            ViewBag.OperationId = operation.Id;
            
            ViewData["StatusId"] = new SelectList(_context.TaskStatuses, "Id", "Title");
            return View();
        }

        // POST: Tasks/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OperationId,Title,Description,StatusId,DateStatusSet")] Task task)
        {
            if (ModelState.IsValid)
            {
                _context.Add(task);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new
                {
                    id = task.OperationId
                });
            }
            
            var operation = await _context.Operations.FindAsync(task.OperationId);
            if (operation == null)
            {
                return NotFound();
            }

            ViewBag.OperationName = operation.Name;
            ViewBag.OperationId = operation.Id;
            
            ViewData["StatusId"] = new SelectList(_context.TaskStatuses, "Id", "Title", task.StatusId);
            return View(task);
        }
        
        // GET: Tasks/AddWorker/5
        public async Task<IActionResult> AddWorker(int id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null)
            {
                return NotFound();
            }

            ViewBag.TaskTitle = task.Title;
            ViewBag.TaskId = task.Id;
            
            ViewData["WorkerId"] = new SelectList(_context.Workers, "Id", "FullName");
            return View();
        }

        // POST: Tasks/AddWorker
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddWorker([Bind("TaskId, WorkerId")] TasksToWorkers taskToWorker)
        {
            if (ModelState.IsValid)
            {
                if (_context.TasksToWorkers.Any(tw =>
                        tw.WorkerId == taskToWorker.WorkerId && tw.TaskId == taskToWorker.TaskId))
                    return RedirectToAction(nameof(AddWorker), new
                    {
                        id = taskToWorker.TaskId
                    });
                
                _context.Add(taskToWorker);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Details), new
                {
                    id = taskToWorker.TaskId
                });
            }
            
            var task = await _context.Tasks.FindAsync(taskToWorker.TaskId);
            if (task == null)
            {
                return NotFound();
            }
            
            ViewBag.TaskTitle = task.Title;
            ViewBag.TaskId = task.Id;
            
            ViewData["WorkerId"] = new SelectList(_context.Workers, "Id", "FullName");
            return View();
        }

        // GET: Tasks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Tasks == null)
            {
                return NotFound();
            }

            var task = await _context.Tasks.FindAsync(id);
            if (task == null)
            {
                return NotFound();
            }

            var operation = await _context.Operations.FindAsync(task.OperationId);

            if (operation == null)
                return NotFound();

            ViewBag.OperationId = operation.Id;
            ViewBag.OperationName = operation.Name;
            
            ViewData["StatusId"] = new SelectList(_context.TaskStatuses, "Id", "Title", task.StatusId);
            return View(task);
        }

        // POST: Tasks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,OperationId,Title,Description,StatusId,DateStatusSet")] Task task)
        {
            if (id != task.Id)
            {
                return NotFound();
            }
            
            // update the timestamp
            task.DateStatusSet = DateTime.Now;

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(task);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TaskExists(task.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Details), new
                {
                    id = task.Id
                });
            }

            var operation = await _context.Operations.FindAsync(task.OperationId);

            if (operation == null)
                return NotFound();
            
            ViewBag.OperationId = operation.Id;
            ViewBag.OperationName = operation.Name;
            
            ViewData["StatusId"] = new SelectList(_context.TaskStatuses, "Id", "Name", task.StatusId);
            return View(task);
        }

        // GET: Tasks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Tasks == null)
            {
                return NotFound();
            }

            var task = await _context.Tasks
                .Include(t => t.Operation)
                .Include(t => t.Status)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (task == null)
            {
                return NotFound();
            }

            return View(task);
        }

        // POST: Tasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Tasks == null)
            {
                return Problem("Entity set 'IaDbContext.Tasks'  is null.");
            }
            var task = await _context.Tasks.FindAsync(id);
            int? operationId = task?.OperationId;
            if (task != null)
            {
                _context.Tasks.Remove(task);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new
            {
                id = operationId
            });
        }
        
        // POST: Tasks/RemoveWorker/5
        // Remove TasksToWorkers record by its id
        [HttpPost, ActionName("RemoveWorker")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveWorkerConfirmed(int id)
        {
            if (_context.TasksToWorkers == null)
            {
                return Problem("Entity set 'IaDbContext.TasksToWorkers'  is null.");
            }
            var taskToWorker = await _context.TasksToWorkers.FindAsync(id);
            int? taskId = taskToWorker?.TaskId;
            if (taskToWorker != null)
            {
                _context.TasksToWorkers.Remove(taskToWorker);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Details), new
            {
                id = taskId
            });
        }

        private bool TaskExists(int id)
        {
          return (_context.Tasks?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
