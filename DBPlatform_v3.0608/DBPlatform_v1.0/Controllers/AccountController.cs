using DBPlatform_v1._0.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace DBPlatform_v1._0.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Login()
        {
            return View();
        }

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                // поиск пользователя в бд
                User user = null;
                using (DBPlatform db = new DBPlatform())
                {
                    var temp = db.Users.ToList();
                    user = db.Users.FirstOrDefault(u => u.login == model.login && u.password == model.Password);

                }
                if (user != null)
                {
                    var authTicket = new FormsAuthenticationTicket(
                        1,                             // version
                        user.login,                      // user name
                        DateTime.Now,                  // created
                        DateTime.Now.AddMinutes(20),   // expires
                        true,                    // persistent?
                        user.Role                        // can be used to store roles
                    );

                    string encryptedTicket = FormsAuthentication.Encrypt(authTicket);

                    var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                    if (authTicket.IsPersistent)
                    {
                        authCookie.Expires = authTicket.Expiration;
                    }
                    System.Web.HttpContext.Current.Response.Cookies.Add(authCookie);


                    /*FormsAuthentication.SetAuthCookie(user.login, true);*/

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "The username or password is incorrect");
                }
            }

            return View(model);
        }

        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                User user = null;
                using (DBPlatform db = new DBPlatform())
                {
                    user = db.Users.FirstOrDefault(u => u.login == model.login);
                }
                if (user == null)
                {
                    // создаем нового пользователя
                    using (DBPlatform db = new DBPlatform())
                    {
                        db.Users.Add(new User { login = model.login, password = model.Password, Name = model.Name, Role = "User" });
                        db.SaveChanges();

                        user = db.Users.Where(u => u.login == model.login && u.password == model.Password).FirstOrDefault();
                    }

                    // если пользователь удачно добавлен в бд
                    if (user != null)
                    {
                        var authTicket = new FormsAuthenticationTicket(
                            1,                             // version
                            user.login,                      // user name
                            DateTime.Now,                  // created
                            DateTime.Now.AddMinutes(20),   // expires
                            true,                    // persistent?
                            user.Role                        // can be used to store roles
                        );

                        string encryptedTicket = FormsAuthentication.Encrypt(authTicket);

                        var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                        if (authTicket.IsPersistent)
                        {
                            authCookie.Expires = authTicket.Expiration;
                        }
                        System.Web.HttpContext.Current.Response.Cookies.Add(authCookie);

                        /*FormsAuthentication.SetAuthCookie(model.login, true);*/
                        return RedirectToAction("Index", "Home");

                    }
                }
                else
                {
                    ModelState.AddModelError("", $"User with login '{model.login}' already exist");
                }
            }

            return View(model);
        }
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                using (DBPlatform db = new DBPlatform())
                {
                    var user = db.Users.FirstOrDefault(u => u.login == User.Identity.Name);
                    return View(user);
                }

            }
            else return RedirectToAction("Login", "Account");

        }
        public ActionResult Logoff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Account");
        }
        [Authorize]
        public ActionResult Manage()
        {
            if (User.Identity.IsAuthenticated)
            {
                using (DBPlatform db = new DBPlatform())
                {
                    var user = db.Users.FirstOrDefault(u => u.login == User.Identity.Name);
                    return View(user);
                }

            }
            return View("Login");
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Users()
        {
            using (DBPlatform db = new DBPlatform())
            {
                var users = new List<User>();
                var curUser = User.Identity.Name;
                foreach (var dbUser in db.Users)
                {
                    if (dbUser.login == curUser) continue;
                    users.Add(new User
                    {
                        Id = dbUser.Id,
                        Name = dbUser.Name,
                        login = dbUser.login,
                        Role = dbUser.Role
                    });
                }

                return View(users);
            }
        }
        [Authorize(Roles = "Admin")]
        public ActionResult PromoteUser(int? id)
        {
            using (DBPlatform db = new DBPlatform())
            {
                var user = db.Users.Find(id);
                if (user != null)
                {
                    if (user.Role == "User")
                    {
                        user.Role = "Moderator";
                        db.Entry(user).State = EntityState.Modified;
                        db.SaveChanges();
                    }

                }
            }
            return RedirectToAction("Users");
        }

        [Authorize(Roles = "Admin")]
        public ActionResult DemodeUser(int? id)
        {
            using (DBPlatform db = new DBPlatform())
            {
                var user = db.Users.Find(id);
                if (user != null)
                {
                    if (user.Role == "Moderator")
                    {
                        user.Role = "User";
                        db.Entry(user).State = EntityState.Modified;
                        db.SaveChanges();
                    }

                }
            }
            return RedirectToAction("Users");
        }
        [Authorize(Roles = "Admin,Moderator")]
        public ActionResult DeleteUser(int? id)
        {
            using (DBPlatform db = new DBPlatform())
            {
                var user = db.Users.Find(id);
                if (user != null)
                {
                    db.Users.Remove(user);
                    db.SaveChanges();
                }

            }
            return RedirectToAction("Users");
        }

    }
}