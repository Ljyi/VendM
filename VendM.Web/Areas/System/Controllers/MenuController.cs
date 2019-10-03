using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using VendM.Core;
using VendM.Model.DataModel;
using VendM.Model.DataModelDto;
using VendM.Service;
using VendM.Web.Authorization;
using VendM.Web.BaseApplication;
using VendM.Web.Models;


namespace VendM.Web.Areas.System.Controllers
{
    public class MenuController : ServicedController<MenuService>
    {
        // GET: System/Menu
        public ActionResult Index()
        {
            return View();
        }
        #region 列表(Grid)
        /// <summary>
        /// Menu列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetMenuGrid(int limit, int offset, string sort, string sortOrder, string menucode, string menuname, int? parentId)
        {
            TablePageParameter gp = new TablePageParameter() { Limit = limit, Offset = offset, SortName = sort, SortOrder = sortOrder };
            List<SysMenuDto> menuDtos = Service.GetMenuGrid(gp, menucode, menuname, parentId);
            return Json(new { total = gp.TotalCount, rows = menuDtos }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 是否存在子节点
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetMenuSubNode(int menuId)
        {
            ResultJsonInfo resultJsonInfo = new ResultJsonInfo() { Data = "", ErrorMsg = "", Success = true };
            try
            {
                resultJsonInfo.Success = Service.IsHasSubNode(menuId);
            }
            catch (Exception)
            {
                throw;
            }
            return Json(resultJsonInfo, JsonRequestBehavior.AllowGet);
        }
        #endregion

        [HttpGet]
        public ActionResult Add(int? pId)
        {
            ViewData["ParentList"] = GetParentSelectList("-1", "一级菜单", pId);
            return View();
        }

        /// <summary>
        /// 父级菜单
        /// </summary>
        /// <param name="emptyKey"></param>
        /// <param name="emptyValue"></param>
        /// <param name="isAll"></param>
        /// <returns></returns>
        public List<SelectListItem> GetParentSelectList(string emptyKey, string emptyValue, int? selectId)
        {
            List<SelectListItem> listItems = new List<SelectListItem>();
            if (emptyKey != null)
            {
                listItems.Add(new SelectListItem { Text = emptyValue, Value = emptyKey });
            }
            List<SysMenu> lists = Service.GetParentList();
            foreach (var item in lists)
            {
                if (selectId.HasValue && selectId.Value == item.Id)
                {
                    listItems.Add(new SelectListItem { Text = item.MenuName, Value = item.Id.ToString(), Selected = true });
                }
                else
                {
                    listItems.Add(new SelectListItem { Text = item.MenuName, Value = item.Id.ToString() });
                }
            }
            return listItems;
        }
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="sysMenu"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Add(SysMenu sysMenu)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (sysMenu.ParentId < 0)
                    {
                        sysMenu.ParentId = 0;
                    }
                    sysMenu.CreateUser = ServiceHelper.GetCurrentUser().UserName;
                    sysMenu.CredateTime = DateTime.Now;
                    sysMenu.UpdateUser = ServiceHelper.GetCurrentUser().UserName;
                    sysMenu.UpdateTime = DateTime.Now;
                }
                if (Service.Add(sysMenu))
                {
                    return this.Ajax_JGClose();
                }
                return View();
            }
            catch (Exception ex)
            {
                @ViewBag.errorMsg = ex.Message;
                return View();
            }
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var entity = Service.Find(id);
            ViewBag.ParentList = Service.GetParentList("", "").ToSelectList(entity.ParentId);
            ViewData.Model = entity;
            return View();
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="sysMenu"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Edit(SysMenuDto sysMenu)
        {
            ResultJsonInfo resultJsonInfo = new ResultJsonInfo() { Data = "", ErrorMsg = "", Success = true };
            List<string> errorMsg = new List<string>();
            try
            {
                if (ModelState.IsValid)
                {
                    sysMenu.UpdateUser = ServiceHelper.GetCurrentUser().UserName;
                    sysMenu.UpdateTime = DateTime.Now;
                    if (Service.Update(sysMenu))
                    {
                        return this.Ajax_JGClose();
                    }
                    else
                    {
                        errorMsg.Add("更新失败");
                    }
                }
                else
                {

                    foreach (var key in ModelState.Keys)
                    {
                        var modelstate = ModelState[key];
                        if (modelstate.Errors.Any())
                        {
                            errorMsg.Add(modelstate.Errors.FirstOrDefault().ErrorMessage);
                        }
                    }
                }
                var entity = Service.Find(sysMenu.Id);
                ViewBag.ParentList = Service.GetParentList("", "").ToSelectList(entity.ParentId);
                ViewData.Model = entity;
                @ViewBag.errorMsg = errorMsg.ToSeparateString();
                return View();
            }
            catch (Exception ex)
            {
                @ViewBag.errorMsg = ex.Message;
                return View();
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