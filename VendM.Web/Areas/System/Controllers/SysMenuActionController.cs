using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using VendM.Core;
using VendM.Model.DataModelDto;
using VendM.Service;
using VendM.Web.Authorization;
using VendM.Web.BaseApplication;
using VendM.Web.Models;

namespace VendM.Web.Areas.System.Controllers
{
    public class SysMenuActionController : ServicedController<SysMenuActionService>
    {
        // GET: System/SysMenuAction
        public ActionResult Index()
        {
            return View();
        }
        #region 列表(Grid)
        /// <summary>
        /// SysMenuAction列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetSysMenuActionGrid(int limit, int offset, string sort, string sortOrder, int? menuId, string buttonName, string buttonCode)
        {
            TablePageParameter gp = new TablePageParameter() { Limit = limit, Offset = offset, SortName = sort, SortOrder = sortOrder };
            List<SysMenuActionGridDto> buttonDtos = Service.GeSysMenuActionGrid(gp, menuId, buttonName, buttonCode);
            return Json(new { total = gp.TotalCount, rows = buttonDtos }, JsonRequestBehavior.AllowGet);
        }
        #endregion
        [HttpGet]
        public ActionResult Add(int menuId)
        {
            ViewBag.ButtonList = Service.buttonService.GetButtonList("-1", "请选择").ToSelectList();
            var menu = Service.menuService.Find(menuId);
            ViewBag.ControlName = "";
            ViewBag.ControlCode = "";
            ViewBag.SysMenuId = "";
            if (menu != null)
            {
                ViewBag.ControlName = menu.Url;
                ViewBag.ControlCode = menu.MenuCode;
                ViewBag.SysMenuId = menu.Id;
            }
            return View();
        }  /// <summary>
           /// 添加
           /// </summary>
           /// <param name="sysMenu"></param>
           /// <returns></returns>
        [HttpPost]
        public JsonResult Add(SysMenuActionDto sysMenuActionDto)
        {
            ResultJsonInfo resultJsonInfo = new ResultJsonInfo() { Data = "", ErrorMsg = "", Success = true };
            try
            {
                List<string> errorMsg = new List<string>();
                if (ModelState.IsValid)
                {
                    sysMenuActionDto.CreateUser = ServiceHelper.GetCurrentUser().UserName;
                    sysMenuActionDto.CredateTime = DateTime.Now;
                    sysMenuActionDto.UpdateUser = ServiceHelper.GetCurrentUser().UserName;
                    sysMenuActionDto.UpdateTime = DateTime.Now;
                    resultJsonInfo.Success = Service.SysMenuActionAdd(sysMenuActionDto);
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
                    resultJsonInfo.Success = false;
                    resultJsonInfo.ErrorMsg = errorMsg.ToString();
                }
            }
            catch (Exception ex)
            {
                resultJsonInfo.ErrorMsg = ex.Message;
                resultJsonInfo.Success = false;
            }
            return Json(resultJsonInfo, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var entity = Service.GetSysMenuActionById(id);
            ViewBag.ButtonList = Service.buttonService.GetButtonList("请选择", "-1").ToSelectList(entity.SysButtonId);
            ViewData.Model = entity;
            var menu = Service.menuService.Find(entity.SysMenuId);
            ViewBag.ControlName = "";
            ViewBag.ControlCode = "";
            ViewBag.SysMenuId = "";
            if (menu != null)
            {
                ViewBag.ControlName = menu.Url;
                ViewBag.ControlCode = menu.MenuCode;
                ViewBag.SysMenuId = menu.Id;
            }
            return View();
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="sysMenu"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Edit(SysMenuActionDto sysMenuActionDto)
        {
            ResultJsonInfo resultJsonInfo = new ResultJsonInfo() { Data = "", ErrorMsg = "", Success = true };
            List<string> errorMsg = new List<string>();
            try
            {
                if (ModelState.IsValid)
                {
                    sysMenuActionDto.UpdateUser = ServiceHelper.GetCurrentUser().UserName;
                    sysMenuActionDto.UpdateTime = DateTime.Now;
                    resultJsonInfo.Success = Service.SysMenuActionUpdate(sysMenuActionDto);
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
                    resultJsonInfo.Success = false;
                }
            }
            catch (Exception ex)
            {
                resultJsonInfo.ErrorMsg = ex.Message;
                resultJsonInfo.Success = false;
            }
            return Json(resultJsonInfo, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Delete(string ids)
        {
            ResultJsonInfo resultJsonInfo = new ResultJsonInfo() { Data = "", ErrorMsg = "", Success = true };
            try
            {
                resultJsonInfo.Success = Service.SysMenuActionDeletes(ids);
            }
            catch (Exception ex)
            {
                resultJsonInfo.Success = false;
                resultJsonInfo.ErrorMsg = ex.Message;
            }
            return Json(resultJsonInfo, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 验证功能是否存在
        /// </summary>
        /// <param name="controlName"></param>
        /// <param name="actionName"></param>
        /// <returns></returns>
        [HttpGet]
        public string ValidateActionName(string controlName, string actionName, int? id)
        {
            return Service.ValidateActionName(controlName, actionName, id).ToString().ToLower();
        }

        /// <summary>
        /// 验证权限编码是否存在
        /// </summary>
        /// <param name="controlName"></param>
        /// <param name="authorizeCode"></param>
        /// <returns></returns>
        [HttpGet]
        public string ValidateAuthorizeCode(string controlName, string authorizeCode, int? id)
        {
            return Service.ValidateAuthorizeCode(controlName, authorizeCode, id).ToString().ToLower();
        }
        /// <summary>
        /// 获取按钮详情
        /// </summary>
        /// <param name="buttonId"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetButtonInfo(int buttonId)
        {
            ResultJsonInfo resultJsonInfo = new ResultJsonInfo() { Data = "", ErrorMsg = "", Success = true };
            try
            {
                var button = Service.buttonService.Find(buttonId);
                resultJsonInfo.Data = JsonConvert.SerializeObject(button);
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