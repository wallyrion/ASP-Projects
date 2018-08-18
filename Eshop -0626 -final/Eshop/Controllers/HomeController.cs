using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Eshop.Domain.Concrete;
using Eshop.Domain.Entities;
using Eshop.Domain.Entities.Goods;
using Eshop.Domain.Entities.Helpers;
using Eshop.Models;
using NLog;
using WebGrease.Css.Extensions;

namespace Eshop.Controllers
{
    public class HomeController : Controller
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly UnitOfWork _unitOfWork;

        public HomeController()
        {
            _unitOfWork = new UnitOfWork();
            ViewData["AllCategories"] = _unitOfWork.Categories.GetAllNames();
        }
        /// <summary>
        /// TEST
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            try
            {
                throw new NotImplementedException();
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Exception occured in index");
                  return Redirect("page500.aspx");

            }
        }
        /// <summary>
        /// Get goods from selected category
        /// </summary>
        /// <param name="categoryName">if null or empty, return all goods from all categories</param>
        /// <returns></returns>
        public ActionResult Category(string categoryName)
        {
            try
            {
                Logger.Info("Get goods from category: " + (categoryName ?? "All"));

                var viewModel = new IndexViewModel
                {
                    CategoryName = categoryName,
                    PageSize = 10,
                    Properties = _unitOfWork.Properties.GetAllByCategoryName(categoryName)
                };
                ViewBag.Title = categoryName ?? "All";
                return View(viewModel);

            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                return RedirectToAction("Category");
            }
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        /// <summary>
        /// sort list of goods
        /// </summary>
        /// <param name="goods">List of Goods</param>
        /// <param name="sortType">Sort Type</param>
        /// <returns>sorted list of goods</returns>
        private List<Good> SortGoods(List<Good> goods, string sortType)
        {
            try
            {
                if (sortType == "By Name Ascending")
                    goods = goods.OrderBy(g => g.Name).ToList();
                else if (sortType == "By Name Descending")
                    goods = goods.OrderByDescending(g => g.Name).ToList();
                else if (sortType == "By Price Ascending")
                    goods = goods.OrderBy(g => g.Price).ToList();
                else if (sortType == "By Price Descending")
                    goods = goods.OrderByDescending(g => g.Price).ToList();
                else if (sortType == "By Date Ascending")
                    goods = goods.OrderBy(g => g.Date).ToList();
                else
                    goods = goods.OrderByDescending(g => g.Date).ToList();

                Logger.Info("Sort goods success");

            }
            catch (Exception ex)
            {
                Logger.Error(ex,"Error occurred in SortGoods");
            }

            return goods;
        }
        /// <summary>
        /// apply filter rools for goods
        /// </summary>
        /// <param name="minPrice"></param>
        /// <param name="maxPrice"></param>
        /// <param name="specificationIds">specification ids that are involved into filted</param>
        /// <param name="category"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="sortType"></param>
        /// <returns></returns>
        public JsonResult ApplyFilter(int? minPrice, int? maxPrice, int[] specificationIds, string category, int page = 1, int pageSize = 3, string sortType = "By Price Ascending")
        {
            try
            {
                Logger.Info("Try to apply filters");
                var dbGoods = _unitOfWork.Goods.GetBySpecifications(specificationIds, category)
                    .Where(g => g.IfFitsMinMax(minPrice, maxPrice)).ToList();

                dbGoods = SortGoods(dbGoods, sortType);
                IEnumerable<Good> goodsPerPages = dbGoods.Skip((page - 1) * pageSize).Take(pageSize);

                var goodsFilter = new List<GoodView>();
                goodsPerPages.ForEach(g => goodsFilter.Add(new GoodView(g)));

                var goodHelper = new GoodsHelperPaging(goodsFilter, page, TotalPages(dbGoods.Count, pageSize));
                Logger.Info("Filters applied successfully");
                return Json(goodHelper, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
            }

            return null;

        }
        /// <summary>
        /// Add good to the cart
        /// </summary>
        /// <param name="goodId">good id to be added to cart</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AddToCart(int? goodId)
        {
            try
            {
                if (goodId == null) return null;
                if (Request.IsAuthenticated)
                {
                    _unitOfWork.Users.AddGoodToCart(HttpContext.User.Identity.Name, goodId.Value);
                    return Json("success", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var cookie = AddToCookie(Request.Cookies["GoodsIds"], goodId.Value);

                    System.Web.HttpContext.Current.Response.Cookies.Add(cookie);
                }

                return Json(goodId, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Logger.Error(ex,"");
                return null;
            }
            
        }
        /// <summary>
        /// add good id to cookie for unauthorized user
        /// </summary>
        /// <param name="cookieGoodsIds">cookie that contain array of goods' ids</param>
        /// <param name="id">good's id that need to be added in cart (cookie)</param>
        /// <returns></returns>
        private HttpCookie AddToCookie(HttpCookie cookieGoodsIds, int id)
        {
            if (cookieGoodsIds == null)
            {
                cookieGoodsIds = new HttpCookie("GoodsIds") { Expires = DateTime.Now.AddMinutes(1000) };
            }

            var ids = cookieGoodsIds.Values.GetValues("id");
            if (ids == null || !ids.Contains(id.ToString()))
                cookieGoodsIds.Values.Add("id", id.ToString());

            return cookieGoodsIds;
        }
        
        /// <summary>
        /// Remove good from cart
        /// </summary>
        /// <param name="id"></param>
        public ActionResult RemoveGoodFromCart(int id)
        {
            if (Request.IsAuthenticated)
            {
                _unitOfWork.Users.RemoveGoodFromCart(HttpContext.User.Identity.Name, id); //if user is authenticated remove food from his cart
            }
            else
            { // remove good from cookies
                var cookieGoodsIds = Request.Cookies["GoodsIds"];
                var idsStrings = cookieGoodsIds?.Values.GetValues("id");
                if (idsStrings != null)
                {
                    var idStr = id.ToString();
                    var resArray = idsStrings.Where(str => str != idStr).ToArray();
                    cookieGoodsIds.Values.Clear();
                    resArray.ForEach(idGood => cookieGoodsIds.Values.Add("id", idGood));

                    Response.SetCookie(cookieGoodsIds);
                }

            }
            return RedirectToAction("Cart");
        }
        /// <summary>
        /// Show list of goods in cart
        /// </summary>
        public ActionResult Cart()
        {
            List<Good> goods = new List<Good>();
            if (Request.IsAuthenticated)
            {
                goods = _unitOfWork.Users.GetGoodsInCart(HttpContext.User.Identity.Name); // If user logged in, get goods from his cart
            }
            else
            { //get goods' ids from cookies
                var cookieGoodsIds = Request.Cookies["GoodsIds"];
                var idsStrings = cookieGoodsIds?.Values.GetValues("id");
                if (idsStrings != null)
                {
                    foreach (var id in idsStrings)
                    {
                        var g = _unitOfWork.Goods.GetWithInclude(Convert.ToInt32(id));
                        if (g!=null)
                            goods.Add(g);
                    }
                }
            }
            return View(goods);

        }

        /// <summary>
        /// Get all orders of authorized user
        /// </summary>
        [Authorize]
        public ActionResult Orders()
        {
            var orders = _unitOfWork.Orders.GetByUserLogin(HttpContext.User.Identity.Name).Reverse();
            return View(orders);
        }

        /// <summary>
        /// Create order with goods from the cart
        /// </summary>
        /// <param name="goodQuantity"></param>
        [Authorize]
        [HttpPost]
        public ActionResult Cart(IEnumerable<GoodQuantityView> goodQuantity)
        {
            var user = _unitOfWork.Users.GetByLogin(HttpContext.User.Identity.Name);
            var order = new Order {User = user};
            foreach (var pair in goodQuantity)
            {
                var good = _unitOfWork.Goods.GetWithInclude(pair.GoodId);
                if (good != null)
                {
                    var gq = new GoodQuantity()
                    {
                        
                        PurchasedGoodId = good.Id,
                        GoodName = good.Name,
                        TotalPrice = good.Price *pair.Quantity,
                        GoodCount = pair.Quantity
                    };
                    order.GoodsQuantities.Add(gq);
                }
            }
            _unitOfWork.Orders.Create(order);
            _unitOfWork.Users.RemoveAllGoodsFromCart(user.Id);

          
            return RedirectToAction("Orders");
        }

        private int TotalPages(int totalItems, int pageSize) => (int)Math.Ceiling((decimal)totalItems / pageSize);

    }
}