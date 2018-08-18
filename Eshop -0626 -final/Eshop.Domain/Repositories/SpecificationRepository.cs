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
    public class SpecificationRepository : IRepository<Specification>
    {
        private ShopContext db;

        public SpecificationRepository(ShopContext context)
        {
            this.db = context;
        }

        public IEnumerable<Specification> GetAll()
        {
            return db.Specifications.Include(spec=>spec.Property).ToList();
        }

        public Specification Get(int id)
        {
            return db.Specifications.Include(spec=>spec.Property).FirstOrDefault(spec=>spec.Id==id);
        }

        public void Create(Specification specification)
        {
            if (!db.Specifications.Any(s=>s.Property.Id==specification.Property.Id && s.Name==specification.Name))
              db.Specifications.Add(specification);
        }

        public void Update(Specification specification)
        {
            db.Entry(specification).State = EntityState.Modified;
        }

        public Specification GetByName(string name)
        {
            return db.Specifications.FirstOrDefault(spec => spec.Name == name);
        }
        public void Delete(int id)
        {
            Specification specification = db.Specifications.Find(id);
            if (specification != null)
                db.Specifications.Remove(specification);
        }
    }
}
