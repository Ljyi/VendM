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
    public class MachineController : ServicedController<MachineService>
    {

        #region VendM.Web.Areas.Basics.Controllers
        /// <summary>
        /// 首页
        /// </summary>
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.FaultType = GetSelectList(typeof(FaultEnum), true);
            ViewBag.StatusType = GetSelectList(typeof(StatusEnum), true);
            return View();
        }
        /// <summary>
        /// 列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetMachineGrid(int limit, int offset, string sort, string sortOrder, string name, string code, string status, string fault)
        {
            TablePageParameter gp = new TablePageParameter() { Limit = limit, Offset = offset, SortName = sort, SortOrder = sortOrder };
            List<MachineDto> machineList = Service.GetMachineGrid(gp, name, code, status,fault);
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
            ViewData["StoreList"] = GetStoreSelectList("-1", "请选择");
            ViewBag.StatusType = GetSelectList(typeof(StatusEnum), true);
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

        //添加机器设备 同时也会加一条机器库存
        [HttpPost]
        public ActionResult Add(MachineDto Machinedto)
        {
            ResultJsonInfo resultJsonInfo = new ResultJsonInfo() { Data = "", ErrorMsg = "", Success = true };
            try
            {
                if (ModelState.IsValid)
                {
                    if (Service.AddMachine(Machinedto, ServiceHelper.GetCurrentUser().UserName))
                    {
                        return this.Ajax_JGClose();
                    }
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
            ViewBag.StoreList = new StoreService().GetStoreList("", "").ToSelectList(entity.StoreId);
            ViewBag.FaultType = EnumExtension.GetSelectList(typeof(FaultEnum), "", "故障类型...").ToSelectList(entity.FaultType);
            ViewData.Model = entity;
            return View();
        }
        [HttpPost]
        public ActionResult Edit(MachineDto Machinedto)
        {
            string errorMsg = "";
            if (ModelState.IsValid)
            {
                Machinedto.UpdateUser = ServiceHelper.GetCurrentUser().LoginName;
                Machinedto.UpdateTime = DateTime.Now;
                if (Service.Update(Machinedto))
                {
                    return this.Ajax_JGClose();
                }
            }
            else
            {
                var entity = Service.Get(Machinedto.Id);
                ViewBag.StoreList = new StoreService().GetStoreList("", "").ToSelectList(entity.StoreId);
                ViewData.Model = entity;
            }
            @ViewBag.errorMsg = errorMsg;
            return View();
        }

        /// <summary>
        /// 编辑故障类型
        /// </summary>
        /// <returns>返回操作结果</returns>
        [HttpGet]
        public ActionResult SetFault(int id)
        {
            var entity = Service.Get(id);
            ViewBag.FaultType = EnumExtension.GetSelectList(typeof(FaultEnum), "", "故障类型...").ToSelectList(entity.FaultType);
            ViewData.Model = entity;
            return View();
        }

        /// <summary>
        /// 编辑编辑故障类型
        /// </summary>
        /// <returns>返回操作结果</returns>
        [HttpPost]
        public ActionResult SetFault(MachineDto Machinedto)
        {
            string errorMsg = "";
            if (ModelState.IsValid)
            {
                if (Service.SetFault(Machinedto.Id, Machinedto.FaultType, Machinedto.FaultTime, Machinedto.HandleTime))
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
                    resultJsonInfo.Data = "请先删除该机器的通道";
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