using System;
using System.Collections.Generic;
using System.Web.Mvc;
using VendM.Core;
using VendM.Core.Excel.Npoi;
using VendM.Model.DataModelDto;
using VendM.Service;
using VendM.Web.Authorization;
using VendM.Web.BaseApplication;
using VendM.Web.Models;
using VendM.Web.Models.ExportModel;


namespace VendM.Web.Areas.System.Controllers
{
    public class UserController :  ServicedController<UserService>
    {
        // GET: System/User
        public ActionResult Index()
        {
            return View();
        }
        #region 列表(Grid)
        /// <summary>
        /// User列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetUserGrid(int limit, int offset, string sort, string sortOrder, string username, string logingname)
        {
            TablePageParameter gp = new TablePageParameter() { Limit = limit, Offset = offset, SortName = sort, SortOrder = sortOrder };
            List<UserDto> userDtos = Service.GetUserGrid(gp, username, logingname);
            return Json(new { total = gp.TotalCount, rows = userDtos }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        [HttpGet]
        public ActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Add(UserDto userDto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    userDto.CreateUser = ServiceHelper.GetCurrentUser().UserName;
                    userDto.CredateTime = DateTime.Now;
                    userDto.UpdateUser = ServiceHelper.GetCurrentUser().UserName;
                    userDto.UpdateTime = DateTime.Now;
                }
                if (Service.Add(userDto))
                {
                    return this.Ajax_JGClose();
                }
                return View();
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                throw;
            }
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var entity = Service.Find(id);
            ViewData.Model = entity;
            return View();
        }
        [HttpPost]
        public ActionResult Edit(UserDto userDto)
        {
            ResultJsonInfo resultJsonInfo = new ResultJsonInfo() { Data = "", ErrorMsg = "", Success = true };
            try
            {
                if (ModelState.IsValid)
                {
                    userDto.UpdateUser = ServiceHelper.GetCurrentUser().UserName;
                    userDto.UpdateTime = DateTime.Now;
                    if (Service.Update(userDto))
                    {
                        return this.Ajax_JGClose();
                    }
                }
                return View();
            }
            catch (Exception)
            {
                throw;
            }

        }
        [HttpPost]
        public JsonResult Delete(string ids)
        {
            ResultJsonInfo resultJsonInfo = new ResultJsonInfo() { Data = "", ErrorMsg = "", Success = true };
            try
            {
                if (!Service.Deletes(ids, ServiceHelper.GetCurrentUser().UserName))
                {
                    resultJsonInfo.Success = false;
                }
            }
            catch (Exception ex)
            {
                resultJsonInfo.Success = false;
                resultJsonInfo.ErrorMsg = ex.Message;
            }
            return Json(resultJsonInfo, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult ExportExcel(string ids)
        {
            ResultJsonInfo resultJsonInfo = new ResultJsonInfo() { Data = "", ErrorMsg = "", Success = true };
            List<UserExport> userExportModels = new List<UserExport>();
            List<UserDto> list = Service.GetAllUsers();
            //if (!string.IsNullOrEmpty(ids))
            //{
            //    list = Service.Find(ids);
            //}
            //else
            //{

            //}
            foreach (var item in list)
            {
                userExportModels.Add(new UserExport()
                {
                    用户名 = item.UserName,
                    账号 = item.LogingName,
                    邮箱 = item.Email
                });
            }
            if (list == null || list.Count == 0)
            {
                resultJsonInfo.Success = false;
                resultJsonInfo.ErrorMsg = "暂未数据";
            }
            else
            {
                string fileName = "用户导出.xls";
                ExportExcelHelper<UserExport>.ExportListToExcel_MvcResult(userExportModels, ref fileName);
                var path = "/Export/Temp/" + fileName;
                resultJsonInfo.Data = path;
            }
            return Json(resultJsonInfo);
        }
    }
}