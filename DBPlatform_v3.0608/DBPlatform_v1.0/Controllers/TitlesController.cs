using DBPlatform_v1._0.Models;
using DBPlatform_v1._0.Models.Companies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace DBPlatform_v1._0.Controllers
{
    [Authorize]
    public class TitlesController : Controller
    {
        DBPlatform dbNew = new DBPlatform();
        // GET: Titles
        
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(SearchView searchView)
        {
            if (searchView.searchType == "All") return View(dbNew.Titles.Include(l => l.JobLevel).ToList());
            else if (searchView.searchType == "By Parts")
            {
                var list = dbNew.Titles.Include(t=>t.JobLevel).Where(t => t.Name.Contains(searchView.searchName)).ToList();
                return View(list);
            }
            else if (searchView.searchType == "Exact")
            {
                var list = dbNew.Titles.Include(l => l.JobLevel).Where(t => t.Name == searchView.searchName).ToList();
                return View(list);
            }
            if (searchView.searchName == null) return View();



            return View();
        }
        //public ActionResult Edit()
        //{
          
        //    TitleEditView titleEditView = new TitleEditView(t, dbNew.JobAreas, dbNew.JobLevels);
        //    return View(titleEditView);
        //}
        [HttpGet]
        public ActionResult Edit(int ?  id)
        {
            Title title = null;
            //var temp = dbNew.JobAreas.ToList();
            if (id == null) title = new Title();
            else
            {
                title = dbNew.Titles.Include(t => t.JobLevel).Include(t => t.Contacts).FirstOrDefault(t => t.Id == id);
            }
            if (title == null)
                return HttpNotFound();
            TitleEditView titleEditView = new TitleEditView(title, dbNew.JobAreas, dbNew.JobLevels.OrderBy(l=>l.level));
            return View(titleEditView);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(TitleEditView editView, string submitbutton)
        {
            //if (ModelState.IsValid)
            //{
            //    return ("Good");
            //}

            if (editView.Title.Name == null)
            {

                TempData["EmptyName"] = ViewData;
                return RedirectToAction("Edit", editView.Title.Id);
            }
            Title title;
            if (submitbutton == "Save")
            {
                title = dbNew.Titles.Find(editView.Title.Id);
               


            }
            else title = editView.Title;
            if (title.Name != editView.Title.Name && dbNew.Titles.FirstOrDefault(co => co.Name == editView.Title.Name) != null)
            {

                TempData["NameExist"] = ViewData;
                return RedirectToAction("Edit", title.Id);
                //return View("~/Views/Shared/Error.cshtml");
                //ModelState.AddModelError("", "Title Already Exist");
                //return View(editView);
            }

            else
            {

                title.JobAreas.Clear();
                title.Name = editView.Title.Name;
                title.JobLevelId = editView.Title.JobLevelId;
                if (editView.selectedJobAreas != null)
                {
                    foreach (var c in dbNew.JobAreas.Where(area => editView.selectedJobAreas.Contains(area.Id)))
                    {
                        title.JobAreas.Add(c);
                    }
                }

                if (submitbutton == "Save") dbNew.Entry(title).State = EntityState.Modified;
                else dbNew.Titles.Add(title);
                dbNew.SaveChanges();
                return RedirectToAction("Index");
            }
            

        }
        [HttpGet]
        public ActionResult Delete(int id)
        {
            Title title = dbNew.Titles.Find(id);
            if (title == null)
            {
                return HttpNotFound();
            }
            dbNew.Titles.Remove(title);
            dbNew.SaveChanges();
            return RedirectToAction("Index");
        }
        //public ActionResult Index()
        //{
        //    return View("Search");
        //}
    }
}