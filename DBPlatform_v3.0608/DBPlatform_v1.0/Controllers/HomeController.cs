using DBPlatform_v1._0.Models;
using DBPlatform_v1._0.Models.Companies;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using DBPlatform_v1._0.Helpers;
using DBPlatform_v1._0.Models.Alias;
using DBPlatform_v1._0.Models.ViewHelpers;
using Microsoft.AspNet.Identity;
using WebGrease.Css.Extensions;

namespace DBPlatform_v1._0.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        DBPlatform dbNew = new DBPlatform();


        [Authorize(Roles = "Admin, Moderator")]
        public ActionResult Industries()
        {
            var indust = dbNew.Industries.Include(ind => ind.Aliases).ToList();
            ViewBag.unmappedAliases = dbNew.IndustryAliases.Where(a => a.Industry == null).ToList();
            return View(dbNew.Industries.ToList());
        }

        [Authorize(Roles = "Admin, Moderator")]
        [HttpPost]
        public ActionResult Industries(string newName)
        {
            if (!String.IsNullOrEmpty(newName))
            {
                var ind = dbNew.Industries.FirstOrDefault(i => i.Name == newName);
                if (ind == null)
                {
                    var umMapped = dbNew.IndustryAliases.FirstOrDefault(al => al.Name == newName);
                    if (umMapped != null)
                    {
                        dbNew.IndustryAliases.Remove(umMapped);
                    }
                    dbNew.Industries.Add(new Industry(newName));
                    dbNew.SaveChanges();
                }
            }

            ViewBag.unmappedAliases = dbNew.IndustryAliases.Where(a => a.Industry == null).ToList();
            return View(dbNew.Industries.ToList());
        }

        [Authorize(Roles = "Admin, Moderator")]
        [HttpPost]
        public ActionResult AddIndustryAlias(string aliasName, int? Id)
        {
            if (aliasName != null && Id != null)
            {
                var indExist = dbNew.Industries.FirstOrDefault(ind => ind.Name == aliasName);
                if (indExist == null)
                {
                    var indAliasExist = dbNew.IndustryAliases.FirstOrDefault(a => a.Name == aliasName);
                    var industry = dbNew.Industries.Find(Id);
                    if (industry != null)
                    {
                        if (indAliasExist != null && indAliasExist.Industry == null)
                        {
                            industry.Aliases.Add(indAliasExist);
                        }
                        else if (indAliasExist == null)
                        {
                            industry.Aliases.Add(new IndustryAlias { Name = aliasName });
                        }
                        dbNew.Entry(industry).State = EntityState.Modified;
                        dbNew.SaveChanges();
                    }
                }
            }

            return RedirectToAction("Industries");
        }

        [Authorize(Roles = "Admin, Moderator")]
        public ActionResult Countries()
        {
            return View(dbNew.Countries.ToList());
        }

        [Authorize(Roles = "Admin, Moderator")]
        [HttpPost]
        public ActionResult Countries(string newName)
        {
            var ind = dbNew.Countries.FirstOrDefault(i => i.Name == newName);
            if (ind == null)
            {
                dbNew.Countries.Add(new Models.Companies.Country(newName));
                dbNew.SaveChanges();
            }

            return View(dbNew.Countries.ToList());
        }

        [Authorize(Roles = "Admin, Moderator")]
        public ActionResult JobAreas()
        {
            return View(dbNew.JobAreas.ToList());
        }

        [Authorize(Roles = "Admin, Moderator")]
        [HttpPost]
        public ActionResult JobAreas(string newName)
        {
            var ind = dbNew.JobAreas.FirstOrDefault(i => i.Name == newName);
            if (ind == null)
            {
                dbNew.JobAreas.Add(new JobArea(newName));
                dbNew.SaveChanges();
            }

            return View(dbNew.JobAreas.ToList());
        }

        [Authorize(Roles = "Admin, Moderator")]
        public ActionResult JobLevels()
        {
            return View(dbNew.JobLevels.OrderBy(o => o.level).ToList());
        }

        [Authorize(Roles = "Admin, Moderator")]
        [HttpPost]
        public ActionResult JobLevels(string newName, int? newLevel)
        {
            var ind = dbNew.JobLevels.FirstOrDefault(i => i.Name == newName);
            if (ind == null && newLevel != null)
            {
                foreach (var item in dbNew.JobLevels.Where(l => l.level >= newLevel))
                {
                    item.level++;
                }

                dbNew.JobLevels.Add(new JobLevel(newName, (int)newLevel));
                dbNew.SaveChanges();
            }

            return View(dbNew.JobLevels.OrderBy(level => level.level).ToList());
        }

        [Authorize(Roles = "Admin, Moderator")]
        [HttpGet]
        public ActionResult DeleteJobLevel(int? id)
        {
            var jl = dbNew.JobLevels.FirstOrDefault(l => l.Id == id);
            if (jl != null)
            {
                dbNew.JobLevels.Remove(jl);
                dbNew.SaveChanges();
            }

            return RedirectToAction("JobLevels");
        }

        [Authorize(Roles = "Admin, Moderator")]
        [HttpGet]
        public ActionResult DeleteIndustry(int? id)
        {
            var ind = dbNew.Industries.FirstOrDefault(l => l.Id == id);
            if (ind != null)
            {
                dbNew.Industries.Remove(ind);
                dbNew.SaveChanges();
            }

            return RedirectToAction("Industries");
        }

        [Authorize(Roles = "Admin, Moderator")]
        [HttpPost]
        public ActionResult DeleteJobArea(int id)
        {

            var area = dbNew.JobAreas.FirstOrDefault(l => l.Id == id);
            if (area != null)
            {
                dbNew.JobAreas.Remove(area);
                dbNew.SaveChanges();
            }
            string message = "SUCCESS";
            return Json(new { Message = message, JsonRequestBehavior.AllowGet });


        }

        [Authorize(Roles = "Admin, Moderator")]
        [HttpGet]
        public ActionResult DeleteCountry(int? id)
        {
            var country = dbNew.Countries.FirstOrDefault(l => l.Id == id);
            if (country != null)
            {
                dbNew.Countries.Remove(country);
                dbNew.SaveChanges();
            }

            return RedirectToAction("Countries");
        }

        [Authorize(Roles = "Admin, Moderator")]
        [HttpGet]
        public ActionResult login()
        {
            //return View("authorization");
            return RedirectToAction("Index", "Account");
            // return View("IndexBooks",db.Books);
        }

        [Authorize(Roles = "Admin, Moderator")]
        [HttpPost]
        public ActionResult login(User user)
        {

            //return View("authorization");
            return View();
            // return View("IndexBooks",db.Books);
        }

        [HttpGet]
        public ActionResult Index()
        {
            List<SelectListItem> retrievedLists = new List<SelectListItem>();
            foreach (var retrieveList in dbNew.RetrieveLists)
            {
                retrievedLists.Add(new SelectListItem()
                {
                    Text = retrieveList.Name,
                    Value = retrieveList.Id.ToString(),
                    Selected = false
                });
            }

            ViewBag.retrievedLists = retrievedLists;
            return View();
        }

        [HttpPost]
        public ActionResult Index(int retrievedListId)
        {
            var retrievedList = dbNew.RetrieveLists.Find(retrievedListId);
            if (retrievedList == null) return RedirectToAction("Index");

            var contactsIds = retrievedList.ContactsIds.Split(',');

            var contacts = PlatformHelper.GetContactsByIds(dbNew, retrievedList.ContactsIds.Split(','));
            ViewBag.listId = retrievedList.Id;
            return View("RetrieveList", contacts);

        }
        //public string Index()
        //{
        //    string result = "Вы не авторизованы";
        //    if (User.Identity.IsAuthenticated)
        //    {
        //        result = "Ваш логин: " + User.Identity.Name;

        //        //return RedirectToAction("Index", "Account");
        //    }
        //    // else return RedirectToAction("Login", "Account");
        //    return result;

        //}

        [HttpPost]
        public string Array(List<string> names)
        {
            string fin = "";
            for (int i = 0; i < names.Count; i++)
            {
                fin += names[i] + ";  ";
            }

            return fin;
        }

        [HttpPost]
        public ActionResult authorization(string n)
        {
            return View();
        }

        [HttpGet]
        public ActionResult authorization()
        {
            return View();
        }

        public ActionResult GSheet(string gSheetUrl)
        {
            //gSheetUrl =
            // "https://docs.google.com/spreadsheets/d/1MXehuMdxTRZusvJdQhoz4PBlIQY5-V43K96SaKHNTlE/edit#gid=0";
            var leads = Helpers.GSheetHelper.GetContactInfoFromGSheet(gSheetUrl);
            if (leads == null)
                return RedirectToAction("Index");

            var jobLevels = dbNew.JobLevels.ToList();
            foreach (var lead in leads)
            {
                var contact = dbNew.Contacts.FirstOrDefault(c => c.email == lead.Email);
                if (lead.Email != "")
                {
                    if (contact == null)
                    {
                        contact = new Contact();
                        contact.email = lead.Email;
                        dbNew.Contacts.Add(contact);
                        dbNew.Entry(contact).State = EntityState.Added;
                    }
                    else dbNew.Entry(contact).State = EntityState.Modified;

                    contact.first_name = lead.FirstName;
                    contact.last_name = lead.LastName;
                    contact.prooflink = lead.Prooflink;
                    if (lead.Country != "")
                    {
                        var country = dbNew.Countries.FirstOrDefault(countr => countr.Name == lead.Country);
                        if (country != null)
                        {
                            contact.Country = country;
                        }
                    }

                    if (lead.Title != "")
                    {
                        var title = dbNew.Titles.FirstOrDefault(t => t.Name == lead.Title);
                        if (title == null)
                        {
                            title = new Title {Name = lead.Title};
                            

                            var jobLevel = dbNew.JobLevels.FirstOrDefault(jl => lead.Title.Contains(jl.Name));
                            if (jobLevel == null)
                            {
                                jobLevel = jobLevels.FirstOrDefault(jl => PlatformHelper.FindByParts(lead.Title, jl.Name));
                            }

                            if (jobLevel != null)
                            {
                                title.JobLevel = jobLevel;
                            }
                            dbNew.Titles.Add(title);
                            dbNew.Entry(title).State = EntityState.Added;
                            
                        }

                        contact.Title = title;

                    }
                }

                if (lead.Company != "")
                {
                    var company = dbNew.Companies.FirstOrDefault(comp => comp.name == lead.Company);
                    if (company == null)
                    {
                        company = new Company();
                        company.name = lead.Company;
                        dbNew.Companies.Add(company);
                        dbNew.Entry(company).State = EntityState.Added;
                    }
                    else dbNew.Entry(company).State = EntityState.Modified;

                    if (!String.IsNullOrEmpty(lead.Industry))
                    {
                        var industry = dbNew.Industries.FirstOrDefault(ind => ind.Name == lead.Industry);
                        if (industry == null)
                        {
                            var aliasIndustry =
                                dbNew.IndustryAliases.Include(indAlias=>indAlias.Industry).FirstOrDefault(indAlias => indAlias.Name == lead.Industry);
                            if (aliasIndustry == null)
                            {
                                dbNew.IndustryAliases.Add(new IndustryAlias { Name = lead.Industry });
                            }
                            else if (aliasIndustry.Industry != null)
                            {
                                company.Industries.Add(aliasIndustry.Industry);
                                company.PrimaryIndustry = aliasIndustry.Industry;
                            }
                        }
                        else
                        {
                            company.Industries.Add(industry);
                            company.PrimaryIndustry = industry;
                        }
                    }


                    company.SetDomain(lead.Email);
                    company.employees_prooflink = lead.EmpployeesProoflink;
                    company.revenue_prooflink = lead.RevenueProoflink;
                    company.SetMinMaxEmployees(lead.Employees);
                    company.SetMinMaxRevenue(lead.Revenue);
                    if (contact != null)
                    {
                        contact.Company = company;
                    }
                }

                /*if (lead.Title != "")
                {
                    var title = dbNew.Titles.FirstOrDefault(t => t.Name == lead.Title);
                    if (title == null)
                    {
                        title = new Title();
                        title.Name = lead.Title;
                        dbNew.Titles.Add(title);
                        dbNew.Entry(title).State = EntityState.Added;
                    }

                    if (contact != null)
                    {
                        contact.Title = title;
                    }
                }*/
                dbNew.SaveChanges();


            }



            return RedirectToAction("Index");
        }

        public ActionResult NotFound()
        {
            return HttpNotFound();
        }
        public ActionResult Downloads()
        {
            return View();
        }
        
        [HttpGet]
        public FileContentResult ExportList(int? id)
        {
            var generatedList = dbNew.RetrieveLists.Find(id);
            if (generatedList != null)
            {
                var contacts = PlatformHelper.GetContactsByIds(dbNew, generatedList.ContactsIds.Split(','));
                var csvString = PlatformHelper.GenerateCSVString(contacts);
                var fileName = generatedList.Name +" " + DateTime.UtcNow.ToString() + ".csv";
                return File(new System.Text.UTF8Encoding().GetBytes(csvString), "text/csv", fileName);
            }

            return null;


        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult GenerateList()
        {
            var generateList = new GenerateList
            {
                Industries = dbNew.Industries.ToList(),
                JobAreas = dbNew.JobAreas.ToList(),
                JobLevels = dbNew.JobLevels.ToList(),
                Countries = dbNew.Countries.ToList()
            };
            return View(generateList);
        }

        [HttpPost]
        public ActionResult GenerateList(string listName,
            int? employeesMin, int? employeesMax,
            int? revenueMin, int? revenueMax,
            int[] countries, int[] industries, int[] jobLevels, int[] jobAreas)
        {
            if (employeesMin == null) employeesMin = 0;
            if (employeesMax == null) employeesMax = 0;
            if (revenueMin == null) revenueMin = 0;
            if (revenueMax == null) revenueMax = 0;
            var ifListNameExist = dbNew.RetrieveLists.FirstOrDefault(l => l.Name == listName);
            if (ifListNameExist != null)
            {
                listName = "Copy of " + listName;
            }
            var list = new RetrieveList();
            list.Name = listName;

            List<Contact> contacts;
            if (countries != null)
            {
                contacts = dbNew.Contacts.Include(c => c.Company).Include(c => c.Country).Include(c => c.Company.Industries).Include(c => c.Title.JobLevel)
                    .Where(c => countries.Contains(c.Country.Id)).ToList();
            }
            else contacts = dbNew.Contacts.Include(c => c.Company).Include(c => c.Country).Include(c => c.Company.Industries).Include(c => c.Title.JobLevel).ToList();

            List<Contact> filterContacts = new List<Contact>();
            
            string contactsIds = "";
            foreach (var cont in contacts)
            {
                if (cont.Company.IfMinMaxFitsCriteria(employeesMin, employeesMax) &&
                    cont.Company.IfMinMaxFitsCriteria(revenueMin, revenueMax) &&
                    (jobLevels == null || (cont.Title.JobLevel != null && jobLevels.Contains(cont.Title.JobLevel.Id))) &&
                    (jobAreas == null || (cont.Title.JobAreas.Any(ja => jobAreas.Contains(ja.Id)))) &&
                    (industries == null || (cont.Company.Industries.Any(ind => industries.Contains(ind.Id)))))
                {
                    filterContacts.Add(cont);
                    contactsIds += (cont.Id.ToString() + ",");
                }
            }
            if (contactsIds.Length>0)
                contactsIds = contactsIds.Remove(contactsIds.Length - 1);

            list.ContactsIds = contactsIds;
            dbNew.RetrieveLists.Add(list);
            dbNew.SaveChanges();

            ViewBag.listId = list.Id;
            return View("RetrieveList", filterContacts);
        }

        [HttpPost]
        public ActionResult CreateJobArea(JobArea jobArea)
        {
            if (PlatformHelper.FindJobArea(jobArea.Name) == null)
            {
                dbNew.JobAreas.Add(jobArea);
                dbNew.SaveChanges();
            }
            string message = "SUCCESS";
            return Json(new { Message = message, JsonRequestBehavior.AllowGet });
        }

        public JsonResult GetJobareas(int ? id)
        {
            List<JobArea> jobAreas = new List<JobArea>();
            jobAreas = dbNew.JobAreas.ToList();

            var ja = new List<JobAreaHelper>();
            foreach (var jobArea in jobAreas)
            {
                ja.Add(new JobAreaHelper {Id = jobArea.Id, Name = jobArea.Name});
            }
            return Json(ja, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetLastAddedCompanies(int count)
        {
            var dbCompanies = dbNew.Companies.Include(comp=>comp.Contacts).Include(comp=>comp.PrimaryIndustry).OrderByDescending(comp => comp.DateCreated).Take(count).ToList();
            var companies = new List<CompanyViewHelper>(); 
            dbCompanies.ForEach(comp=>companies.Add(new CompanyViewHelper(comp)));
            return Json(companies, JsonRequestBehavior.AllowGet);
        }
    }
}