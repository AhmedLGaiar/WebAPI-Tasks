using Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly APIContext context;

        public CategoryRepository(APIContext context)
        {
            this.context = context;
        }

        public IEnumerable<Category> GetAll() => context.Categories.ToList();

        public Category? GetById(int id) => context.Categories.Find(id);

        public void Delete(int id) => context.Categories.Remove(GetById(id));

        public void Insert(Category entity) => context.Categories.Add(entity);

        public void Update(Category entity)=>context.Categories.Update(entity);
 
        public void Save() => context.SaveChanges();

    }
}
