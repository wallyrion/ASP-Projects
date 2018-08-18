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
    public class MotherboardRepository : IRepository<Motherboard>
    {
        private ShopContext db;

        public MotherboardRepository(ShopContext context)
        {
            this.db = context;
        }

        public IEnumerable<Motherboard> GetAll()
        {
            return db.Motherboards.ToList();
        }

        public Motherboard Get(int id)
        {
            return db.Motherboards.Find(id);
        }

        public void Create(Motherboard mb)
        {
            db.Motherboards.Add(mb);
        }

        public void Update(Motherboard mb)
        {
            db.Entry(mb).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            Monitor monitor = db.Monitors.Find(id);
            if (monitor != null)
                db.Monitors.Remove(monitor);
        }
    }
}
