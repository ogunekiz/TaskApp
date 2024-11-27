using Microsoft.EntityFrameworkCore;
using Task = TaskApp.Entity.Concrete.Task;

namespace TaskApp.Dal.DataContext
{
    public class TaskAppDbContext : DbContext
    {
        public TaskAppDbContext(DbContextOptions<TaskAppDbContext> options) : base(options)
        {
        }

        public DbSet<Task> Tasks { get; set; }
    }
}
