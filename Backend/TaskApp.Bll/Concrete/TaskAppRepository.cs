using Microsoft.EntityFrameworkCore;
using TaskApp.Bll.Abstract;
using TaskApp.Dal.DataContext;
using Task = TaskApp.Entity.Concrete.Task;

namespace TaskApp.Bll.Concrete
{

    public class TaskAppRepository : ITaskAppService
    {
        private readonly TaskAppDbContext _context;

        public TaskAppRepository(TaskAppDbContext context)
        {
            _context = context;
        }

        public Task? GetById(int id)
        {
            Task? task = _context.Tasks
                .AsNoTracking()
                .FirstOrDefault(x => x.Id == id);
            return task;
        }

        public IEnumerable<Task> GetAll()
        {
            IEnumerable<Task> taskList = _context.Tasks
                .OrderByDescending(x => x.Id)
                .ToList();
            return taskList;
        }

        public bool Add(Task entity)
        {
            if (entity is not null)
            {
                _context.Tasks.Add(entity);
                _context.SaveChanges();

                return true;
            }

            else
                return false;
        }

        public bool Update(Task entity)
        {
            var isTaskExists = GetById(entity.Id);

            if (isTaskExists is not null)
            {
                _context.Tasks.Update(entity);
                _context.SaveChangesAsync();

                return true;
            }

            else
                return false;
        }

        public bool Delete(int id)
        {
            var isTaskExists = GetById(id);

            if (isTaskExists is not null)
            {
                _context.Tasks.Remove(isTaskExists);
                _context.SaveChanges();

                return true;
            }

            else
                return false;
        }

    }

}
