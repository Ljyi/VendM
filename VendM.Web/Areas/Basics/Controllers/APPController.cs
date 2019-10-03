using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VendM.Core;
using VendM.Model.DataModel;
using VendM.Service.BasicsService;
using VendM.Web.Authorization;
using VendM.Web.BaseApplication;
using VendM.Web.Models;

namespace VendM.Web.Areas.Basics.Controllers
{
    public class APPController : ServicedController<APPVersionService>
    {
        // GET: Basics/APP

        /// <summary>
        /// 首页
        /// </summary>
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 列表
        /// </summary>
        /// <returns>返回查询列表</returns>
        [HttpGet]
        public JsonResult GetAPPVersionGrid(int limit, int offset, string sort, string sortOrder, string version)
        {
            TablePageParameter gp = new TablePageParameter() { Limit = limit, Offset = offset, SortName = sort, SortOrder = sortOrder };
            List<APP> appList = Service.GetAPPVersionGrid(gp, version);
            return Json(new
            {
                total = gp.TotalCount,
                rows = appList
            }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 新建
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Add()
        {
            return View();
        }
        /// <summary>
        /// 执行新建
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Add(APP model)
        {
            string errorMsg = "";
            if (ModelState.IsValid)
            {
                model.CreateUser = ServiceHelper.GetCurrentUser().LoginName;
                model.CredateTime = DateTime.Now;
                model.UpdateUser = ServiceHelper.GetCurrentUser().LoginName;
                model.UpdateTime = DateTime.Now;
                if (Service.Add(model, ref errorMsg))
                {
                    return this.Ajax_JGClose();
                }
            }
            @ViewBag.errorMsg = errorMsg;
            return View();
        }
        /// <summary>
        /// 编辑
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var entity = Service.Get(id);
            ViewData.Model = entity;
            ViewBag.Url = entity.Url;
            return View();
        }
        /// <summary>
        /// 执行编辑
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Edit(APP model)
        {
            string errorMsg = "";
            if (ModelState.IsValid)
            {
                model.UpdateUser = ServiceHelper.GetCurrentUser().LoginName;
                model.UpdateTime = DateTime.Now;
                if (Service.Update(model, ref errorMsg))
                {
                    return this.Ajax_JGClose();
                }
            }
            ViewBag.errorMsg = errorMsg;
            return View();
        }


        /// <summary>
        /// 删除
        /// </summary>
        /// <returns>返回操作结果</returns>
        [HttpPost]
        public JsonResult DeleteAPPVersion(string ids)
        {
            ResultJsonInfo resultJsonInfo = new ResultJsonInfo() { Data = "", ErrorMsg = "", Success = true };
            try
            {
                resultJsonInfo.Success = Service.DeleteAsync(ids, ServiceHelper.GetCurrentUser().LoginName);
            }
            catch (Exception ex)
            {
                resultJsonInfo.Success = false;
                resultJsonInfo.ErrorMsg = ex.Message;
            }
            return Json(resultJsonInfo, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 上传文件
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Upload(string name, string type, string lastModifiedDate, int size, HttpPostedFileBase file)
        {

            //保存到临时文件夹 
            ResultJsonInfo resultJsonInfo = new ResultJsonInfo() { Data = "", ErrorMsg = "", Success = true };
            string urlPath = "/Upload/APP";
            string filePathName = string.Empty;

            string localPath = Path.Combine(HttpRuntime.AppDomainAppPath, "Upload/APP");
            if (Request.Files.Count == 0)
                return Json(new { error = new { code = 102, message = "保存失败" } });

            string ex = Path.GetExtension(file.FileName);
            filePathName = Guid.NewGuid().ToString("N") + ex;
            if (!Directory.Exists(localPath))
            {
                Directory.CreateDirectory(localPath);
            }
            file.SaveAs(Path.Combine(localPath, filePathName));
            resultJsonInfo.Data = urlPath + "/" + filePathName;
            return Json(resultJsonInfo);
        }
        /// <summary>
        /// 清除URL
        /// </summary>
        /// <param name="id"></param>
        /// <param name="filename"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult DeleteFiles(int id, string filename)
        {
            string errorMsg = "";
            if (ModelState.IsValid)
            {
                Service.DeleteFiles(id, filename, ref errorMsg);
            }
            ViewBag.errorMsg = errorMsg;
            return View();
        }
    }
}