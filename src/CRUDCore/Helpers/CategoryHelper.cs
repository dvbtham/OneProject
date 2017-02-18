using CRUDCore.Data;
using CRUDCore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRUDCore.Helpers
{
    [ViewComponent(Name = "CategoryList")]
    public class CategoryHelper : ViewComponent
    {
        private readonly TaskManagementDbContext _context;
        public CategoryHelper(TaskManagementDbContext context)
        {
            _context = context;
        }
        public async Task<List<CategoryTask>> GetAll()
        {
            var categoryTasks = await _context.CategoryTasks.ToListAsync();
            return categoryTasks;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var categoryTasks = await _context.CategoryTasks.ToListAsync();
            return View("_CategoriesPartial", categoryTasks);
        }
    }
}
