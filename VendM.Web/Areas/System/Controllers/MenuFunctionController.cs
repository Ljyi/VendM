using System.Web.Mvc;
using VendM.Service;
using VendM.Web.BaseApplication;

namespace VendM.Web.Areas.System.Controllers
{
    public class MenuFunctionController : ServicedController<MenuService>
    {
        // GET: System/MenuFunction
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult GetMenuTree()
        {
            var list = Service.GetMenuZTree();
            return Json(new { status = 1, value = list }, JsonRequestBehavior.AllowGet);
        }
    }
}