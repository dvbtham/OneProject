using CRUDCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRUDCore.Data
{
    public static class DbInitializer
    {
        public static void Initialize(TaskManagementDbContext context)
        {
            context.Database.EnsureCreated();

            if (context.CategoryTasks.Any())
            {
                return;
            }
            var categoryTasks = new List<CategoryTask>();
            for (int i = 1; i <= 20; i++)
            {
                var categoryTask = new CategoryTask();
                categoryTask.Title = "Category Task : Title " + i;
                categoryTask.Description = "Description of Category Task : Title " + i;
                var isOdd = i % 2 == 0 ? true : false;
                categoryTask.IsActived = isOdd;
                context.CategoryTasks.Add(categoryTask);
            }
            context.SaveChanges();

            var tasks = new List<Tasks>();
            for (int i = 1; i <= 20; i++)
            {
                var task = new Tasks();
                task.Title = "Task : Title " + i;
                task.IdCategoryTask = i;
                task.Description = "Description of Task : Title " + i;
                task.FromDate = DateTime.Now.AddDays(i);
                task.DeadlineDate = DateTime.Now.AddDays(i + 1);
                task.UnitPer = i;
                var isOdd = i % 2 == 0 ? true : false;
                task.IsActived = isOdd;
                context.Tasks.Add(task);
            }
            context.SaveChanges();
        }
    }
}
