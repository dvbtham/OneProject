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
    public class CategoryTasksController : Controller
    {
        private readonly SchoolContext _context;

        public CategoryTasksController(SchoolContext context)
        {
            _context = context;
        }

        // GET: CategoryTasks
        public async Task<IActionResult> Index(string currentFilter, string searchString, int? page)
        {
            ViewBag.currentFilter = searchString;
            var categoryTasks = from s in _context.CategoryTasks select s;
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            if (!string.IsNullOrEmpty(searchString))
                categoryTasks = categoryTasks.Where(x => x.Title.Contains(searchString));
           
            ViewData["CurrentFilter"] = searchString;
            int pageSize = 3;
            return View(await PaginatedList<CategoryTask>.CreateAsync(categoryTasks.OrderByDescending(x=>x.Title).AsNoTracking(), page ?? 1, pageSize));
        }

        // GET: CategoryTasks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categoryTask = await _context.CategoryTasks.SingleOrDefaultAsync(m => m.ID == id);
            if (categoryTask == null)
            {
                return NotFound();
            }

            return View(categoryTask);
        }

        // GET: CategoryTasks/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CategoryTasks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Description,IsActived,Title")] CategoryTask categoryTask)
        {
            if (ModelState.IsValid)
            {
                _context.Add(categoryTask);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(categoryTask);
        }

        // GET: CategoryTasks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categoryTask = await _context.CategoryTasks.SingleOrDefaultAsync(m => m.ID == id);
            if (categoryTask == null)
            {
                return NotFound();
            }
            return View(categoryTask);
        }

        // POST: CategoryTasks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Description,IsActived,Title")] CategoryTask categoryTask)
        {
            if (id != categoryTask.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(categoryTask);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryTaskExists(categoryTask.ID))
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
            return View(categoryTask);
        }

        // GET: CategoryTasks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categoryTask = await _context.CategoryTasks.SingleOrDefaultAsync(m => m.ID == id);
            if (categoryTask == null)
            {
                return NotFound();
            }

            return View(categoryTask);
        }

        // POST: CategoryTasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var categoryTask = await _context.CategoryTasks.SingleOrDefaultAsync(m => m.ID == id);
            _context.CategoryTasks.Remove(categoryTask);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool CategoryTaskExists(int id)
        {
            return _context.CategoryTasks.Any(e => e.ID == id);
        }
    }
}
