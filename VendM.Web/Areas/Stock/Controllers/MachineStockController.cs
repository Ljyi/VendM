using System;
using System.Collections.Generic;
using System.Web.Mvc;
using VendM.Core;
using VendM.Model.DataModelDto;
using VendM.Model.EnumModel;
using VendM.Service;
using VendM.Web.Authorization;
using VendM.Web.BaseApplication;
using VendM.Web.Models;

namespace VendM.Web.Areas.Stock.Controllers
{
    public class MachineStockController : ServicedController<MachineStockService>
    {
        #region  VendM.Web.Areas.AdvertiseMent.Controllers
        /// <summary>
        /// 首页
        /// </summary>
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.ReplenishmentUserList = GetReplenishmentUserSelectList("-1", "请选择");
            ViewBag.PercentList = GetSelectList(typeof(PercentEnum),true);
            return View();
        }
        /// <summary>
        /// 库存明细
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult MachineStockDetail(string storeName, string machineNo, string machineAddress)
        {
            ViewBag.storeName = storeName;
            ViewBag.machineNo = machineNo;
            ViewBag.machineAddress = machineAddress;
            return View();
        }
        /// <summary>
        /// 列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetMachineStockGrid(int limit, int offset, string sort, string sortOrder)
        {
            TablePageParameter gp = new TablePageParameter() { Limit = limit, Offset = offset, SortName = sort, SortOrder = sortOrder };
            List<MachineStockGridDto> machineStockList = Service.GetMachineStockGrid(gp);
            return Json(new { total = gp.TotalCount, rows = machineStockList }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="limit"></param>
        /// <param name="offset"></param>
        /// <param name="sort"></param>
        /// <param name="sortOrder"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetMachineStockDetailGrid(int limit, int offset, string sort, string sortOrder, string machineNo)
        {
            TablePageParameter gp = new TablePageParameter() { Limit = limit, Offset = offset, SortName = sort, SortOrder = sortOrder };
            List<MachineStockDetailDto> machineStockList = Service.GetMachineStockDetailGrid(gp, machineNo);
            return Json(new { total = gp.TotalCount, rows = machineStockList }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 新建
        /// </summary>
        /// <param name="model">实体</param>
        /// <returns>返回操作结果</returns>
        [HttpGet]
        public ActionResult AddMachineStock()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddMachineStock(MachineStockDto model)
        {
            string errorMsg = "";
            if (ModelState.IsValid)
            {
                model.CreateUser = ServiceHelper.GetCurrentUser().LoginName;
                model.CredateTime = DateTime.Now.Date;
                model.UpdateUser = ServiceHelper.GetCurrentUser().LoginName;
                model.UpdateTime = DateTime.Now.Date;
                if (Service.Add(model))
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
        /// <returns>返回操作结果</returns>
        [HttpGet]
        public ActionResult EditMachineStock(int id)
        {
            var entity = Service.Get(id);
            ViewData.Model = entity;
            return View();
        }
        [HttpPost]
        public ActionResult EditMachineStock(MachineStockDto model)
        {
            string errorMsg = "";
            if (ModelState.IsValid)
            {
                model.UpdateUser = ServiceHelper.GetCurrentUser().LoginName;
                model.UpdateTime = DateTime.Now.Date;
                if (Service.Update(model))
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
        public JsonResult DeleteMachineStock(int id)
        {
            ResultJsonInfo resultJsonInfo = new ResultJsonInfo() { Data = "", ErrorMsg = "", Success = true };
            try
            {
                if (!Service.Delete(id))
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

        /// <summary>
        /// 更新补货员
        /// </summary>
        /// <returns>返回操作结果</returns>
        [HttpPost]
        public JsonResult SetReplenishmentUser(string ids, string replenishmentUser)
        {
            ResultJsonInfo resultJsonInfo = new ResultJsonInfo() { Data = "", ErrorMsg = "", Success = true };
            try
            {
                if (!string.IsNullOrEmpty(ids) && !string.IsNullOrEmpty(replenishmentUser))
                {
                    resultJsonInfo.Success = Service.SetReplenishmentUser(ids, replenishmentUser);
                }
            }
            catch (Exception ex)
            {
                resultJsonInfo.Success = false;
                resultJsonInfo.ErrorMsg = ex.Message;
            }
            return Json(resultJsonInfo, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 设置预警库存百分比
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="replenishmentUser"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SetPercent(string ids, string percent)
        {
            ResultJsonInfo resultJsonInfo = new ResultJsonInfo() { Data = "", ErrorMsg = "", Success = true };
            try
            {
                if (!string.IsNullOrEmpty(ids) && !string.IsNullOrEmpty(percent))
                {
                    resultJsonInfo.Success = Service.SetPercent(ids, percent);
                }
            }
            catch (Exception ex)
            {
                resultJsonInfo.Success = false;
                resultJsonInfo.ErrorMsg = ex.Message;
            }
            return Json(resultJsonInfo, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="emptyKey"></param>
        /// <param name="emptyValue"></param>
        /// <param name="isAll"></param>
        /// <returns></returns>
        public List<SelectListItem> GetReplenishmentUserSelectList(string emptyKey, string emptyValue, bool isAll = false)
        {
            List<SelectListItem> listItems = new List<SelectListItem>();
            if (!string.IsNullOrEmpty(emptyKey))
            {
                listItems.Add(new SelectListItem { Text = emptyValue, Value = emptyKey });
            }
            List<ReplenishmentUserDto> lists = Service.GetReplenishmentUserList();
            foreach (var item in lists)
            {
                listItems.Add(new SelectListItem { Text = item.Name, Value = item.Id.ToString() });
            }
            return listItems;
        }

        #endregion
    }
}