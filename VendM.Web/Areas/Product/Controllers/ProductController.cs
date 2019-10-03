using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using VendM.Application;
using VendM.Core;
using VendM.Model.DataModel.Product;
using VendM.Model.DataModelDto.Product;
using VendM.Model.EnumModel;
using VendM.Service;
using VendM.Web.Authorization;
using VendM.Web.BaseApplication;
using VendM.Web.Models;

namespace VendM.Web.Areas.Product.Controllers
{
    public class ProductController : ServicedController<ProductService>
    {

        GenerateCodeService generateCodeService = null;
        private ProductRedeem productRedeem;

        public ProductController()
        {
            generateCodeService = GenerateCodeService.SingleGenerateCodeService();
            this.productRedeem = new ProductRedeem();
        }

        /// <summary>
        /// 首页
        /// </summary>
        [HttpGet]
        public ActionResult Index()
        {
            var lis = GetProductCategorySelectList("", "");
            ViewBag.ProductCategoryList = new SelectList(lis, "value", "text");
            return View();
        }

        /// <summary>
        /// 列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetProductGrid(int limit, int offset, string sort, string sortOrder, string name, DateTime? dateFrom, DateTime? dateTo, int typeId, string productCode)
        {
            TablePageParameter gp = new TablePageParameter { Limit = limit, Offset = offset, SortName = sort, SortOrder = sortOrder };
            List<ProductDto> productList = Service.GetProductGrid(gp, name, dateFrom, dateTo, typeId, productCode);
            //JsonSerializerSettings setting = new JsonSerializerSettings()
            //{
            //    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            //    Formatting = Formatting.None
            //};
            //var ret = JsonConvert.SerializeObject(new { total = gp.TotalCount, rows = productList }, setting);
            return Json(new { total = gp.TotalCount, rows = productList }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 新建
        /// </summary>
        /// <param name="model">实体</param>
        /// <returns>返回操作结果</returns>
        [HttpGet]
        public ActionResult Add()
        {
            ViewData["ProductCategoryList"] = GetProductCategorySelectList("-1", "请选择");
            string productCode = "";
            generateCodeService.GetNextProdcutCode(ref productCode);
            ViewBag.productCode = productCode;
            return View();
        }

        [HttpPost]
        public ActionResult Add(ProductDto productDto)
        {
            string errorMsg = "";
            if (ModelState.IsValid)
            {
                productDto.CreateUser = ServiceHelper.GetCurrentUser().LoginName;
                productDto.CredateTime = DateTime.Now;
                productDto.UpdateUser = ServiceHelper.GetCurrentUser().LoginName;
                productDto.UpdateTime = DateTime.Now;
                if (Service.Add(productDto, ref errorMsg))
                {
                    return this.Ajax_JGClose();
                }
            }
            @ViewBag.errorMsg = errorMsg;
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="emptyKey"></param>
        /// <param name="emptyValue"></param>
        /// <param name="isAll"></param>
        /// <returns></returns>
        public List<SelectListItem> GetProductCategorySelectList(string emptyKey, string emptyValue, bool isAll = false)
        {
            List<SelectListItem> listItems = new List<SelectListItem>();
            if (!string.IsNullOrEmpty(emptyKey))
            {
                listItems.Add(new SelectListItem { Text = emptyValue, Value = emptyKey });
            }
            List<ProductCategoryDto> lists = new ProductCategoryService().GetProductCategoryGrid();
            foreach (var item in lists)
            {
                listItems.Add(new SelectListItem { Text = item.CategoryName_CN, Value = item.Id.ToString() });
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
            var lis = GetProductCategorySelectList("", "");
            ViewBag.ProductCategoryList = new SelectList(lis, "value", "text", entity.ProductCategoryId);
            ViewData.Model = entity;
            return View();
        }
        [HttpPost]
        public ActionResult Edit(ProductDto model)
        {
            string errorMsg = "";
            if (ModelState.IsValid)
            {
                model.UpdateUser = ServiceHelper.GetCurrentUser().LoginName;
                model.UpdateTime = DateTime.Now;
                if (Service.Update(model))
                {
                    return this.Ajax_JGClose();
                }
            }
            ViewBag.errorMsg = errorMsg;
            return View();
        }

        /// <summary>
        /// 设置商品金额
        /// </summary>
        /// <returns>返回操作结果</returns>
        [HttpGet]
        public ActionResult SetPrice(int id)
        {
            var entity = Service.GetProductPrices(id);
            ViewBag.Model = entity;
            ViewBag.allprice = 0;
            ViewBag.allPoint = 0;
            ViewBag.partPrice = 0;
            ViewBag.partPoint = 0;
            ViewBag.IsAllPrice = entity.Where(p => p.SaleType == (int)SaleTypeEnum.Money && p.Price > 0).Count() > 0 ? "true" : "false";
            if (ViewBag.IsAllPrice == "true")
            {
                ViewBag.allprice = entity.FirstOrDefault(p => p.SaleType == (int)SaleTypeEnum.Money).Price;
            }
            ViewBag.IsAllPoint = entity.Where(p => p.SaleType == (int)SaleTypeEnum.Point && p.Point > 0).Count() > 0 ? "true" : "false";
            if (ViewBag.IsAllPoint == "true")
            {
                ViewBag.allPoint = entity.FirstOrDefault(p => p.SaleType == (int)SaleTypeEnum.Point).Point;
            }
            ViewBag.IsPart = entity.Where(p => p.SaleType == (int)SaleTypeEnum.MoneyAndPoint && p.Price > 0 && p.Point > 0).Count() > 0 ? "true" : "false";
            if (ViewBag.IsPart == "true")
            {
                ViewBag.partPrice = entity.FirstOrDefault(p => p.SaleType == (int)SaleTypeEnum.MoneyAndPoint).Price;
                ViewBag.partPoint = entity.FirstOrDefault(p => p.SaleType == (int)SaleTypeEnum.MoneyAndPoint).Point;
            }
            return View();
        }

        /// <summary>
        /// 设置商品金额
        /// </summary>
        /// <returns>返回操作结果</returns>
        [HttpPost]
        public ActionResult SetPrice(ProductPriceDto model)
        {
            string errorMsg = "";
            if (ModelState.IsValid)
            {
                model.CreateUser = ServiceHelper.GetCurrentUser().LoginName;
                model.CredateTime = DateTime.Now;
                model.UpdateUser = ServiceHelper.GetCurrentUser().LoginName;
                model.UpdateTime = DateTime.Now;
                if (Service.SetProductPrice(model))
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
        public JsonResult DeleteProduct(string ids)
        {
            ResultJsonInfo resultJsonInfo = new ResultJsonInfo { Data = "", ErrorMsg = "", Success = true };
            try
            {
                ProductRedeem a = new ProductRedeem();
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


        [HttpGet]
        public string ValidateName(string ProductCode, int? id)
        {
            try
            {
                if (!Service.IsExist(ProductCode, id))
                    return "true";
            }
            catch (Exception ex)
            {

            }
            return "false";
        }


        public ActionResult Upload(string name, string type, string lastModifiedDate, int size, HttpPostedFileBase file)
        {
            //保存到临时文件夹 
            ResultJsonInfo resultJsonInfo = new ResultJsonInfo() { Data = "", ErrorMsg = "", Success = true };
            string urlPath = "/Upload/Photo";
            string filePathName = string.Empty;

            string localPath = Path.Combine(HttpRuntime.AppDomainAppPath, "Upload/Photo");
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

        public JsonResult DeleteImg(string filename)
        {
			var imgService = new ProdcuctImgeService();
			if (string.IsNullOrEmpty(filename))
				return Json(new { success = false, mes = "无文件名" });
			var res = imgService.DeleteImg(filename);

			if (!res)
				return Json(new { success = res, mes = "找不到该文件" });

			return Json(new ResultJsonInfo() { Data = "", ErrorMsg = "", Success = res }, JsonRequestBehavior.AllowGet);
		}

        /// <summary>
        /// 从API导入商品
        /// </summary>
        /// <returns>返回操作结果</returns>
        [HttpGet]
        public async Task<JsonResult> InportProduct()
        {
            ResultJsonInfo resultJsonInfo = new ResultJsonInfo { Data = "", ErrorMsg = "", Success = true };
            try
            {
                string result = await productRedeem.ImportProductFormAPIAsync();
                if (result.ToLower() != "true")
                {
                    resultJsonInfo = JsonConvert.DeserializeObject<ResultJsonInfo>(result);
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