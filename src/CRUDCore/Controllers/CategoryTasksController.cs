using CRUDCore.Data;
using CRUDCore.Helpers;
using CRUDCore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CRUDCore.Controllers
{
    public class CategoryTasksController : BaseController
    {
        private readonly TaskManagementDbContext _context;

        public CategoryTasksController(TaskManagementDbContext context)
        {
            _context = context;
        }
        #region Methods
        public async Task<IActionResult> Index(string currentFilter, string searchString, int page = 1)
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
            int pageSize = 5;
            int totalRow = categoryTasks.Count();
            categoryTasks = categoryTasks.OrderBy(x => x.ID).Skip((page - 1) * pageSize).Take(pageSize);
            var model = new PaginatedList<CategoryTask>()
            {
                TotalCount = totalRow,
                TotalPages = (int)Math.Ceiling((double)totalRow / pageSize),
                Items = await categoryTasks.ToListAsync(),
                Page = page,
                MaxPage = 5
            };
            return PartialView(CommonConstants._CategoryTaskListsPartial, model);
        }

        public IActionResult Manager(int id)
        {
            ViewBag.Id = id;
            if (id > 0)
            {
                var model = _context.CategoryTasks.FirstOrDefault(x => x.ID == id);
                return View(model);
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Manager(int id, CategoryTask categoryTask)
        {
            if (id > 0)
            {
                #region Edit
                if (id != categoryTask.ID)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        _context.Update(categoryTask);
                        _context.SaveChanges();
                        SetAlert("Item Updated Successfully", "success");
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
                #endregion
            }
            else
            {
                if (ModelState.IsValid)
                {
                    _context.Add(categoryTask);
                    _context.SaveChanges();
                    SetAlert("Item Added Successfully", "success");
                    return RedirectToAction("Index");
                }
                return View(categoryTask);
            }
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categoryTask = await _context.CategoryTasks.Include(x => x.Tasks).SingleOrDefaultAsync(m => m.ID == id);
            if (categoryTask == null)
            {
                return NotFound();
            }

            return View(categoryTask);
        }

        [HttpPost]
        public JsonResult Delete(int id)
        {
            try
            {
                var categoryTask = _context.CategoryTasks.SingleOrDefault(m => m.ID == id);
                if (categoryTask == null)
                    return Json(new
                    {
                        status = false,
                        message = "Not found!"
                    });
                _context.CategoryTasks.Remove(categoryTask);
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

        private bool CategoryTaskExists(int id)
        {
            return _context.CategoryTasks.Any(e => e.ID == id);
        }
        #endregion
    }
}