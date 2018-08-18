using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using Eshop.Domain.Abstract;
using Eshop.Domain.Concrete;
using Eshop.Domain.Entities;
using Eshop.Domain.Entities.Goods;

namespace Eshop.Domain.Repositories
{
    public class MonitorRepository : IRepository<Monitor>
    {
        private ShopContext db;

        public MonitorRepository(ShopContext context)
        {
            this.db = context;
        }

        public IEnumerable<Monitor> GetAll()
        {
            return db.Monitors.ToList();
        }

        public Monitor Get(int id)
        {
            return db.Monitors.Find(id);
        }

        public void Create(Monitor monitor)
        {
            db.Monitors.Add(monitor);
        }

        public void Update(Monitor monitor)
        {
            db.Entry(monitor).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            Monitor monitor = db.Monitors.Find(id);
            if (monitor != null)
                db.Monitors.Remove(monitor);
        }
    }
}
