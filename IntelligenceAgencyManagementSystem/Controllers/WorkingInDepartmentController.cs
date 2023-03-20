using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using IntelligenceAgencyManagementSystem;
using IntelligenceAgencyManagementSystem.Utils;

namespace IntelligenceAgencyManagementSystem.Controllers
{
    public class WorkingInDepartmentController : Controller
    {
        private readonly IaDbContext _context;

        public WorkingInDepartmentController(IaDbContext context)
        {
            _context = context;
        }

        // GET: WorkingInDepartment
        public async Task<IActionResult> Index()
        {
            var iaDbContext = _context
                .WorkingInDepartments
                .Include(w => w.Department)
                .Include(w => w.Role)
                .Include(w => w.Worker);
            return View(await iaDbContext.ToListAsync());
        }

        // GET: WorkingInDepartment/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.WorkingInDepartments == null)
            {
                return NotFound();
            }

            var workingInDepartment = await _context.WorkingInDepartments
                .Include(w => w.Department)
                .Include(w => w.Role)
                .Include(w => w.Worker)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (workingInDepartment == null)
            {
                return NotFound();
            }

            return View(workingInDepartment);
        }

        // GET: WorkingInDepartment/Create
        public IActionResult Create()
        {
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Name");
            ViewData["RoleId"] = new SelectList(_context.Roles, "Id", "Title");
            ViewData["WorkerId"] = new SelectList(_context.Workers, "Id", "FullName");
            return View();
        }

        // POST: WorkingInDepartment/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,WorkerId,DepartmentId,RoleId,Description,DateStarted,DateEnded")] WorkingInDepartment workingInDepartment)
        {
            try
            {
                if (workingInDepartment.DateStarted != null && workingInDepartment.DateEnded != null &&
                    workingInDepartment.DateEnded < workingInDepartment.DateStarted)
                    throw new Exception("Введіть вірні дати");
                
                if (ModelState.IsValid)
                {
                    _context.Add(workingInDepartment);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = e.Message;
            }
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Name", workingInDepartment.DepartmentId);
            ViewData["RoleId"] = new SelectList(_context.Roles, "Id", "Title", workingInDepartment.RoleId);
            ViewData["WorkerId"] = new SelectList(_context.Workers, "Id", "FullName", workingInDepartment.WorkerId);
            return View(workingInDepartment);
        }

        // GET: WorkingInDepartment/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.WorkingInDepartments == null)
            {
                return NotFound();
            }

            var workingInDepartment = await _context.WorkingInDepartments.FindAsync(id);
            if (workingInDepartment == null)
            {
                return NotFound();
            }
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Name", workingInDepartment.DepartmentId);
            ViewData["RoleId"] = new SelectList(_context.Roles, "Id", "Title", workingInDepartment.RoleId);
            ViewData["WorkerId"] = new SelectList(_context.Workers, "Id", "FullName", workingInDepartment.WorkerId);
            return View(workingInDepartment);
        }

        // POST: WorkingInDepartment/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,WorkerId,DepartmentId,RoleId,Description,DateStarted,DateEnded")] WorkingInDepartment workingInDepartment)
        {
            if (id != workingInDepartment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (workingInDepartment.DateStarted != null && workingInDepartment.DateEnded != null &&
                        workingInDepartment.DateEnded < workingInDepartment.DateStarted)
                        throw new Exception("Введіть вірні дати");
                    
                    _context.Update(workingInDepartment);
                    await _context.SaveChangesAsync();
                    
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception e)
                {
                    if (!WorkingInDepartmentExists(workingInDepartment.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        ViewBag.ErrorMessage = e.Message;
                    }
                }
            }
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Name", workingInDepartment.DepartmentId);
            ViewData["RoleId"] = new SelectList(_context.Roles, "Id", "Title", workingInDepartment.RoleId);
            ViewData["WorkerId"] = new SelectList(_context.Workers, "Id", "FullName", workingInDepartment.WorkerId);
            return View(workingInDepartment);
        }

        // GET: WorkingInDepartment/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.WorkingInDepartments == null)
            {
                return NotFound();
            }

            var workingInDepartment = await _context.WorkingInDepartments
                .Include(w => w.Department)
                .Include(w => w.Role)
                .Include(w => w.Worker)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (workingInDepartment == null)
            {
                return NotFound();
            }

            return View(workingInDepartment);
        }

        // POST: WorkingInDepartment/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.WorkingInDepartments == null)
            {
                return Problem("Entity set 'IaDbContext.WorkingInDepartments'  is null.");
            }
            var workingInDepartment = await _context.WorkingInDepartments.FindAsync(id);
            if (workingInDepartment != null)
            {
                _context.WorkingInDepartments.Remove(workingInDepartment);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WorkingInDepartmentExists(int id)
        {
          return (_context.WorkingInDepartments?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Import(IFormFile excelFile)
        {
            try
            {
                if (excelFile == null)
                    throw new NullReferenceException("Оберіть файл");

                await using (var stream = new FileStream(excelFile.FileName, FileMode.Create))
                {
                    await excelFile.CopyToAsync(stream);
                    using XLWorkbook workbook = new XLWorkbook(stream);
                    var importer = new WDImporter(_context);
                    importer.ImportExcel(workbook);
                }

                ViewBag.SuccessMessage = "Зміни збережено!";
            }
            catch (NullReferenceException e)
            {
                ViewBag.ErrorMessage = e.Message;
            }
            catch (FileFormatException)
            {
                ViewBag.ErrorMessage = "Невірний формат файлу!";
            }
            catch (FormatException e)
            {
                ViewBag.ErrorMessage = e.Message;
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = e.Message;
            }
            
            var iaDbContext = _context
                .WorkingInDepartments
                .Include(w => w.Department)
                .Include(w => w.Role)
                .Include(w => w.Worker);
            return View("Index", await iaDbContext.ToListAsync());
        }
    }
}
