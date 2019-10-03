using System.Web.Mvc;
using VendM.Service;
using VendM.Web.BaseApplication;
using VendM.Web.Models;

namespace VendM.Web.Areas.System.Controllers
{
    public class UserRightController : ServicedController<UserRightsService>
    {
        // GET: System/UserRight
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 获取用户权限
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetUserMenuFunctionTree(int? userId)
        {
            var list = Service.GetUserMenuZTree(userId);
            return Json(new { status = 1, value = list }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 保存用户功能
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Save(string sysUserMenuFunctionList, int userId)
        {
            ResultJsonInfo resultJsonInfo = new ResultJsonInfo() { Data = "", ErrorMsg = "", Success = true };
            return Json(resultJsonInfo, JsonRequestBehavior.AllowGet);
        }
    }
}