using System;
using System.Collections.Generic;
using System.Text;

namespace Eshop.Domain.Entities.Goods
{
    public class Good
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public string Description { get; set; }
        public Category Category { get; set; }
        public DateTime Date { get; set; }
        public virtual ICollection<Specification> Specifications { get; set; }
        public virtual ICollection<Cart> Carts { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }

        public byte[] ImageData { get; set; }
        public string ImageMimeType{get; set; }


        public Good()
        {
            Date = DateTime.UtcNow;
            Carts = new List<Cart>();
            Specifications = new List<Specification>();
/*
            Category = new Category();
*/
        }


        public bool IfFitsMinMax(int? min, int? max)
        {
            
            if (min == null && max == null)
            {
                return true;
            }
            if (max == null && Price >= min)
            {
                return true;
            }
            else if (min == null && Price<=max)
            {
                return true;
            }
            else if (min != null && max != null && Price >= min && Price <= max)
            {
                return true;
            }

            return false;
        }
    }
}
