using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using Eshop.Domain.Abstract;
using Eshop.Domain.Concrete;
using Eshop.Domain.Entities.Goods;
using Eshop.Domain.Entities.Helpers;

namespace Eshop.Domain.Repositories
{

    public class GoodRepository : IRepository<Good>
    {

        private ShopContext db;

        public GoodRepository(ShopContext context)
        {
            this.db = context;
        }

        public List<Good> GetAllByCategoryName(string categoryName)
        {
            if (String.IsNullOrEmpty(categoryName))
            {
                return db.Goods.ToList();
            }

            return db.Categories.Include(cat => cat.Goods).FirstOrDefault(cat => cat.Name == categoryName)
                ?.Goods
                .ToList();
        }
        public IEnumerable<Good> GetByIds(IEnumerable<int> ids)
        {
            if (ids == null) return new List<Good>();
            return db.Goods.Include(g => g.Specifications.Select(s => s.Property)).Where(g => ids.Contains(g.Id)).ToList();
        }

        public List<Good> GetBySpecifications(int[] specificatioIds, string category)
        {
            if (specificatioIds == null)
            {
                if (String.IsNullOrEmpty(category))
                {
                    return db.Goods.ToList();
                }

                return GetAllByCategoryName(category);
            }

            var goods = db.Goods.Include(g => g.Specifications).ToList();

            var filter = db.Specifications.Where(s => specificatioIds.Contains(s.Id)).GroupBy(s => s.Property).Select(
                s =>
                    new PropertyHelper
                    {
                        Name = s.Key.Name,
                        Specifications = s.Key.Specifications.Where(sp => specificatioIds.Contains(sp.Id)).ToList()
                    }).ToList();


            var goods2 = db.Goods.AsEnumerable().Where(g => filter.All(f => g.Specifications.Any(s => f.Specifications.Contains(s))))
                .ToList();

            return goods2;

        }
        public IEnumerable<Good> GetAll()
        {
            return db.Goods.Include(g => g.Specifications).ToList();
        }

        public Good Get(int id)
        {
            return db.Goods.Find(id);

        }
        public Good GetWithInclude(int id)
        {
            var reviews = db.Reviews.Select(r=>r).ToList();
            /*var good = db.Goods.Select(g=>g).FirstOrDefault(g => g.Id == id);
            return */
            var good = db.Goods.Include(g => g.Specifications.Select(y => y.Property)).Include(g=>g.Reviews.Select(r=>r.User)).Include(g => g.Category).FirstOrDefault(g => g.Id == id);
            return good;
        }

        public void Create(Good good)
        {
            db.Goods.Add(good);
        }

        public void Create(Good good, string categoryName, IEnumerable<int> specifications )
        {
            var g = db.Goods.Find(good.Id);
            if (g != null || String.IsNullOrEmpty(categoryName)) return;

            var category = db.Categories.FirstOrDefault(cat => cat.Name == categoryName);
            if (category == null) return;
            good.Category = category;

            if (specifications != null)
            {
                foreach (var spec in specifications)
                {
                    var s = db.Specifications.Find(spec);
                    if (s != null)
                    {
                        good.Specifications.Add(s);
                    }
                }
            }
            

            db.Goods.Add(good);
            
        }
        public void Update(Good good)
        {
            db.Entry(good).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            Good good = db.Goods.Find(id);
            if (good != null)
                db.Goods.Remove(good);
        }

        private void AddSpecification(int goodId, int specId)
        {
        }
        
        public void Save(Good editedGood, Dictionary<string, int> goodSpecifications)
        {
            var good = db.Goods.Include(p => p.Specifications).Include(g => g.Category).FirstOrDefault(p => p.Id == editedGood.Id);

            if (good ==null)
            {
                good = new Good();
                db.Goods.Add(good);
            }
            db.Entry(good).State = EntityState.Modified;

            good.Name = editedGood.Name;
            good.Price = editedGood.Price;
            if (editedGood.ImageData != null && editedGood.ImageMimeType != null)
            {
                good.ImageData = editedGood.ImageData;
                good.ImageMimeType = editedGood.ImageMimeType;
            }
            

            good.Specifications.Clear();
            foreach (var spec in goodSpecifications)
            {
                good.Specifications.Add(db.Specifications.Find(spec.Value));
            }

        }
    }
}
