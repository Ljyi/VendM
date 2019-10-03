using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using VendM.Core;
using VendM.Model.DataModelDto;
using VendM.Model.EnumModel;
using VendM.Service.BasicsService;
using VendM.Web.Authorization;
using VendM.Web.BaseApplication;
using VendM.Web.Models;
namespace VendM.Web.Areas.Basics.Controllers
{
    public class ReplenishmentUserController : ServicedController<ReplenishmentUserService>
    {
        #region VendM.Web.Areas.Basics.Controllers
        /// <summary>
        /// 首页
        /// </summary>
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.StatusType = GetSelectList(typeof(StatusEnum), true);
            return View();
        }
        /// <summary>
        /// 列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetReplenishmentUserGrid(int limit, int offset, string sort, string sortOrder,string name, string email,string status)
        {
            TablePageParameter gp = new TablePageParameter() { Limit = limit, Offset = offset, SortName = sort, SortOrder = sortOrder };
            List<ReplenishmentUserDto> machineList = Service.GetReplenishmentUserGrid(gp,name, email, status);
            return Json(new { total = gp.TotalCount, rows = machineList }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 新建
        /// </summary>
        /// <param name="model">实体</param>
        /// <returns>返回操作结果</returns>
        [HttpGet]
        public ActionResult Add()
        {
            ViewBag.StatusType = GetSelectList(typeof(StatusEnum), true);
            return View();
        }

        ///// <summary>
        ///// Market
        ///// </summary>
        ///// <param name="emptyKey"></param>
        ///// <param name="emptyValue"></param>
        ///// <param name="isAll"></param>
        ///// <returns></returns>
        //public List<SelectListItem> GetStoreSelectList(string emptyKey, string emptyValue, bool isAll = false)
        //{
        //    List<SelectListItem> listItems = new List<SelectListItem>();
        //    if (emptyKey != null)
        //    {
        //        listItems.Add(new SelectListItem { Text = emptyValue, Value = emptyKey });
        //    }
        //    List<StoreDto> lists = new StoreService().GetStoreGrid();
        //    foreach (var item in lists)
        //    {
        //        listItems.Add(new SelectListItem { Text = item.Name, Value = item.Id.ToString() });
        //    }
        //    return listItems;
        //}

        public string ValidateReplenishmentUserNo(string replenishmentUserNo)
        {
            try
            {
                if (!Service.IsExistReplenishmentUserNo(replenishmentUserNo))
                    return "true";
            }
            catch (Exception ex)
            {

            }
            return "false";
        }

        [HttpPost]
        public ActionResult Add(ReplenishmentUserDto ReplenishmentUserdto)
        {
            ResultJsonInfo resultJsonInfo = new ResultJsonInfo() { Data = "", ErrorMsg = "", Success = true };
            try
            {
                if (ModelState.IsValid)
                {
                    ReplenishmentUserdto.CreateUser = ServiceHelper.GetCurrentUser().UserName;
                    ReplenishmentUserdto.CredateTime = DateTime.Now;
                    ReplenishmentUserdto.UpdateUser = ServiceHelper.GetCurrentUser().UserName;
                    ReplenishmentUserdto.UpdateTime = DateTime.Now;
                }
                if (Service.Add(ReplenishmentUserdto))
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
        /// <summary>
        /// 编辑
        /// </summary>
        /// <returns>返回操作结果</returns>
        [HttpGet]
        public ActionResult Edit(int id)
        {
            ViewBag.StatusType = GetSelectList(typeof(StatusEnum), true);
            var entity = Service.Get(id);
            ViewData.Model = entity;
            return View();
        }
        [HttpPost]
        public ActionResult Edit(ReplenishmentUserDto ReplenishmentUserdto)
        {
            string errorMsg = "";
            if (ModelState.IsValid)
            {
                ReplenishmentUserdto.UpdateUser = ServiceHelper.GetCurrentUser().LoginName;
                ReplenishmentUserdto.UpdateTime = DateTime.Now.Date;
                if (Service.Update(ReplenishmentUserdto))
                {
                    return this.Ajax_JGClose();
                }
            }
            @ViewBag.errorMsg = errorMsg;
            return View();
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <returns>返回操作结果</returns>
        [HttpPost]
        public JsonResult Delete(string ids)
        {
            ResultJsonInfo resultJsonInfo = new ResultJsonInfo() { Data = "", ErrorMsg = "", Success = true };
            try
            {
                if (!Service.Deletes(ids, ServiceHelper.GetCurrentUser().LoginName))
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
        #endregion
    }
}