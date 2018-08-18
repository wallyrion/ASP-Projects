using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using Eshop.Domain.Concrete;
using Eshop.Domain.Entities;
using Eshop.Domain.Entities.Goods;

namespace Eshop.Domain.Repositories
{
    public class UserRepository
    {
        private ShopContext db;

        public UserRepository(ShopContext context)
        {
            this.db = context;
        }

        public User GetByLoginPassword(string login, string password)
        {
            return db.Users.FirstOrDefault(user => user.Login == login && user.Password == password);
        }

        public User GetByLogin(string login)
        {
            var a = db.Users.FirstOrDefault(user => user.Login == login);
            return a;
        }

        public void RemoveGoodFromCart(string login, int id)
        {
            var good = db.Goods.Find(id);
            if (good == null) return;

            var cart = db.Carts.Include(c => c.Goods).FirstOrDefault(c => c.User.Login == login && c.Goods.Any(g => g.Id == good.Id));
            if (cart == null) return;


            cart.Goods.Remove(good);
            db.Entry(cart).State = EntityState.Modified;
            db.SaveChanges();
        }
        public List<Good> GetGoodsInCart(string userLogin)
        {
            return db.Carts.Include(c => c.Goods.Select(g => g.Specifications.Select(s => s.Property))).FirstOrDefault(c => c.User.Login == userLogin)?.Goods.ToList();
        }
        public void AddGoodToCart(string userLogin, int id)
        {
            var good = db.Goods.Include(g => g.Category).Include(g => g.Specifications).Include(g => g.Carts).Include(g => g.Category.Goods).FirstOrDefault(g => g.Id == id);
            if (good != null)
            {

                var cart = db.Carts.Include(c => c.Goods).FirstOrDefault(c => c.User.Login == userLogin);
                if (cart == null)
                {
                    cart = new Cart
                    {
                        User = db.Users.FirstOrDefault(u => u.Login == userLogin)
                    };
                    db.Carts.Add(cart);
                    db.SaveChanges();
                }


                cart.Goods.Add(good);

                db.Entry(cart).State = EntityState.Modified;
                db.SaveChanges();


                var categ = db.Categories.ToList();
            }
        }

        public Cart GetUserCart(string login)
        {
            return db.Users.Include(u => u.Cart).Include(u => u.Cart.Goods).FirstOrDefault(user => user.Login == login)
                ?.Cart;
        }
        public IEnumerable<User> GetAll()
        {
            return db.Users.Include(u => u.Orders).ToList();
        }

        public User Get(int id)
        {
            return db.Users.Find(id);
        }

        public void Create(User user)
        {
            db.Users.Add(user);
        }

        public void Update(User user)
        {
            db.Entry(user).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            User user = db.Users.Include(u => u.Reviews).Include(u => u.Cart).FirstOrDefault(u => u.Id == id);

            if (user != null && user.Role!="Admin")
            {
                if (user.Cart != null)
                {
                    user.Cart.Goods.Clear();
                }

                foreach (var review in user.Reviews)
                {
                    review.User = null;
                }

                user = db.Users.Find(id);
                db.Users.Remove(user);
            }

        }

        public void PromoteUser(int id)
        {
            var user = db.Users.Find(id);
            if (user == null) return;
            if (user.Role == "User")
                user.Role = "Moderator";

            db.Entry(user).State = EntityState.Modified;
            db.SaveChanges();
        }
        public void DemodeUser(int id)
        {
            var user = db.Users.Find(id);
            if (user == null) return;
            if (user.Role == "Moderator")
                user.Role = "User";

            db.Entry(user).State = EntityState.Modified;
            db.SaveChanges();
        }
        public void RemoveAllGoodsFromCart(int userId)
        {
            var cart = db.Users.Include(u => u.Cart.Goods).FirstOrDefault(u => u.Id == userId)?.Cart;
            db.Carts.Remove(cart);

            db.SaveChanges();
        }
        public void UnBlockUser(int id)
        {
            var user = db.Users.Find(id);
            if (user == null) return;

            user.Status = 1;
            db.Entry(user).State = EntityState.Modified;
            db.SaveChanges();
        }
        public void BlockUser(int id)
        {
            var user = db.Users.Find(id);
            if (user == null  || user.Role=="Admin") return;
            user.Status = 0;
            db.Entry(user).State = EntityState.Modified;
            db.SaveChanges();
        }
    }
}
