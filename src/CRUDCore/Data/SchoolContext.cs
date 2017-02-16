using CRUDCore.Models;
using Microsoft.EntityFrameworkCore;

namespace CRUDCore.Data
{
    public class SchoolContext : DbContext
    {
        public SchoolContext(DbContextOptions<SchoolContext> options) : base(options)
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