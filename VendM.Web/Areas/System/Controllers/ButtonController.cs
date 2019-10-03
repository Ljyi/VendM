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
    public class ButtonController :  ServicedController<ButtonService>
    {
        // GET: System/Button
        public ActionResult Index()
        {
            return View();
        }
        #region 列表(Grid)
        /// <summary>
        /// Button列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetButtonGrid(int limit, int offset, string sort, string sortOrder, string buttonname)
        {
            TablePageParameter gp = new TablePageParameter() { Limit = limit, Offset = offset, SortName = sort, SortOrder = sortOrder };
            List<ButtonDto> buttonDtos = Service.GetButtonGrid(gp, buttonname);
            return Json(new { total = gp.TotalCount, rows = buttonDtos }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        [HttpGet]
        public ActionResult Add()
        {
            //ViewData["ParentList"] = GetParentSelectList("-1", "请选择");
            return View();
        }

        ///// <summary>
        ///// 父级菜单
        ///// </summary>
        ///// <param name="emptyKey"></param>
        ///// <param name="emptyValue"></param>
        ///// <param name="isAll"></param>
        ///// <returns></returns>
        //public List<SelectListItem> GetParentSelectList(string emptyKey, string emptyValue, bool isAll = false)
        //{
        //    List<SelectListItem> listItems = new List<SelectListItem>();
        //    if (emptyKey != null)
        //    {
        //        listItems.Add(new SelectListItem { Text = emptyValue, Value = emptyKey });
        //    }
        //    List<ButtonDto> lists = Service.GetParentList();
        //    foreach (var item in lists)
        //    {
        //        listItems.Add(new SelectListItem { Text = item.ButtonName, Value = item.Id.ToString() });
        //    }
        //    return listItems;
        //}


        [HttpPost]
        public ActionResult Add(ButtonDto buttonDto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    buttonDto.CreateUser = ServiceHelper.GetCurrentUser().UserName;
                    buttonDto.CredateTime = DateTime.Now;
                    buttonDto.UpdateUser = ServiceHelper.GetCurrentUser().UserName;
                    buttonDto.UpdateTime = DateTime.Now;
                }
                if (Service.Add(buttonDto))
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
            //ViewBag.ParentList = Service.GetParentList("", "").ToSelectList(entity.ParentId);
            ViewData.Model = entity;
            return View();
        }
        [HttpPost]
        public ActionResult Edit(ButtonDto buttonDto)
        {
            ResultJsonInfo resultJsonInfo = new ResultJsonInfo() { Data = "", ErrorMsg = "", Success = true };
            try
            {
                if (ModelState.IsValid)
                {
                    buttonDto.UpdateUser = ServiceHelper.GetCurrentUser().UserName;
                    buttonDto.UpdateTime = DateTime.Now;
                    if (Service.Update(buttonDto))
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