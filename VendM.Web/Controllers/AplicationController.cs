using System.Web.Mvc;
using System.Web.Security;
using VendM.Service;
using VendM.Web.Authorization;
using VendM.Web.BaseApplication;
using VendM.Web.Models;

namespace VendM.Web.Controllers
{
    public class AplicationController : ServicedController<SystemService>
    {
        // GET: Aplication
        public ActionResult Index()
        {
            var user = ServiceHelper.GetCurrentUser();
            ViewBag.User = user.LoginName;
            int userId = user.UserID;
            bool isAdmin = user.IsAdmin;
            ViewBag.MenuList = Service.GetMenuByUserId(userId, isAdmin);
            return View();
        }
        [HttpGet]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            ViewBag.errorMsg = "";
            return View();
        }
        [HttpPost]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            string errorMsg = "";
            if (ModelState.IsValid)
            {
                bool isLoginSc = AuthenticationModule.AuthenticateUser(model.LoginName, model.Password, ref errorMsg);
                if (isLoginSc)
                {
                    return RedirectToAction("Index", "Aplication");
                }
                ViewBag.errorMsg = errorMsg;
            }
            return View(model);
        }
        [HttpGet]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Aplication");
        }
    }
}