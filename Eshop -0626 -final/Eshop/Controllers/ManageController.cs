using System;
using System.Linq;
using System.Web.Mvc;
using Eshop.Domain.Concrete;
using Eshop.Domain.Entities.Goods;
using Eshop.Models;
using NLog;

namespace Eshop.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ManageController : Controller
    {
        private readonly UnitOfWork _unitOfWork;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public ManageController()
        {
            _unitOfWork = new UnitOfWork();
            ViewData["AllCategories"] = _unitOfWork.Categories.GetAllNames();
        }
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Categories()
        {
            var cat = _unitOfWork.Categories.GetAll().ToList();
            return View(cat);
        }

        public ActionResult AddCategory(string categoryName)
        {
            if (!String.IsNullOrEmpty(categoryName))
            {
                _unitOfWork.Categories.Create(new Category {Name = categoryName});
                _unitOfWork.Save();
            }
            return RedirectToAction("Categories");
        }

        public ActionResult AddProperty(string propertyName, int categoryId)
        {
            var category = _unitOfWork.Categories.Get(categoryId);
            if (category != null && !String.IsNullOrEmpty(propertyName))
            {
                _unitOfWork.Properties.Create(new Property {Name = propertyName, Category = category});
                _unitOfWork.Save();
            }
            
            return RedirectToAction("Categories");
        }

        public ActionResult AddSpecification(string specificationName, int propertyId)
        {
            var property = _unitOfWork.Properties.Get(propertyId);

            if (property!=null && !String.IsNullOrEmpty(specificationName))
            {
                _unitOfWork.Specifications.Create(new Specification {Name = specificationName,Property = property});
                _unitOfWork.Save();
            }

            return RedirectToAction("Categories");
        }

        public ActionResult Users()
        {
            return View(_unitOfWork.Users.GetAll().Where(u => u.Login != HttpContext.User.Identity.Name));
        }
        public ActionResult Deleteuser(int id)
        {
            _unitOfWork.Users.Delete(id);
            _unitOfWork.Save();
            return RedirectToAction("Users");
        }
        public ActionResult PromoteUser(int id)
        {
            _unitOfWork.Users.PromoteUser(id);
            return RedirectToAction("Users");
        }

        public ActionResult DemodeUser(int id)
        {
            _unitOfWork.Users.DemodeUser(id);
            return RedirectToAction("Users");
        }

        public ActionResult BlockUser(int id)
        {
            _unitOfWork.Users.BlockUser(id);
            return RedirectToAction("Users");

        }

        public ActionResult UnblockUser(int id)
        {
            _unitOfWork.Users.UnBlockUser(id);
            return RedirectToAction("Users");
        }


        public ActionResult Category(int id)
        {
            var category = _unitOfWork.Categories.Get(id);
            var renameHelper = new RenameView(category.Name, category.Id, "Category");
            return View("Rename",renameHelper);
        }

        [HttpPost]
        public ActionResult Category(RenameView renameView)
        {
            var newCategoryExist = _unitOfWork.Categories.GetByName(renameView.NewName);
            var category = _unitOfWork.Categories.Get(renameView.OldName);
            if (category != null && newCategoryExist==null)
            {
                category.Name = renameView.NewName;
                _unitOfWork.Categories.Update(category);
                _unitOfWork.Save();
                Logger.Info($"Category id = {category.Id} updated: set name = {category.Name}");

            }
            return RedirectToAction("Categories");
        }
        public ActionResult Property(int id)
        {
            var property = _unitOfWork.Properties.Get(id);
            var renameHelper = new RenameView(property.Name, property.Id, "Property");
            return View("Rename", renameHelper);
        }

        [HttpPost]
        public ActionResult Property(RenameView renameView)
        {
            var property = _unitOfWork.Properties.Get(renameView.ItemId);
            if (property != null )
            {
                property.Name = renameView.NewName;
                _unitOfWork.Properties.Update(property);
                _unitOfWork.Save();
                Logger.Info($"Property id = {property.Id} updated: set name = {property.Name}");

            }
            return RedirectToAction("Categories");
        }
        public ActionResult Specification(int id)
        {
            var specification = _unitOfWork.Specifications.Get(id);
            var renameHelper = new RenameView(specification.Name, specification.Id, "Specification");
            return View("Rename",renameHelper);
        }
        [HttpPost]
        public ActionResult Rename(RenameView renameView)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction(renameView.ActionName);
            }

            return View(renameView);

        }
        [HttpPost]
        public ActionResult Specification(RenameView renameView)
        {

            var specification = _unitOfWork.Specifications.Get(renameView.ItemId);
            if (specification != null)
            {
                specification.Name = renameView.NewName;
                _unitOfWork.Specifications.Update(specification);
                _unitOfWork.Save();
                Logger.Info($"Specification id = {specification.Id} updated: set name = {specification.Name}");

            }
            return RedirectToAction("Categories");
        }
        public ActionResult Orders()
        {
            var orders = _unitOfWork.Orders.GetAll();
            return View(orders);
        }

        [HttpPost]
        public ActionResult Orders(int status, int id)
        {
            var order = _unitOfWork.Orders.Get(id);
            if (order != null)
            {
                if (status <= 2 && status >= 0)
                {
                    order.Status = status;
                    _unitOfWork.Orders.Update(order);
                    _unitOfWork.Save();
                    Logger.Info($"order id = {id} updated: set status = {status}");

                }
            }
            return RedirectToAction("Orders");
        }

        public ActionResult DeleteSpecification(int id)
        {
            _unitOfWork.Specifications.Delete(id);
            _unitOfWork.Save();
            Logger.Info($"Specification id = {id} was deleted");
            return RedirectToAction("Categories");
        }

        public ActionResult DeleteProperty(int id)
        {
            _unitOfWork.Properties.Delete(id);
            _unitOfWork.Save();
            Logger.Info($"Property id = {id} was deleted");
            return RedirectToAction("Categories");
        }

        public ActionResult DeleteCategory(int id)
        {
            _unitOfWork.Categories.Delete(id);
            _unitOfWork.Save();
            Logger.Info($"Category id = {id} was deleted");
            return RedirectToAction("Categories");
        }
    }
}