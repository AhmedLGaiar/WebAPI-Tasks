namespace Repository
{
    public interface IRepository<T> where T : class
    {
        public IEnumerable<T> GetAll();
        public T GetById(int id);
        public void Insert(T entity);
        public void Update(T entity);
        public void Delete(int id);
        public void Save();

    }
}
