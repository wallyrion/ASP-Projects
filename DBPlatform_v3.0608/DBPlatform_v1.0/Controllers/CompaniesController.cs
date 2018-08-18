using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DBPlatform_v1._0.Helpers;
using DBPlatform_v1._0.Models;
namespace DBPlatform_v1._0.Controllers
{
    [Authorize]
    public class CompaniesController : Controller
    {
        DBPlatform dbNew = new DBPlatform();

        public ActionResult CompanyDetails(int? id)

        {
            if (id == null)
            {
                return HttpNotFound();
            }
            var company = dbNew.Companies.Include(c => c.Contacts).FirstOrDefault(comp => comp.Id == id);
            //Company2 company = dbNew.Companies2.Find(2);
            if (company == null)
            {
                return HttpNotFound();
            }
            return View(company);
        }


        [HttpGet]
        public ActionResult Index()
        {
            return View(new CompaniesSearchView
            {
                searchType = "All"
            });
        }



        [HttpPost]
        public ActionResult Index(CompaniesSearchView c)
        {

            if (c.searchType == "All")
            {
                c.companies = dbNew.Companies.Include(co => co.PrimaryIndustry).ToList();

            }

            else if (c.searchType == "Exact")
            {
                var comp = dbNew.Companies.FirstOrDefault(co => co.name == c.searchName);

                if (comp == null) return View(c);
                //var l = new List<Company>();
                //l.Add(comp);
                //c.companies = l;
                return RedirectToAction("EditCompany", "Companies", new { @id = comp.Id });

                // return View("EditCompany", comp);
            }
            else if (c.searchType == "By Parts")
            {
                if (c.searchName == "")
                    return View(c);
                
                    var parts = c.searchName.Split(' ');
                    c.companies = dbNew.Companies
                        .Include(co => co.PrimaryIndustry)
                        .Where(co => parts.Where(p=>p.Length>1)
                            .All(p => co.name.Contains(p))).ToList();
                
            }
            /*else c.companies = (dbNew.Companies.Where(co => co.name == c.searchName));
            List<SelectListItem> listSelectListItems = new List<SelectListItem>();
            foreach (Company comp in dbNew.Companies)
            {
                listSelectListItems.Add(new SelectListItem()
                {
                    Text = comp.name,
                    Value = comp.Id.ToString(),
                    Selected = false
                });
            }
            c.companiesList = listSelectListItems;*/

            return View(c);
        }
        //[HttpGet]
        //public ActionResult Index(string companyName)
        //{
        //    if (companyName == null)
        //        return View(new Company());
        //    var resultCompanies = dbNew.Companies.Where(c => c.name == companyName).ToList();

