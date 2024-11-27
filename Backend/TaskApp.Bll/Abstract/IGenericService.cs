namespace TaskApp.Bll.Abstract
{
    public interface IGenericService<T> where T : class
    {
        IEnumerable<T> GetAll();
        T? GetById(int id);
        bool Add(T entity);
        bool Update(T entity);
        bool Delete(int id);
    }
}
