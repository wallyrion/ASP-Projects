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
    public class PropertyRepository : IRepository<Property>
    {
        private ShopContext db;

        public PropertyRepository(ShopContext context)
        {
            this.db = context;
        }


        public IEnumerable<Property> GetAll()
        {
            return db.Properties.Include(prop=>prop.Specifications).ToList();
        }
        public List<Property> GetAllByCategoryName(string categoryName)
        {
            return db.Properties.Include(prop=>prop.Specifications).Where(prop => prop.Category.Name == categoryName).ToList();
        }
        public Property Get(int id)
        {
            return db.Properties.Include(prop=>prop.Specifications).FirstOrDefault(prop=>prop.Id==id);
        }

        public void Create(Property property)
        {
            if (db.Properties.FirstOrDefault(p => p.Name == property.Name && p.Category.Name==property.Category.Name) == null)
            {
                db.Properties.Add(property);
            }
               
        }

        public Property GetByName(string name)
        {
            return db.Properties.Include(spec=>spec.Category).FirstOrDefault(prop => prop.Name == name);
        }
        public void Update(Property property)
        {
            db.Entry(property).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            Property property = db.Properties.Find(id);
            if (property != null)
                db.Properties.Remove(property);
        }
    }
}
