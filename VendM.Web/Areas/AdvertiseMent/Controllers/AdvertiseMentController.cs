using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using VendM.Core;
using VendM.Model.DataModelDto;
using VendM.Service;
using VendM.Web.Authorization;
using VendM.Web.BaseApplication;
using VendM.Web.Models;

namespace VendM.Web.Areas.AdvertiseMent.Controllers
{
    public class AdvertiseMentController : ServicedController<AdvertisementService>
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
        /// 列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetAdvertiseMentGrid(int limit, int offset, string sort, string sortOrder, string name)
        {
            TablePageParameter gp = new TablePageParameter() { Limit = limit, Offset = offset, SortName = sort, SortOrder = sortOrder };
            List<AdvertisementDto> advertisementList = Service.GetAdvertisementGrid(gp, name);
            advertisementList.ForEach(p =>
            {
                p.VideoUrl = p.Videos.Count > 0 ? p.Videos.FirstOrDefault().VideoUrl : "";
            });
            return Json(new { total = gp.TotalCount, rows = advertisementList }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 新建
        /// </summary>
        /// <param name="model">实体</param>
        /// <returns>返回操作结果</returns>
        [HttpGet]
        public ActionResult AddAdvertiseMent()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddAdvertiseMent(AdvertisementDto model)
        {
            string errorMsg = "";
            if (ModelState.IsValid)
            {
                model.CreateUser = ServiceHelper.GetCurrentUser().LoginName;
                model.CredateTime = DateTime.Now;
                model.UpdateUser = ServiceHelper.GetCurrentUser().LoginName;
                model.UpdateTime = DateTime.Now;
                if (Service.Add(model, ref errorMsg))
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
        public ActionResult EditAdvertiseMent(int id)
        {
            var entity = Service.Get(id);
            ViewBag.VideoUrl = entity.Videos.Select(p => p.VideoUrl).ToList();
            ViewData.Model = entity;
            return View();
        }
        [HttpPost]
        public ActionResult EditAdvertiseMent(AdvertisementDto model)
        {
            string errorMsg = "";
            if (ModelState.IsValid)
            {
                model.CreateUser = ServiceHelper.GetCurrentUser().LoginName;
                model.CredateTime = DateTime.Now;
                model.UpdateUser = ServiceHelper.GetCurrentUser().LoginName;
                model.UpdateTime = DateTime.Now;
                if (Service.Update(model))
                {
                    return this.Ajax_JGClose();
                }
            }
            else
            {
                var entity = Service.Get(model.Id);
                ViewBag.VideoUrl = entity.Videos.Select(p => p.VideoUrl).ToList();
                ViewData.Model = entity;
            }
            @ViewBag.errorMsg = errorMsg;
            return View();
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <returns>返回操作结果</returns>
        [HttpPost]
        public JsonResult DeleteAdvertiseMent(string ids)
        {
            ResultJsonInfo resultJsonInfo = new ResultJsonInfo() { Data = "", ErrorMsg = "", Success = true };
            try
            {
                resultJsonInfo.Success = Service.DeleteAsync(ids);
            }
            catch (Exception ex)
            {
                resultJsonInfo.Success = false;
                resultJsonInfo.ErrorMsg = ex.Message;
            }
            return Json(resultJsonInfo, JsonRequestBehavior.AllowGet);
        }
        #region  上传文件
        /// <summary>
        /// 上传文件
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UploadReadio()
        {
            string fileName = Request["name"];
            string fileRelName = fileName.Substring(0, fileName.LastIndexOf('.'));//设置临时存放文件夹名称
            int index = Convert.ToInt32(Request["chunk"]);//当前分块序号
            var guid = Request["guid"];//前端传来的GUID号
            var dir = Server.MapPath("~/Upload/Readio");//文件上传目录
            dir = Path.Combine(dir, fileRelName);//临时保存分块的目录
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            string filePath = Path.Combine(dir, index.ToString());//分块文件名为索引名，更严谨一些可以加上是否存在的判断，防止多线程时并发冲突
            var data = Request.Files["file"];//表单中取得分块文件
            data.SaveAs(filePath);//报错
            return Json(new { erron = 0 });//Demo，随便返回了个值，请勿参考
        }
        public ActionResult Merge()
        {
            ResultJsonInfo resultJsonInfo = new ResultJsonInfo() { Data = "", ErrorMsg = "", Success = true };
            try
            {
                var guid = Request["guid"];//GUID
                var uploadDir = Server.MapPath("~/Upload/Readio");//Upload 文件夹
                var fileName = Request["fileName"];//文件名
                string fileRelName = fileName.Substring(0, fileName.LastIndexOf('.'));
                var dir = Path.Combine(uploadDir, fileRelName);//临时文件夹          
                var files = Directory.GetFiles(dir);//获得下面的所有文件
                var finalPath = Path.Combine(uploadDir, fileName);//最终的文件名（demo中保存的是它上传时候的文件名，实际操作肯定不能这样）
                var fs = new FileStream(finalPath, FileMode.Create);
                foreach (var part in files.OrderBy(x => x.Length).ThenBy(x => x))//排一下序，保证从0-N Write
                {
                    var bytes = FileHelper.FileToBytes(part);
                    fs.Write(bytes, 0, bytes.Length);
                    bytes = null;
                    FileHelper.DeleteFile(part);//删除分块
                }
                fs.Flush();
                fs.Close();
                Directory.Delete(dir);//删除文件夹
                string fileNamePath = uploadDir;
                string applictionPath = "";//这里写你的ffmpeg所在的绝对路径
                string targetImgPath = HttpContext.Server.MapPath("~/Uploads") + "\\VideoImages" + "\\" + fileRelName + ".jpg";
                VideoConverToImg.ConverToImg(applictionPath, fileNamePath, targetImgPath);
                resultJsonInfo.Data = "/Upload/Readio/" + Path.GetFileName(finalPath);
            }
            catch (Exception ex)
            {
                resultJsonInfo.Success = false;
                resultJsonInfo.ErrorMsg = ex.Message;
            }
            return Json(resultJsonInfo);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult DeleteReadio(int id, string filename)
        {
            ResultJsonInfo resultJsonInfo = new ResultJsonInfo() { Data = "", ErrorMsg = "", Success = true };
            try
            {
                if (Service.DeleteReadio(id, filename))
                {
                    resultJsonInfo.Success = true;
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

        #region 验证
        /// <summary>
        /// 验证广告名称
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpGet]
        public bool ValidateName(string name, int? id)
        {
            try
            {
                return Service.ValidateName(name, id);
            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion
        #endregion
    }
}