using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using Eshop.Domain.Concrete;
using Eshop.Domain.Entities;
using Eshop.Domain.Entities.Goods;
using Eshop.Models;
using NLog;
using WebGrease.Css.Extensions;

namespace Eshop.Controllers
{
    public class GoodController : Controller
    {

        private readonly UnitOfWork _unitOfWork;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public GoodController()
        {
            _unitOfWork = new UnitOfWork();
            ViewData["AllCategories"] = _unitOfWork.Categories.GetAllNames();
        }

        /// <summary>
        /// Get good's image from DB
        /// </summary>
        /// <param name="id">good id</param>
        /// <returns>return Image</returns>
        public FileContentResult GetImage(int id)
        {
            Good good = _unitOfWork.Goods.Get(id);

            if (good.ImageData != null && good.ImageMimeType !=null)
            {
                return File(good.ImageData, good.ImageMimeType);
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// Get good for editing
        /// </summary>
        /// <param name="id">good's id</param>
        /// <returns></returns>
        [Authorize(Roles = "Admin,Moderator")]
        public ActionResult Edit(int? id)
        {
            Good g = null;
            if(id.HasValue)
                g= _unitOfWork.Goods.GetWithInclude(id.Value);

            if (g==null) g=new Good();
            var goodEditView = new GoodEditView(g, _unitOfWork.Properties.GetAllByCategoryName(g.Category.Name));


            var categories = _unitOfWork.Categories.GetAllNames();
            ViewBag.categories = categories;

            
            return View(goodEditView);
        }

        /// <summary>
        /// Update data of edited good
        /// </summary>
        /// <param name="good">selected good</param>
        /// <param name="goodSpecifications">selected specifications</param>
        /// <param name="image">selected image</param>
        /// <returns></returns>
        [Authorize(Roles = "Admin,Moderator")]
        [HttpPost]
        public ActionResult Edit(Good good, Dictionary<string,int>goodSpecifications, HttpPostedFileBase image = null)
        {
            try
            {


                if (image != null)
                {
                    good.ImageMimeType = image.ContentType;
                    good.ImageData = new byte[image.ContentLength];
                    image.InputStream.Read(good.ImageData, 0, image.ContentLength);
                }


                _unitOfWork.Goods.Save(good, goodSpecifications);
                _unitOfWork.Save();
                Logger.Info($"Good id = {good.Id} was changed");

                return RedirectToAction("Edit", new {id = good.Id});
            }
            catch (Exception ex)
            {
                Logger.Error(ex,"Error while trying to save good");
              

            }
            return RedirectToAction("Category", "Home");
        }
        
        /// <summary>
        /// Create new good
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Admin,Moderator")]
        public ActionResult Create()
        {
            var g = new Good();
            return View(g);
        }

        /// <summary>
        /// Save created good 
        /// </summary>
        /// <param name="newGood">New good</param>
        /// <param name="categoryName">selected category</param>
        /// <param name="specificationIds">selected Specifications</param>
        /// <param name="image">selected image</param>
        /// <returns></returns>
        [Authorize(Roles = "Admin,Moderator")]
        [HttpPost]
        public ActionResult Create(Good newGood,string categoryName, IEnumerable<int> specificationIds, HttpPostedFileBase image = null)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    if (_unitOfWork.Categories.Get(categoryName) == null)
                    {
                        ModelState.AddModelError("", "You must choose category");
                        return View(newGood);

                    }

                    if (newGood.Price <= 0)
                    {
                        ModelState.AddModelError("", "Price must be more than 0");
                        return View(newGood);
                    }

                    if (image != null)
                    {
                        newGood.ImageMimeType = image.ContentType;
                        newGood.ImageData = new byte[image.ContentLength];
                        image.InputStream.Read(newGood.ImageData, 0, image.ContentLength);
                    }

                    _unitOfWork.Goods.Create(newGood, categoryName, specificationIds);
                    _unitOfWork.Save();
                    Logger.Info($"Good id = {newGood.Id} was created");
                    return RedirectToAction("Create");
                }

                ModelState.AddModelError("", "Something went wrong");
                return View(newGood);
            }
            catch (Exception ex)
            {
                Logger.Error(ex,"Error while trying to create good");
                return RedirectToAction("Category", "Home");
            }


        }
        /// <summary>
        /// Get properties from selected category when creating new good
        /// </summary>
        /// <param name="category">selected category</param>
        /// <returns></returns>
        public JsonResult GetProperties(string category)
        {
            var properties = _unitOfWork.Properties.GetAllByCategoryName(category);
            var propList = new List<PropertyView>();
            foreach (var prop in properties)
            {
               
                var p = new PropertyView
                {
                    Id = prop.Id,
                    Name = prop.Name,
                    Specifications = new List<SpecificationView>()
                };
                prop.Specifications.ForEach(s=>p.Specifications.Add(new SpecificationView
                {
                    Id = s.Id,
                    Name = s.Name
                }));
                propList.Add(p);
            }

            return Json(propList, JsonRequestBehavior.AllowGet);

        }
        /// <summary>
        /// Get all category names
        /// </summary>
        /// <returns></returns>
        public JsonResult GetCategories()
        {
            return Json(_unitOfWork.Categories.GetAllNames(), JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// Get good description
        /// </summary>
        /// <param name="id">Good's id</param>
        /// <returns></returns>
        public ActionResult Description(int id)
        {
            var good = _unitOfWork.Goods.GetWithInclude(id);

            return View(good);
        }
        /// <summary>
        /// Post user's comment
        /// </summary>
        /// <param name="goodId">good's id</param>
        /// <param name="comment">comment-string</param>
        /// <param name="rate">rate (1 to 5)</param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public ActionResult Description(int goodId, string comment, int ?rate)
        {
            var good = _unitOfWork.Goods.Get(goodId);
            var user = _unitOfWork.Users.GetByLogin(HttpContext.User.Identity.Name);
            if (good != null && user!=null && rate>=0 && rate<=5)
            {
                var review = new Review {Comment = comment, Good = good, User = user, Rate = rate??1};
                _unitOfWork.Reviews.Create(review);
                _unitOfWork.Save();
            }
            Logger.Info("Get good description");
            return RedirectToAction("Description", new {id = goodId});
        }

        /// <summary>
        /// delete good from db
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin, Moderator")]
        public ActionResult DeleteGood(int id)
        {
            _unitOfWork.Goods.Delete(id);
            _unitOfWork.Save();
            Logger.Info($"Good id = {id} was deleted");
            return RedirectToAction("Category", "Home");
        }
    }
}