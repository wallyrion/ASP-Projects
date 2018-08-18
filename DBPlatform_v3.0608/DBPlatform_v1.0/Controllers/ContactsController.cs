using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DBPlatform_v1._0.Models;

namespace DBPlatform_v1._0.Controllers
{
    [Authorize]
    public class ContactsController : Controller
    {
        private DBPlatform db = new DBPlatform();



        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return View();
            }
            else
            {
                var contact = db.Contacts.Include(cont => cont.Country)
                                         .Include(cont=>cont.Title)
                                         .Include(cont=>cont.Company)
                                         .FirstOrDefault(cont => cont.Id == id);
                if (contact == null) return HttpNotFound();
                ContactEditView contactEditView = new ContactEditView(contact, db.Companies, db.Titles, db.Countries);
                return View(contactEditView);
            }
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Edit(ContactEditView editView)
        {
            var cont = db.Contacts.Find(editView.Id);
            if (cont == null) return HttpNotFound();
            if (cont.email!=editView.email && db.Contacts.FirstOrDefault(con=>con.email==editView.email)!=null)
            {
                ModelState.AddModelError("", "Email already Exist");
                editView.updateInfo(db.Companies, db.Titles, db.Countries);
                return View(editView);
            }
            else
            {
                cont.getNewInfo(editView);
                db.Entry(cont).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            
        }
        public ActionResult SearchByEmail(string email)
        {
            Contact contact = db.Contacts.FirstOrDefault(c => c.email == email);
            if (contact != null)
            {
                return RedirectToAction("Edit","Contacts", new { @id = contact.Id });
            }
            else return View("Edit");
        }



        // GET: Contacts
        public async Task<ActionResult> Index()
        {
            var t = await db.Contacts.Include(cont => cont.Country)
                                      .Include(cont => cont.Title)
                                      .Include(cont => cont.Company)
                                      .ToListAsync();
            return View(await db.Contacts.ToListAsync());
        }

        // GET: Contacts/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contact contact = await db.Contacts.FindAsync(id);
            if (contact == null)
            {
                return HttpNotFound();
            }
            return View(contact);
        }


        //[HttpPost]
        //public ActionResult Create2(ContactCreateView contactCreateView)
        //{
        //    //if (db.Contacts.First(c => c.email == contactCreateView.contact.email) == null)
        //    //{
        //    //    db.Contacts.Add(contactCreateView.contact);
        //    //    db.SaveChanges();
        //    //}
        //    //return View(contactCreateView);
        //}

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Create(ContactCreateView contactCreateView)
        {
            if (ModelState.IsValid) {
                var contact = db.Contacts.FirstOrDefault(c => c.email == contactCreateView.email);
                if (contact != null)
                {
                    ModelState.AddModelError("", "Email already Exist");

                }
                else
                {

                    db.Contacts.Add(new Contact(contactCreateView));
                    db.SaveChanges();
                    return RedirectToAction("Edit");
                }
            }
            contactCreateView.updateInfo(db.Companies, db.Titles, db.Countries);

            return View(contactCreateView);
        }
        public ActionResult Create()
        {
            ContactCreateView createView = new ContactCreateView(db.Companies, db.Titles, db.Countries);


            return View(createView);
        }
        private IEnumerable<SelectListItem> GetSelectListItems(IEnumerable<Company> elements)
        {
            // Create an empty list to hold result of the operation
            var selectList = new List<SelectListItem>();

            // For each string in the 'elements' variable, create a new SelectListItem object
            // that has both its Value and Text properties set to a particular value.
            // This will result in MVC rendering each item as:
            //     <option value="State Name">State Name</option>
            foreach (var element in elements)
            {
                selectList.Add(new SelectListItem
                {
                    Value = element.Id.ToString(),
                    Text = element.name
                });
            }

            return selectList;
        }

        // POST: Contacts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create2([Bind(Include = "first_name,last_name,email,prooflink,company_id_FK,title_id_FK,country_id_FK")] Contact contact)
        {
            if (ModelState.IsValid)
            {
                db.Contacts.Add(contact);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(contact);
        }

        // GET: Contacts/Edit/5
        public async Task<ActionResult> Edit2(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contact contact = await db.Contacts.FindAsync(id);
            if (contact == null)
            {
                return HttpNotFound();
            }
            return View(contact);
        }

        // POST: Contacts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit2([Bind(Include = "first_name,last_name,email,prooflink,company_id_FK,title_id_FK,country_id_FK")] Contact contact)
        {
            if (ModelState.IsValid)
            {
                db.Entry(contact).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(contact);
        }

        // GET: Contacts/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contact contact = await db.Contacts.Include(c=>c.Country)
                                               .Include(c=>c.Title)
                                               .Include(c=>c.Company)
                .FirstOrDefaultAsync(c=>c.Id==id);
            if (contact == null)
            {
                return HttpNotFound();
            }
            return View(contact);
        }

        // POST: Contacts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Contact contact = await db.Contacts.FindAsync(id);
            db.Contacts.Remove(contact);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


    }
}
