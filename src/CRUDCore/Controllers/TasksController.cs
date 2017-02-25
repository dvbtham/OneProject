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
        public async Task<IActionResult> Index(string searchString, int page = 1)
        {
            ViewBag.currentFilter = searchString;
            var tasks = from s in _context.Tasks.Include(x => x.CategoryTask) select s;
            
            if (!string.IsNullOrEmpty(searchString))
            {
                page = 1;
                tasks = tasks.Where(x => x.Title.Contains(searchString.Trim()) || x.CategoryTask.Title.Contains(searchString));
            }
            
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
                return PageNotFound();
            }
            var tasks = await _context.Tasks.Include(x => x.CategoryTask).FirstOrDefaultAsync(m => m.ID == id);
            if (tasks == null)
            {
                return PageNotFound();
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
                if (tasks == null)
                    return PageNotFound();
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
            if (taskModel.UnitPer < 0 || taskModel.UnitPer > 100)
            {
                SetAlert("Value is invalid (Value must be between 0 and 100)", "error");
                return RedirectToAction("Manager");
            }
            taskModel.UnitPer = (float)Math.Round(taskModel.UnitPer, 2);
            if (taskModel.DeadlineDate < taskModel.FromDate)
            {
                SetAlert("Value is invalid (DeadlineDate value must be greater than FromDate value) ", "error");
                return RedirectToAction("Manager");
            }

            if (ModelState.IsValid)
            {
                if (id > 0)
                {
                    if (id != taskModel.ID)
                    {
                        return PageNotFound();
                    }
                    _context.Update(taskModel);
                    _context.SaveChanges();
                    SetAlert(CommonConstants.UpdateSuccess, "success");

                    return RedirectToAction("Index");

                }
                else
                {
                    _context.Add(taskModel);
                    _context.SaveChanges();
                    SetAlert(CommonConstants.AddSuccess, "success");
                    return RedirectToAction("Index");
                }
            }
            return View(taskModel);

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
                        message = "Task could not be found!"
                    });
                _context.Tasks.Remove(task);
                _context.SaveChanges();
                SetAlert(CommonConstants.DeleteSuccess, "success");
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