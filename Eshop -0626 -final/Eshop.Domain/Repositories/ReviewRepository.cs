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
    public class ReviewRepository
    {
        private readonly ShopContext db;

        public ReviewRepository(ShopContext context)
        {
            this.db = context;
        }

        public IEnumerable<Review> GetAll()
        {
            return db.Reviews.ToList();
        }

        public Review Get(int id)
        {
            return db.Reviews.Find(id);
        }

        public void Create(Review review)
        {
            db.Reviews.Add(review);
        }

        public void Update(Review review)
        {
            db.Entry(review).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            Review review = db.Reviews.Find(id);
            if (review != null)
                db.Reviews.Remove(review);
        }


    }
}
