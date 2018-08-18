using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Eshop.Models
{
    public class GoodsHelperPaging
    {
        public IEnumerable<GoodView> Goods { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }

        public GoodsHelperPaging()
        {
            Goods = new List<GoodView>();
        }

        public GoodsHelperPaging(IEnumerable<GoodView> goods, int currentPage, int totalPages)
        {
            Goods = goods;
            CurrentPage = currentPage;
            TotalPages = totalPages;
        }
    }
}