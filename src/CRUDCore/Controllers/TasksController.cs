using CRUDCore.Data;
using CRUDCore.Helpers;
using CRUDCore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace CRUDCore.Controllers
{
    public class TasksController : BaseController
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
            var tasks = from s in _context.Tasks.Include(x => x.CategoryTask) select s;
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
            return View(await PaginatedList<Tasks>.CreateAsync(tasks.AsNoTracking().OrderByDescending(x => x.Title).AsNoTracking(), page ?? 1, pageSize));
        }

        // GET: Tasks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tasks = await _context.Tasks.Include(x => x.CategoryTask).SingleOrDefaultAsync(m => m.ID == id);
            if (tasks == null)
            {
                return NotFound();
            }

            return View(tasks);
        }

        // GET: Tasks/Create
        public IActionResult Create()
        {
            var cateTasks = from s in _context.CategoryTasks select s;
            var model = new Tasks();
            model.CategoryTasks = new SelectList(cateTasks, "ID", "Title");
            return View(model);
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
                SetAlert("Item Added Successfully", "success");
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
            var cateTasks = from s in _context.CategoryTasks select s;
            if (tasks == null)
            {
                return NotFound();
            }
            tasks.CategoryTasks = new SelectList(cateTasks, "ID", "Title");
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
                    SetAlert("Item Updated Successfully", "success");
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

        [HttpPost]
        public async Task<JsonResult> Delete(int id)
        {
            try
            {
                var task = await _context.Tasks.SingleOrDefaultAsync(m => m.ID == id);
                if (task == null)
                    return Json(new
                    {
                        status = false,
                        message = "Not found!"
                    });
                _context.Tasks.Remove(task);
                await _context.SaveChangesAsync();
                SetAlert("Item Deleted Successfully", "success");
                return Json(new
                {
                    status = true
                });
            }
            catch (DbUpdateException ex)
            {
                return Json(new
                {
                    status = false,
                    message = ex.Message
                });
            }
        }

        private bool TasksExists(int id)
        {
            return _context.Tasks.Any(e => e.ID == id);
        }
    }
}