using System;
using System.Collections.Generic;
using System.Web.Mvc;
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
    public class StoreController : ServicedController<StoreService>
    {
        GenerateCodeService generateCodeService = null;
        public StoreController()
        {
            generateCodeService = GenerateCodeService.SingleGenerateCodeService();
        }

        #region SomeValue
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
        public JsonResult GetStoreGrid(int limit, int offset, string sort, string sortOrder, string name, string status)
        {
            TablePageParameter gp = new TablePageParameter() { Limit = limit, Offset = offset, SortName = sort, SortOrder = sortOrder };
            List<StoreDto> storeList = Service.GetStoreGrid(gp, name, status);
            return Json(new { total = gp.TotalCount, rows = storeList }, JsonRequestBehavior.AllowGet);
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
            string storeCode = "";
            generateCodeService.GetNextStoreCode(ref storeCode);
            ViewBag.storeCode = storeCode;
            return View();
        }
        [HttpPost]
        public ActionResult Add(StoreDto storedto)
        {
            try
            {
                string errorMsg = "";
                try
                {
                    if (ModelState.IsValid)
                    {
                        storedto.CreateUser = ServiceHelper.GetCurrentUser().LoginName;
                        storedto.CredateTime = DateTime.Now;
                        storedto.UpdateUser = ServiceHelper.GetCurrentUser().LoginName;
                        storedto.UpdateTime = DateTime.Now;
                    }
                    if (Service.Add(storedto))
                    {
                        return this.Ajax_JGClose();
                    }
                    ViewBag.StatusType = GetSelectList(typeof(StatusEnum), true);
                    string storeCode = "";
                    generateCodeService.GetNextStoreCode(ref storeCode);
                    ViewBag.storeCode = storeCode;
                    @ViewBag.errorMsg = "添加失败!";
                }
                catch (Exception ex)
                {
                    @ViewBag.errorMsg = ex;

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
        public ActionResult Edit(StoreDto storedto)
        {
            string errorMsg = "";
            if (ModelState.IsValid)
            {
                storedto.UpdateUser = ServiceHelper.GetCurrentUser().LoginName;
                storedto.UpdateTime = DateTime.Now;
                if (Service.Update(storedto))
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
                    resultJsonInfo.Data = "请检查该删除项是否存在机器或通道";
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