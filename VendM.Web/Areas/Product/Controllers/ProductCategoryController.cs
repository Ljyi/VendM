using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using VendM.Application;
using VendM.Core;
using VendM.Model.DataModel.Product;
using VendM.Model.DataModelDto.Product;
using VendM.Service;
using VendM.Web.Authorization;
using VendM.Web.BaseApplication;
using VendM.Web.Models;

namespace VendM.Web.Areas.Product.Controllers
{
    public class ProductCategoryController : ServicedController<ProductCategoryService>
    {

        GenerateCodeService generateCodeService = null;
        private ProductCategoryRedeem productCategoryRedeem;

        public ProductCategoryController()
        {
            generateCodeService = GenerateCodeService.SingleGenerateCodeService();
            this.productCategoryRedeem = new ProductCategoryRedeem();
        }
        #region SomeValue
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
        public JsonResult GetProductCategoryGrid(int limit, int offset, string sort, string sortOrder, string name)
        {
            TablePageParameter gp = new TablePageParameter { Limit = limit, Offset = offset, SortName = sort, SortOrder = sortOrder };
            List<ProductCategoryDto> productList = Service.GetProductCategoryGrid(gp, name);
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
            string productcategoryCode = "";
            generateCodeService.GetNextProductCategoryCode(ref productcategoryCode);
            ViewBag.productcategoryCode = productcategoryCode;
            return View();
        }

        [HttpPost]
        public ActionResult Add(ProductCategoryDto ProductCategoryDto)
        {
            string errorMsg = "";
            if (ModelState.IsValid)
            {
                ProductCategoryDto.CreateUser = ServiceHelper.GetCurrentUser().LoginName;
                ProductCategoryDto.CredateTime = DateTime.Now.Date;
                ProductCategoryDto.UpdateUser = ServiceHelper.GetCurrentUser().LoginName;
                ProductCategoryDto.UpdateTime = DateTime.Now.Date;
                if (Service.Add(ProductCategoryDto, ref errorMsg))
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
            ViewData.Model = entity;
            return View();
        }
        [HttpPost]
        public ActionResult Edit(ProductCategoryDto model)
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
            ViewBag.errorMsg = errorMsg;
            return View();
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <returns>返回操作结果</returns>
        [HttpPost]
        public JsonResult Delete(string ids)
        {
            ResultJsonInfo resultJsonInfo = new ResultJsonInfo { Data = "", ErrorMsg = "", Success = true };
            try
            {
                int[] idArr = ids.StrToIntArray();
                if (!Service.Deletes(idArr, ServiceHelper.GetCurrentUser().LoginName))
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
        /// 从API导入商品分类
        /// </summary>
        /// <returns>返回操作结果</returns>
        [HttpGet]
        public async Task<JsonResult> InportProductCategory()
        {
            ResultJsonInfo resultJsonInfo = new ResultJsonInfo { Data = "", ErrorMsg = "", Success = true };
            try
            {
                string result = await productCategoryRedeem.ImportProductCategoryFormAPIAsync();
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

        [HttpGet]
        public string ValidateName(string CategoryCode, int? id)
        {
            try
            {
                if (!Service.IsExist(CategoryCode, id))
                    return "true";
            }
            catch (Exception ex)
            {

            }
            return "false";
        }


        #endregion
    }
}