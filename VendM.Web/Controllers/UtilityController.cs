using System;
using System.IO;
using System.Web.Mvc;
using Newtonsoft.Json;
using VendM.Core.Upload;

namespace VendM.Web.Controllers
{
    /// <summary>
    /// 公共控制器
    /// </summary>
    public class UtilityController : Controller
    {
        /// <summary>
        /// 上傳圖片
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UploadImage()
        {
            var req = HttpContext.Request;
            var res = new ApiResultModel();
            try
            {
                if (req.Files.Count > 0)
                {
                    var uploadData = new UploadFileModel();
                    foreach (var item in req.Files.AllKeys)
                    {
                        var fileData = req.Files[item];
                        uploadData.filename = fileData.FileName;
                        if (string.IsNullOrEmpty(fileData.FileName) || fileData.ContentLength <= 0)
                            continue;

                        var stream = fileData.InputStream;
                        byte[] bytes = new byte[stream.Length];
                        stream.Read(bytes, 0, bytes.Length);
                        // 设置当前流的位置为流的开始
                        stream.Seek(0, SeekOrigin.Begin);
                        uploadData.stream = bytes;
                        uploadData.uploader = "tahuhu";
                        res = new UploadStaticService().Invoke(uploadData);
                    }
                }
            }
            catch (Exception ex)
            {

                throw;
            }
            return Content(JsonConvert.SerializeObject(res));
        }
    }
}