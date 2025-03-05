using Context;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly APIContext context;

        public ProductRepository(APIContext context)
        {
            this.context = context;
        }

        public void Delete(int id) => context.Products.Remove(GetById(id));

        public IEnumerable<Product> GetAll() => context.Products.Include(e=>e.Category).ToList();

        public Product? GetById(int id) => context.Products.Include(e => e.Category).FirstOrDefault(e=>e.ID==id);

        public void Insert(Product entity) => context.Products.Add(entity);

        public void Save() => context.SaveChanges();

        public void Update(Product entity) => context.Products.Update(entity);
    }
}
