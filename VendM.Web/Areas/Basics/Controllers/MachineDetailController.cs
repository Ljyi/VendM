using System;
using System.Collections.Generic;
using VendM.Service;
using System.Web.Mvc;
using VendM.Core;
using VendM.Model.DataModelDto;
using VendM.Model.DataModelDto.Product;
using VendM.Web.Authorization;
using VendM.Web.BaseApplication;
using VendM.Web.Models;

namespace VendM.Web.Areas.Basics.Controllers
{
    public class MachineDetailController : ServicedController<MachineDetailService>
    {
        #region VendM.Web.Areas.Basics.Controllers
        /// <summary>
        /// 首页
        /// </summary>
        [HttpGet]
        public ActionResult Index()
        {
            ViewData["ProductNoList"] = GetProductNoSelectList("-1", "请选择");
            return View();
        }
        /// <summary>
        /// 列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetMachineDetailGrid(int limit, int offset, string sort, string sortOrder, string machinemame, string machinenumber,string productid)
        {
            TablePageParameter gp = new TablePageParameter() { Limit = limit, Offset = offset, SortName = sort, SortOrder = sortOrder };
            List<MachineDetailDto> MachineDetailList = Service.GetMachineDetailGrid(gp, machinemame, machinenumber, productid);
            return Json(new { total = gp.TotalCount, rows = MachineDetailList }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 新建
        /// </summary>
        /// <param name="model">实体</param>
        /// <returns>返回操作结果</returns>
        [HttpGet]
        public ActionResult Add()
        {
            ViewData["MachineNoList"] = GetMachineNoSelectList("-1", "请选择");
            ViewData["ProductNoList"] = GetProductNoSelectList("-1", "请选择");
            return View();
        }

        [HttpPost]
        public ActionResult Add(MachineDetailDto MachineDetaildto)
        {
            ResultJsonInfo resultJsonInfo = new ResultJsonInfo() { Data = "", ErrorMsg = "", Success = true };
            try
            {
                if (ModelState.IsValid)
                {
                    if (!Service.CheckChannel(MachineDetaildto.MachineId, MachineDetaildto.PassageNumber))
                    {
                        if (Service.AddMachineDetail(MachineDetaildto, ServiceHelper.GetCurrentUser().UserName))
                        {
                            return this.Ajax_JGClose();
                        }
                    }
                    else
                    {
                        return Content("<script>alert('該機器的通道已存在');history.go(-1);</script>");
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
        /// 機器編號
        /// </summary>
        /// <param name="emptyKey"></param>
        /// <param name="emptyValue"></param>
        /// <param name="isAll"></param>
        /// <returns></returns>
        public List<SelectListItem> GetMachineNoSelectList(string emptyKey, string emptyValue, bool isAll = false)
        {
            List<SelectListItem> listItems = new List<SelectListItem>();
            if (emptyKey != null)
            {
                listItems.Add(new SelectListItem { Text = emptyValue, Value = emptyKey });
            }
            List<MachineDto> lists = new Service.MachineService().GetMachineGrid();
            foreach (var item in lists)
            {
                listItems.Add(new SelectListItem { Text = item.MachineNo, Value = item.Id.ToString() });
            }
            return listItems;
        }


        /// <summary>
        /// 產品名稱
        /// </summary>
        /// <param name="emptyKey"></param>
        /// <param name="emptyValue"></param>
        /// <param name="isAll"></param>
        /// <returns></returns>
        public List<SelectListItem> GetProductNoSelectList(string emptyKey, string emptyValue, bool isAll = false)
        {
            List<SelectListItem> listItems = new List<SelectListItem>();
            if (emptyKey != null)
            {
                listItems.Add(new SelectListItem { Text = emptyValue, Value = emptyKey });
            }
            List<ProductDto> lists = Service.GetProductGrid();
            foreach (var item in lists)
            {
                listItems.Add(new SelectListItem { Text = item.ProductName_CH, Value = item.Id.ToString() });
            }
            return listItems;
        }



        /// <summary>
        /// 编辑
        /// </summary>
        /// <returns>返回操作结果</returns>
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var entity = Service.Get(id);
            ViewData["MachineNoList"] = GetMachineNoSelectList("-1", "请选择");
            ViewData["ProductNoList"] = GetProductNoSelectList("-1", "请选择");
            ViewData.Model = entity;
            return View();
        }
        [HttpPost]
        public ActionResult Edit(MachineDetailDto MachineDetaildto)
        {
            string errorMsg = "";
            if (ModelState.IsValid)
            {
                MachineDetaildto.UpdateUser = ServiceHelper.GetCurrentUser().LoginName;
                MachineDetaildto.UpdateTime = DateTime.Now;
                if (Service.Update(MachineDetaildto,ref errorMsg))
                {
                    return this.Ajax_JGClose();
                }
            }
           
            ViewData["MachineNoList"] = GetMachineNoSelectList("-1", "请选择");
            ViewData["ProductNoList"] = GetProductNoSelectList("-1", "请选择");
            ViewData.Model = MachineDetaildto;
            if (!string.IsNullOrEmpty(errorMsg))
            {
                return Content($"<script>alert('{errorMsg}');history.go(-1);</script>");
            }
            else
            {
                return Content($"<script>alert('内部服务器错误!');history.go(-1);</script>");
            }
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
                    resultJsonInfo.Data = "请先清空机器通道的货存";
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