        //    Company resCompany = resultCompanies[0];
        //    return View(resCompany);
        //}
        //[HttpPost]
        //public ActionResult EditCompany(Company comp)
        //{
        //    dbNew.Entry(comp).State = EntityState.Modified;
        //    dbNew.SaveChanges();
        //    return RedirectToAction("Index");
        //}
        public ActionResult EditCompany()
        {
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult Edit(int? id)
        {

            Company comp;
            if (TempData["Emptyname"] != null)
            {
                ModelState.AddModelError("", "Empty company Name");
            }
            else if (TempData["NameExist"] != null) ModelState.AddModelError("", "Company already exist");
            else if (TempData["MinMax"] != null) ModelState.AddModelError("", "Min must be less than max");
            if (id == null)
            {
                comp = new Company();
            }
            else
            {
                comp = dbNew.Companies.Include(t => t.Contacts).FirstOrDefault(t => t.Id == id);
                if (comp == null)
                    return HttpNotFound();
            }

            CompanyEditView companyEditView = new CompanyEditView(comp, dbNew.Industries.ToList(), dbNew.Companies.ToList());
            return View(companyEditView);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CompanyEditView editView, int[] selectedIndustries, string submitbutton)
        {

            if (editView.company.name == null)
            {

                TempData["EmptyName"] = ViewData;
                return RedirectToAction("Edit", editView.company.Id);
            }
            if (!editView.company.isMinMaxGood())
            {
                TempData["MinMax"] = ViewData;
                return RedirectToAction("Edit", editView.company.Id);
            }
            Company comp = new Company();

            if (submitbutton == "Save")
            {
                comp = dbNew.Companies.Find(editView.company.Id);
                if (comp == null) return HttpNotFound();
            }

            if (comp.name != editView.company.name && dbNew.Companies.FirstOrDefault(co => co.name == editView.company.name) != null)
            {

                TempData["NameExist"] = ViewData;
                return RedirectToAction("Edit", comp.Id);
            }
            else
            {
                comp.Industries.Clear();
                comp.getCompanyChanges(editView.company);

                if (editView.company.PrimaryIndustryId == null && selectedIndustries != null)
                {
                    comp.PrimaryIndustryId = selectedIndustries[new Random().Next(0, selectedIndustries.Count())];
                }
                else if (editView.company.PrimaryIndustryId != null && selectedIndustries == null)
                {
                    comp.Industries.Add(dbNew.Industries.Find(editView.company.PrimaryIndustryId));
                }


                // comp.Industries.Clear();
                if (selectedIndustries != null)
                {

                    //получаем выбранные курсы
                    foreach (var c in dbNew.Industries.Where(ind => selectedIndustries.Contains(ind.Id)))
                    {
                        comp.Industries.Add(c);
                    }
                }


                if (submitbutton == "Save") dbNew.Entry(comp).State = EntityState.Modified;
                else dbNew.Companies.Add(comp);
                dbNew.SaveChanges();
                return RedirectToAction("Index");
            }


        }
        [HttpGet]
        public ActionResult EditCompany(int? id)
        {
            Company comp;
            if (id == null)
            {
                // return HttpNotFound();
                comp = new Company();
            }
            else
            {
                if (TempData["ViewData"] != null)
                {
                    ModelState.AddModelError("", "Company already exist");
                }
                //Company comp = dbNew.Companies.Find(id);
                comp = dbNew.Companies.Include(t => t.Contacts).FirstOrDefault(t => t.Id == id);
                if (comp == null)
                    return HttpNotFound();
            }
            SelectList AllIndustries = new SelectList(dbNew.Industries, "Id", "Name");
            ViewBag.AllIndustries = AllIndustries;
            ViewBag.Industries = dbNew.Industries.ToList();

            SelectList companies = new SelectList(dbNew.Companies.Where(co => co.name != comp.name), "Id", "Name");
            ViewBag.AllCompanies = companies;
            // CompanyEditView companyEditView = new CompanyEditView(companies, comp);
            return View(comp);


        }

        [HttpPost]
        public ActionResult EditCompany(Company company, int[] selectedIndustries, string submitbutton)
        {
            Company comp;
            if (submitbutton == "save")
            {
                comp = dbNew.Companies.Find(company.Id);
                comp.Industries.Clear();
                comp.getCompanyChanges(company);
            }
            else comp = company;
            if (comp.name != company.name && dbNew.Companies.FirstOrDefault(co => co.name == company.name) != null)
            {

                TempData["ViewData"] = ViewData;
                return RedirectToAction("EditCompany", comp.Id);
            }
            else
            {

                if (company.PrimaryIndustryId == null && selectedIndustries != null)
                {
                    comp.PrimaryIndustryId = selectedIndustries[new Random().Next(0, selectedIndustries.Count())];
                }
                else if (company.PrimaryIndustryId != null && selectedIndustries == null)
                {
                    comp.Industries.Add(dbNew.Industries.Find(company.PrimaryIndustryId));
                }


                // comp.Industries.Clear();
                if (selectedIndustries != null)
                {

                    //получаем выбранные курсы
                    foreach (var c in dbNew.Industries.Where(ind => selectedIndustries.Contains(ind.Id)))
                    {
                        comp.Industries.Add(c);
                    }
                }


                if (submitbutton == "save") dbNew.Entry(comp).State = EntityState.Modified;
                else dbNew.Companies.Add(comp);
                dbNew.SaveChanges();
                return RedirectToAction("Index");
            }



        }
        [HttpPost]
        public ActionResult Merge(IEnumerable<int> SelectedCompanies, string primaryMerge)
        {
            PlatformHelper.MergeCompanies(dbNew, primaryMerge, SelectedCompanies);

            return RedirectToAction("Index");
        }
        public ActionResult Create()
        {
            //SelectList companies = new SelectList(dbNew.Companies, "company_id", "Name");
            //ViewBag.AllCompanies = companies;
            return RedirectToAction("Edit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Company company)
        {
            dbNew.Companies.Add(company);
            dbNew.SaveChanges();

            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult DeleteCompany(int id)
        {
            Company co = dbNew.Companies.Find(id);
            if (co == null)
            {
                return HttpNotFound();
            }
            dbNew.Companies.Remove(co);
            dbNew.SaveChanges();
            var cs = new CompaniesSearchView();
            cs.searchType = "All";
            return RedirectToAction("Index");
        }


    }
}