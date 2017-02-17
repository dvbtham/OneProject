using CRUDCore.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRUDCore.Data
{
    public class TaskManagementDbContext : DbContext
    {
        public TaskManagementDbContext(DbContextOptions<TaskManagementDbContext> options) : base(options)
        {
        }

        public DbSet<CategoryTask> CategoryTasks { get; set; }
        public DbSet<Tasks> Tasks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Tasks>().Ignore(x => x.CategoryTasks);
            modelBuilder.Entity<CategoryTask>().ToTable("CategoryTasks");
            modelBuilder.Entity<Tasks>().ToTable("Tasks");
            modelBuilder.Entity<CategoryTask>().HasMany(x => x.Tasks).WithOne(c => c.CategoryTask);
        }
    }
}
