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
    public class RoleController :  ServicedController<RoleService>
    {
        // GET: System/Role
        public ActionResult Index()
        {
            return View();
        }
        #region 列表(Grid)
        /// <summary>
        /// Role列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetRoleGrid(int limit, int offset, string sort, string sortOrder, string rolename)
        {
            TablePageParameter gp = new TablePageParameter() { Limit = limit, Offset = offset, SortName = sort, SortOrder = sortOrder };
            List<RoleDto> roleDtos = Service.GetRoleGrid(gp, rolename);
            return Json(new { total = gp.TotalCount, rows = roleDtos }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        [HttpGet]
        public ActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Add(RoleDto roleDto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    roleDto.CreateUser = ServiceHelper.GetCurrentUser().UserName;
                    roleDto.CredateTime = DateTime.Now;
                    roleDto.UpdateUser = ServiceHelper.GetCurrentUser().UserName;
                    roleDto.UpdateTime = DateTime.Now;
                }
                if (Service.Add(roleDto))
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
        public ActionResult Edit(RoleDto roleDto)
        {
            ResultJsonInfo resultJsonInfo = new ResultJsonInfo() { Data = "", ErrorMsg = "", Success = true };
            try
            {
                if (ModelState.IsValid)
                {
                    roleDto.UpdateUser = ServiceHelper.GetCurrentUser().UserName;
                    roleDto.UpdateTime = DateTime.Now;
                    if (Service.Update(roleDto))
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
    }
}