using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Eshop.Domain.Concrete;
using Eshop.Domain.Entities;
using Eshop.Models.Account;
using NLog;

namespace Eshop.Controllers
{
    public class AccountController : Controller
    {
        private readonly UnitOfWork _unitOfWork;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public AccountController()
        {
            _unitOfWork = new UnitOfWork();
            ViewData["AllCategories"] = _unitOfWork.Categories.GetAllNames();
        }

        /// <summary>
        /// Log in
        /// </summary>
        /// <returns></returns>
        public ActionResult Login()
        {
            return View();
        }

        /// <summary>
        /// Try to log in
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // поиск пользователя в бд
                    User user = _unitOfWork.Users.GetByLoginPassword(model.Login, model.Password);
                    if (user != null)
                    {
                        var authTicket = new FormsAuthenticationTicket(
                            1, // version
                            user.Login, // user name
                            DateTime.Now, // created
                            DateTime.Now.AddMinutes(20), // expires
                            true, // persistent?
                            user.Role // can be used to store roles
                        );

                        string encryptedTicket = FormsAuthentication.Encrypt(authTicket);

                        var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                        if (authTicket.IsPersistent)
                        {
                            authCookie.Expires = authTicket.Expiration;
                        }

                        System.Web.HttpContext.Current.Response.Cookies.Add(authCookie);

                        return RedirectToAction("Category", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError("", "The username or password is incorrect");
                    }
                }

                return View(model);
            }
            catch (Exception ex)
            {
                Logger.Error(ex,"");
                return Redirect("page500.aspx");
            }
        }
        /// <summary>
        /// Register
        /// </summary>
        /// <returns></returns>
        public ActionResult Register()
        {
            return View();
        }

        /// <summary>
        /// Try to register
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterModel model)
        {
            try
            {


                if (ModelState.IsValid)
                {
                    User user = _unitOfWork.Users.GetByLogin(model.Login);
                    if (user == null)
                    {
                        _unitOfWork.Users.Create(new User
                        {
                            Login = model.Login,
                            Password = model.Password,
                            Role = "User"
                        });
                        _unitOfWork.Save();
                        // создаем нового пользователя

                        user = _unitOfWork.Users.GetByLoginPassword(model.Login, model.Password);

                        // если пользователь удачно добавлен в бд
                        if (user != null)
                        {
                            var authTicket = new FormsAuthenticationTicket(
                                1, // version
                                user.Login, // user name
                                DateTime.Now, // created
                                DateTime.Now.AddMinutes(20), // expires
                                true, // persistent?
                                user.Role // can be used to store roles
                            );

                            string encryptedTicket = FormsAuthentication.Encrypt(authTicket);

                            var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                            if (authTicket.IsPersistent)
                            {
                                authCookie.Expires = authTicket.Expiration;
                            }

                            System.Web.HttpContext.Current.Response.Cookies.Add(authCookie);

                            return RedirectToAction("Category", "Home");

                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", $"User with login '{model.Login}' already exist");
                    }
                }

                return View(model);
            }
            catch (Exception ex)
            {
                Logger.Error(ex,"");
                return Redirect("page500.aspx");
            }
        }
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Category", "Home");
        }

    }
}