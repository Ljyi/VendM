using System;
using System.Collections.Generic;
using System.Web.Mvc;
using VendM.Core;
using VendM.Model;
using VendM.Model.DataModel.Order;
using VendM.Model.DataModelDto.Order;
using VendM.Model.ExportModel;
using VendM.Service;
using VendM.Web.Authorization;
using VendM.Web.BaseApplication;
using VendM.Web.Models;

namespace VendM.Web.Areas.Order.Controllers
{
    public class OrderController : ServicedController<OrderService>
    {
        #region  VendM.Web.Areas.AdvertiseMent.Controllers
        /// <summary>
        /// 首页
        /// </summary>
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 
        /// </summary>
        [HttpGet]
        public ActionResult StoreOrder()
        {
            return View();
        }
        [HttpGet]
        public ActionResult StoreOrderDetail(DateTime? orderTime, string productCode, string storeName, string machineNo, string machineAddress)
        {
            ViewBag.storeName = storeName;
            ViewBag.machineNo = machineNo;
            ViewBag.machineAddress = machineAddress;
            ViewBag.productCode = productCode;
            ViewBag.orderTime = orderTime;
            return View();
        }
        /// <summary>
        /// 订单查询
        /// </summary>
        /// <param name="limit"></param>
        /// <param name="offset"></param>
        /// <param name="sort"></param>
        /// <param name="sortOrder"></param>
        /// <param name="orderNo">订单编号</param>
        /// <param name="storeNo">Market编号</param>
        /// <param name="machineNo">机器编号</param>
        /// <param name="productName">产品名称</param>
        /// <param name="dateFrom">创建时间</param>
        /// <param name="dateTo">创建时间</param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetOrderGrid(int limit, int offset, string sort, string sortOrder, string orderNo, string storeNo, string machineNo, DateTime? dateFrom, DateTime? dateTo)
        {
            TablePageParameter gp = new TablePageParameter() { Limit = limit, Offset = offset, SortName = sort, SortOrder = sortOrder };
            List<OrderDto> orderList = Service.GetOrderGrid(gp, orderNo: orderNo, storeNo: storeNo, machineNo: machineNo, dateFrom: dateFrom, dateTo: dateTo);
            return Json(new { total = gp.TotalCount, rows = orderList }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// Market订单明细列表
        /// </summary>
        /// <param name="limit"></param>
        /// <param name="offset"></param>
        /// <param name="sort"></param>
        /// <param name="sortOrder"></param>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetStoreOrderViewGrid(int limit, int offset, string sort, string sortOrder, string productName, string storeNo, string machineNo, DateTime? dateFrom, DateTime? dateTo)
        {
            TablePageParameter gp = new TablePageParameter() { Limit = limit, Offset = offset, SortName = sort, SortOrder = sortOrder };
            List<StoreOrderView> orderList = Service.GetStoreOrderViewGrid(gp, productName: productName, storeNo: storeNo, machineNo: machineNo, dateFrom: dateFrom, dateTo: dateTo);
            return Json(new { total = gp.TotalCount, rows = orderList }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 订单明细
        /// </summary>
        /// <param name="limit"></param>
        /// <param name="offset"></param>
        /// <param name="sort"></param>
        /// <param name="sortOrder"></param>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public JsonResult GetOrderDetailsGrid(int limit, int offset, string sort, string sortOrder, int orderId)
        {
            TablePageParameter gp = new TablePageParameter() { Limit = limit, Offset = offset, SortName = sort, SortOrder = sortOrder };
            List<OrderDetailDto> orderList = Service.GetOrderDetailsGrid(gp, orderId);
            return Json(new { total = gp.TotalCount, rows = orderList }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// Market订单明细
        /// </summary>
        /// <param name="limit"></param>
        /// <param name="offset"></param>
        /// <param name="sort"></param>
        /// <param name="sortOrder"></param>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public JsonResult GetStoreOrderDetailsGrid(int limit, int offset, string sort, string sortOrder, string productCode, string machineNo, DateTime? date)
        {
            TablePageParameter gp = new TablePageParameter() { Limit = limit, Offset = offset, SortName = sort, SortOrder = sortOrder };
            List<StoreOrderDetailDto> orderList = Service.GetStoreOrderDetailGrid(gp, productCode, machineNo, date);
            return Json(new { total = gp.TotalCount, rows = orderList }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 订单详情
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult OrderDetail(int orderId)
        {
            OrderView orderView = Service.GetOrderDetail(orderId);
            ViewData.Model = orderView;
            return View();
        }
        /// <summary>
        /// 订单明细
        /// </summary>
        /// <param name="limit"></param>
        /// <param name="offset"></param>
        /// <param name="sort"></param>
        /// <param name="sortOrder"></param>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetOrderDetail(int orderId)
        {
            OrderView orderView = Service.GetOrderDetail(orderId);
            return View(orderView);
        }
        /// <summary>
        /// 新建
        /// </summary>
        /// <param name="model">实体</param>
        /// <returns>返回操作结果</returns>
        [HttpGet]
        public ActionResult AddOrder()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddOrder(OrderDto model)
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
        public ActionResult EditOrder(int id)
        {
            var entity = Service.Get(id);
            ViewData.Model = entity;
            return View();
        }
        [HttpPost]
        public ActionResult EditOrder(OrderDto model)
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
        public JsonResult DeleteOrder(int id)
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
        /// Market列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetStorelist()
        {
            ResultJsonInfo<KeyValue> resultJsonInfo = new ResultJsonInfo<KeyValue>() { errorMsg = "", success = true };
            resultJsonInfo.data = Service.GetStorelist();
            return Json(resultJsonInfo, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 机器列表
        /// </summary>
        /// <param name="storeCode"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetMachinelist(string storeCode)
        {
            ResultJsonInfo<KeyValue> resultJsonInfo = new ResultJsonInfo<KeyValue>() { errorMsg = "", success = true };
            resultJsonInfo.data = Service.GetMachinelist(storeCode);
            return Json(resultJsonInfo, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 导出列表
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="limit"></param>
        /// <param name="offset"></param>
        /// <param name="sort"></param>
        /// <param name="sortOrder"></param>
        /// <param name="orderNo"></param>
        /// <param name="storeNo"></param>
        /// <param name="machineNo"></param>
        /// <param name="productName"></param>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ExportExcel(string ids, int? limit, int? offset, DateTime? dateFrom, DateTime? dateTo, string sort = "", string sortOrder = "", string orderNo = "", string storeNo = "", string machineNo = "")
        {
            ResultJsonInfo resultJsonInfo = new ResultJsonInfo() { Data = "", ErrorMsg = "", Success = true };
            List<OrderExport> list = Service.OrderExportGrid(ids, orderNo: orderNo, storeNo: storeNo, machineNo: machineNo, dateFrom: dateFrom, dateTo: dateTo);
            if (list == null || list.Count == 0)
            {
                resultJsonInfo.Success = false;
                resultJsonInfo.ErrorMsg = "暂未数据";
            }
            else
            {
                string fileName = "OrderExport.xls";
                Core.Excel.Npoi.ExportExcelHelper<OrderExport>.ExportListToExcel_MvcResult(list, ref fileName);
                var path = "/Export/Temp/" + fileName;
                resultJsonInfo.Data = path;
            }
            return Json(resultJsonInfo);
        }
        #endregion
    }
}