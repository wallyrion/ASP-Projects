using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Xml.Schema;
using Eshop.Domain.Entities.Goods;

namespace Eshop.Domain.Entities
{

    public class User
    {
        public int Id { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public string Login { get; set; }
        public string Role { get; set; } // "User", "Moderator" or "Admin"
        public virtual ICollection<Order> Orders {get; set; }
        public Cart Cart { get; set; }
        public int Status { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
        public User()
        {
            Status = 1;
            Orders = new List<Order>();
            Reviews = new List<Review>();
        }
    }
}
