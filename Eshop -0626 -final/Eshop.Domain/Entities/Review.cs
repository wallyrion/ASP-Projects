using System;
using System.Collections.Generic;
using System.Text;
using Eshop.Domain.Entities.Goods;

namespace Eshop.Domain.Entities
{
    public class Review
    {
        public int Id { get; set; }
        public int Rate { get; set; }
        public string Comment { get; set; }
        public DateTime Date { get; set; }

        public Good Good { get; set; }
        public User User { get; set; }

        public Review()
        {
            Date = DateTime.UtcNow;
        }
    }
}
