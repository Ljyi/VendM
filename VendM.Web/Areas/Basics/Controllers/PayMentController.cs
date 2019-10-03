using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using VendM.Core;
using VendM.Model.DataModelDto;
using VendM.Model.EnumModel;
using VendM.Service;
using VendM.Service.BasicsService;
using VendM.Web.Authorization;
using VendM.Web.BaseApplication;
using VendM.Web.Models;
namespace VendM.Web.Areas.Basics.Controllers
{
    public class PayMentController : ServicedController<PayMentService>
    {
        #region VendM.Web.Areas.Basics.Controllers
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
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetPayMentGrid(int limit, int offset, string sort, string sortOrder,string paymentcode, string paymentname)
        {
            TablePageParameter gp = new TablePageParameter() { Limit = limit, Offset = offset, SortName = sort, SortOrder = sortOrder };
            List<PayMentDto> machineList = Service.GetPayMentGrid(gp, paymentcode, paymentname);
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
            ViewBag.StatusType = GetSelectList(typeof(PaymentEnum), true);
            return View();
        }

        /// <summary>
        /// Market
        /// </summary>
        /// <param name="emptyKey"></param>
        /// <param name="emptyValue"></param>
        /// <param name="isAll"></param>
        /// <returns></returns>
        public List<SelectListItem> GetStoreSelectList(string emptyKey, string emptyValue, bool isAll = false)
        {
            List<SelectListItem> listItems = new List<SelectListItem>();
            if (emptyKey != null)
            {
                listItems.Add(new SelectListItem { Text = emptyValue, Value = emptyKey });
            }
            List<StoreDto> lists = new StoreService().GetStoreGrid();
            foreach (var item in lists)
            {
                listItems.Add(new SelectListItem { Text = item.Name, Value = item.Id.ToString() });
            }
            return listItems;
        }

        [HttpPost]
        public ActionResult Add(PayMentDto PayMentdto)
        {
            ResultJsonInfo resultJsonInfo = new ResultJsonInfo() { Data = "", ErrorMsg = "", Success = true };
            try
            {
                if (ModelState.IsValid)
                {
                    PayMentdto.CreateUser = ServiceHelper.GetCurrentUser().UserName;
                    PayMentdto.CredateTime = DateTime.Now;
                    PayMentdto.UpdateUser = ServiceHelper.GetCurrentUser().UserName;
                    PayMentdto.UpdateTime = DateTime.Now;
                }
                if (Service.Add(PayMentdto))
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
            var entity = Service.Find(id);
            ViewBag.StatusType = GetSelectList(typeof(PaymentEnum), true);
            ViewData.Model = entity;
            return View();
        }
        [HttpPost]
        public ActionResult Edit(PayMentDto PayMentdto)
        {
            string errorMsg = "";
            if (ModelState.IsValid)
            {
                PayMentdto.UpdateUser = ServiceHelper.GetCurrentUser().LoginName;
                PayMentdto.UpdateTime = DateTime.Now.Date;
                if (Service.Update(PayMentdto))
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

        public string ValidateName(string PaymentName)
        {
            try
            {
                if (!Service.IsExist(PaymentName))
                    return "true";
            }
            catch (Exception)
            {

            }
            return "false";
        }
        #endregion
    }
}