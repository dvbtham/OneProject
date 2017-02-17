using CRUDCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRUDCore.Data
{
    public static class DbInitializer
    {
        public static void Initialize(SchoolContext context)
        {
            context.Database.EnsureCreated();

            if (context.CategoryTasks.Any())
            {
                return;   // DB has been seeded
            }

            var categoryTasks = new CategoryTask[]
            {
            new CategoryTask{Title="Category Task : Title 1",Description = "Description of Category Task : Title 1",IsActived = true},
            new CategoryTask{Title="Category Task : Title 2",Description = "Description of Category Task : Title 2",IsActived = true},
            new CategoryTask{Title="Category Task : Title 3",Description = "Description of Category Task : Title 3",IsActived = true},
            new CategoryTask{Title="Category Task : Title 4",Description = "Description of Category Task : Title 4",IsActived = true} };
            foreach (var task in categoryTasks)
            {
                context.CategoryTasks.Add(task);
            }
            context.SaveChanges();

            var tasks = new Tasks[]
            {
            new Tasks{Title="Task : Title 1", IdCategoryTask=1, Description="Description of Task : Title 1", FromDate=Convert.ToDateTime("14/02/2017"), DeadlineDate=Convert.ToDateTime("25/02/2017"),UnitPer=10,IsActive=true},
            new Tasks{Title="Task : Title 2", IdCategoryTask=2, Description="Description of Task : Title 2", FromDate=Convert.ToDateTime("26/02/2017"), DeadlineDate=Convert.ToDateTime("20/03/2017"),UnitPer=5,IsActive=true},
            new Tasks{Title="Task : Title 3", IdCategoryTask=3, Description="Description of Task : Title 3", FromDate=Convert.ToDateTime("22/03/2017"), DeadlineDate=Convert.ToDateTime("05/04/2017"),UnitPer=20,IsActive=true},
             new Tasks{Title="Task : Title 3", IdCategoryTask=4, Description="Description of Task : Title 3", FromDate=Convert.ToDateTime("06/04/2017"), DeadlineDate=Convert.ToDateTime("05/05/2017"),UnitPer=20,IsActive=true}
            };
            foreach (var task in tasks)
            {
                context.Tasks.Add(task);
            }
            context.SaveChanges();
        }
    }
}
