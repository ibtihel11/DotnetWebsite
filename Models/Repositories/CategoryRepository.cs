using System.Collections.Generic;
using System.Linq;

namespace Website.Models.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly AppDbContext context;

        public CategoryRepository(AppDbContext context)
        {
            this.context = context;
        }

        public IList<Category> GetAll()
        {
            return context.Categories.OrderBy(x => x.CategoryName).ToList();
        }

        public Category GetById(int id)
        {
            return context.Categories.Find(id);
        }

        public void Add(Category c)
        {
            context.Categories.Add(c);
            context.SaveChanges();
        }

        public Category Update(Category c)
        {
            context.Categories.Update(c);
            context.SaveChanges();
            return c;
        }

        public void Delete(int id)
        {
            var c = context.Categories.Find(id);
            if (c != null)
            {
                context.Categories.Remove(c);
                context.SaveChanges();
            }
        }
    }
}