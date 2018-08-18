using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using DBPlatform_v1._0.Models;
//using DBPlatform_v1._0.Models.Books;

namespace DBPlatform_v1._0.Controllers
{

    public class BooksController : Controller
    {
        // GET: Books
        BookContext db = new BookContext();
        public ActionResult Index()
        {
            return View(db.Books.ToList());
        }
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(Book book)
        {
            db.Entry(book).State = EntityState.Added;
            db.SaveChanges();

            return RedirectToAction("Index");
        }
         //[HttpPost]
        public ActionResult Add()
        {
            return View(db.Books.ToList());
        }
        [HttpGet]
        public ActionResult Delete(int id)
        {
            Book b = db.Books.Find(id);
            if (b == null)
            {
                return HttpNotFound();
            }
            return View(b);
        }
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Book b = db.Books.Find(id);
            if (b == null)
            {
                return HttpNotFound();
            }
            db.Books.Remove(b);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult BookView(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            Book book = db.Books.Find(id);
            if (book != null)
            {
                return View(book);
            }
            return HttpNotFound();
        }
        [HttpPost]
        public ActionResult GetAuthor(Author author)
        {

            return View();
        }
        [HttpGet]
        public ActionResult GetAuthor()
        {
            return View();
        }
    }

}