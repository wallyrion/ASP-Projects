using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using Eshop.Domain.Abstract;
using Eshop.Domain.Concrete;
using Eshop.Domain.Entities.Goods;

namespace Eshop.Domain.Repositories
{
    public class CategoryRepository : IRepository<Category>
    {
        private ShopContext db;

        public CategoryRepository(ShopContext context)
        {
            this.db = context;
        }

        public IEnumerable<Category> GetAll()
        {
            return db.Categories.Include(c => c.Properties.Select(prop => prop.Specifications)).AsEnumerable();
        }

        public List<string> GetAllNames()
        {
            return db.Categories.Select(cat => cat.Name).ToList();
        }
        public Category Get(int id)
        {
            return db.Categories.Find(id);
        }

        public Category Get(string name)
        {
            return db.Categories.FirstOrDefault(category => category.Name == name);
        }
        public void Create(Category category)
        {
            if (db.Categories.FirstOrDefault(c => c.Name == category.Name) == null)
                db.Categories.Add(category);
        }

        public void Update(Category category)
        {
            db.Entry(category).State = EntityState.Modified;
        }

        public Category GetByName(string Name)
        {
            return db.Categories.FirstOrDefault(categ => categ.Name == Name);
        }
        public void Delete(int id)
        {
            //bad code, some issues with cascade on delete (couldn't fix it)
            var goods = db.Goods.Where(g => g.Category.Id == id).ToList();
            foreach (var categoryGood in goods)
            {
                db.Goods.Remove(categoryGood);
                db.SaveChanges();
            }
            var categ = db.Categories.Include(cat => cat.Goods).FirstOrDefault(d => d.Id == id);
            if (categ != null) db.Categories.Remove(categ);
        }
    }
}
