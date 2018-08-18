using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Eshop.Domain.Abstract;
using Eshop.Domain.Concrete;
using Eshop.Domain.Entities;

namespace Eshop.Domain.Repositories
{
    public class OrderRepository : IRepository<Order>
    {
        private ShopContext db;
 
        public OrderRepository(ShopContext context)
        {
            this.db = context;
        }
 
        public IEnumerable<Order> GetAll()
        {
            return db.Orders.Include(o=>o.User).Include(o=>o.GoodsQuantities).ToList();
        }

        public IEnumerable<Order> GetByUserLogin(string login)
        {
            return (db.Users.Include(u => u.Orders.Select(o=>o.GoodsQuantities)).FirstOrDefault(u => u.Login == login))?.Orders.AsEnumerable();
        }
        public Order Get(int id)
        {
            return db.Orders.Find(id);
        }
 
        public void Create(Order order)
        {
            var or1 = db.Orders.ToList();
            /*var us = db.Users.Select(user=>user).FirstOrDefault(u=>u.Id==order.User.Id);*/
            db.Orders.Add(order);
            /*if (us != null)
            {
                us.Orders.Add(new Order());
                db.Entry(us).State = EntityState.Modified;
            }*/

            db.SaveChanges();
            var or2 = db.Orders.Include(o=>o.GoodsQuantities).ToList();
            /*var o = db.Orders.First();*/
            /*db.Orders.Remove(o);*/
            /*db.Orders.Add(order);*/
            /*db.Orders.Add(order);*/



        }
 
        public void Update(Order order)
        {
            db.Entry(order).State = EntityState.Modified;
        }
 
        public void Delete(int id)
        {
            Order order = db.Orders.Find(id);
            if (order != null)
                db.Orders.Remove(order);
        }
    }
}
