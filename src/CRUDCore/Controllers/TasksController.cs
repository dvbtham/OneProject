using CRUDCore.Data;
using CRUDCore.Helpers;
using CRUDCore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CRUDCore.Controllers
{
    public class TasksController : BaseController
    {
        private readonly TaskManagementDbContext _context;

        public TasksController(TaskManagementDbContext context)
        {
            _context = context;
        }
        #region Methods
        public async Task<IActionResult> Index(string currentFilter, string searchString, int page = 1)
        {
            ViewBag.currentFilter = searchString;
            var tasks = from s in _context.Tasks.Include(x => x.CategoryTask) select s;

            if (!string.IsNullOrEmpty(searchString))
            {
                page = 1;
                tasks = tasks.Where(x => x.Title.Contains(searchString) || x.CategoryTask.Title.Contains(searchString));
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;
            int pageSize = 5;
            int totalRow = tasks.Count();
            tasks = tasks.OrderBy(x => x.ID).Skip((page - 1) * pageSize).Take(pageSize);
            var model = new PaginatedList<Tasks>()
            {
                TotalCount = totalRow,
                TotalPages = (int)Math.Ceiling((double)totalRow / pageSize),
                Items = await tasks.ToListAsync(),
                Page = page,
                MaxPage = 5
            };
            return PartialView(CommonConstants._TaskListsPartial, model);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var tasks = await _context.Tasks.Include(x => x.CategoryTask).FirstOrDefaultAsync(m => m.ID == id);
            if (tasks == null)
            {
                return NotFound();
            }
            return View(tasks);
        }

        public IActionResult Manager(int id)
        {
            ViewBag.Id = id;
            var cateTasks = from s in _context.CategoryTasks select s;
            if (id > 0)
            {
                var tasks = _context.Tasks.FirstOrDefault(m => m.ID == id);
                tasks.CategoryTasks = new SelectList(cateTasks, "ID", "Title");
                return View(tasks);
            }
            else
            {
                var model = new Tasks();
                model.CategoryTasks = new SelectList(cateTasks, "ID", "Title");
                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Manager(int id, Tasks taskModel)
        {
            if (id > 0)
            {
                #region Edit Tasks
                if (id != taskModel.ID)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        _context.Update(taskModel);
                        _context.SaveChanges();
                        SetAlert("Item Updated Successfully", "success");
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!TasksExists(taskModel.ID))
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
                return View(taskModel);
                #endregion
            }
            else
            {
                #region Create Tasks
                if (ModelState.IsValid)
                {
                    _context.Add(taskModel);
                    _context.SaveChanges();
                    SetAlert("Item Added Successfully", "success");
                    return RedirectToAction("Index");
                }
                return View(taskModel);
                #endregion
            }
        }

        [HttpPost]
        public JsonResult Delete(int id)
        {
            try
            {
                var task = _context.Tasks.FirstOrDefault(m => m.ID == id);
                if (task == null)
                    return Json(new
                    {
                        status = false,
                        message = "Not found!"
                    });
                _context.Tasks.Remove(task);
                _context.SaveChanges();
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

        public async Task<IActionResult> TasksByCategory(int id, int page = 1)
        {
            int pageSize = 5;
            var tasks = _context.Tasks.Where(x => x.IdCategoryTask == id).Include(x => x.CategoryTask);
            var model = new PaginatedList<Tasks>()
            {
                TotalCount = tasks.Count(),
                TotalPages = (int)Math.Ceiling((double)tasks.Count() / pageSize),
                Items = await tasks.ToListAsync(),
                Page = page,
                MaxPage = 5
            };
            return PartialView(CommonConstants._TaskListsPartial, model);
        }

        private bool TasksExists(int id)
        {
            return _context.Tasks.Any(e => e.ID == id);
        }
        #endregion
    }
}