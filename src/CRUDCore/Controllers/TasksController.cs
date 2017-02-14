using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CRUDCore.Data;
using CRUDCore.Models;
using CRUDCore.Helpers;

namespace CRUDCore.Controllers
{
    public class TasksController : Controller
    {
        private readonly SchoolContext _context;

        public TasksController(SchoolContext context)
        {
            _context = context;    
        }

        // GET: Tasks
        public async Task<IActionResult> Index(string currentFilter, string searchString, int? page)
        {
            ViewBag.currentFilter = searchString;
            var tasks = from s in _context.Tasks select s;
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            if (!string.IsNullOrEmpty(searchString))
                tasks = tasks.Where(x => x.Title.Contains(searchString));

            ViewData["CurrentFilter"] = searchString;
            int pageSize = 3;
            return View(await PaginatedList<Tasks>.CreateAsync(tasks.OrderByDescending(x => x.Title).AsNoTracking(), page ?? 1, pageSize));
        }

        // GET: Tasks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tasks = await _context.Tasks.SingleOrDefaultAsync(m => m.ID == id);
            if (tasks == null)
            {
                return NotFound();
            }

            return View(tasks);
        }

        // GET: Tasks/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Tasks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,DeadlineDate,Description,FromDate,IdCategoryTask,IsActive,Title,UnitPer")] Tasks tasks)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tasks);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(tasks);
        }

        // GET: Tasks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tasks = await _context.Tasks.SingleOrDefaultAsync(m => m.ID == id);
            if (tasks == null)
            {
                return NotFound();
            }
            return View(tasks);
        }

        // POST: Tasks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,DeadlineDate,Description,FromDate,IdCategoryTask,IsActive,Title,UnitPer")] Tasks tasks)
        {
            if (id != tasks.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tasks);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TasksExists(tasks.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            return View(tasks);
        }

        // GET: Tasks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tasks = await _context.Tasks.SingleOrDefaultAsync(m => m.ID == id);
            if (tasks == null)
            {
                return NotFound();
            }

            return View(tasks);
        }

        // POST: Tasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tasks = await _context.Tasks.SingleOrDefaultAsync(m => m.ID == id);
            _context.Tasks.Remove(tasks);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool TasksExists(int id)
        {
            return _context.Tasks.Any(e => e.ID == id);
        }
    }
}
