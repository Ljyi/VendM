using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;
using VendM.Core.Utils;
using VendM.Core.Utils.Images;


namespace VendM.Core.Upload
{
    public class UploadStaticService
    {
        public ApiResultModel Invoke(UploadFileModel ufm)
        {
            if (null == ufm)
                return ApiResultModel.Exception(ResultStatus.Error, "stream can not null");
            ufm.uploader = ufm.uploader.ToLower();

            if (ufm.stream.Length <= 0)
                return ApiResultModel.Exception(ResultStatus.Error, "stream can not empty");

            if (string.IsNullOrEmpty(ufm.filename))
                return ApiResultModel.Exception(ResultStatus.Error, "filename can not empty");

            if (!Config.ContainsKey(ufm.uploader))
                return ApiResultModel.Exception(ResultStatus.Error, "uploader not exist");

            //检查文件扩展名是否合法
            if (!CheckFileExt(Config[ufm.uploader].FileExtList, Path.GetExtension(ufm.filename)))
                return ApiResultModel.Exception(ResultStatus.Error, "upload file extension is illegal");

            //检查文件大小是否合法
            if (!CheckFileSize(Config[ufm.uploader], ufm.stream))
                return ApiResultModel.Exception(ResultStatus.Error, "upload file is too large");

            var url = Handle(ufm, Config[ufm.uploader]);
            var ufr = new UploadFileResult() { Url = url };
            return ApiResultModel.Success(JsonConvert.SerializeObject(ufr));
        }

        /// <summary>
        /// 检查是否为合法的上传文件
        /// </summary> 
        /// <param name="allowExt"></param>
        /// <param name="fileExt"></param>
        /// <returns></returns>
        private bool CheckFileExt(List<Item> allowExt, string fileExt)
        {
            //检查合法文件
            if (allowExt.Count > 0)
            {
                return allowExt.Any(item => item.fileExtension.ToLower() == fileExt.Substring(fileExt.LastIndexOf(".", StringComparison.Ordinal) + 1).ToLower());
            }
            return false;
        }

        /// <summary>
        /// 检查文件大小是否合法
        /// </summary>
        /// <param name="config"></param>
        /// <param name="fileSize">文件大小(B)</param>
        private bool CheckFileSize(StaticConfigEntity config, byte[] fileSize)
        {
            return config.fileSize <= 0 || fileSize.Length / 1024 <= config.fileSize;
        }

        private string Handle(UploadFileModel ufm, StaticConfigEntity entity)
        {
            try
            {
                var filename = ufm.filename;
                var path = entity.ftpPath;
                var subdir = "";
                var ftphost = string.Format("{0}:{1}", entity.ftpUrl, entity.ftpPort);

                // 生成子目录
                if (entity.generateTimeSubdirectory)
                {
                    subdir =
                        DateTime.UtcNow.ToString(string.IsNullOrEmpty(entity.subdirectoryModel)
                            ? "yyyyMMdd"
                            : entity.subdirectoryModel);
                    path = PathHelper.MergeUrl(path, subdir);
                }

                // 创建目录
                XFtpHelper.MakeDir(ftphost, path, entity.ftpUsername, entity.ftpPassword);

                // 生成文件名
                if (entity.generateFileName)
                {
                    string ext = Path.GetExtension(filename);
                    filename = string.Format("{0}{1}", Guid.NewGuid().ToString().Replace("-", ""), ext);
                }

                // 上传文件
                XFtpHelper.UpLoadFile(WriteWatermark(ufm.uploader, ufm.filename, ufm.stream), filename,
                    PathHelper.MergeUrl(ftphost, path), entity.ftpUsername, entity.ftpPassword);

                // 返回地址
                var url = PathHelper.MergeUrl(PathHelper.MergeUrl(entity.httpUrl, subdir), filename);
                Logger.Info($"{ ufm.uploader} upload {ufm.filename}, return url {url}");
                return url;
            }
            catch (Exception ex)
            {
                Logger.Error($"{System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName}=>{System.Reflection.MethodBase.GetCurrentMethod().Name} ：{ex.Message}");
                throw;
            }
            finally
            {
                Logger.Debug(string.Format("{0} upload {1} ", ufm.uploader, ufm.filename));
            }
        }

        /// <summary>
        /// 图片水印
        /// </summary>
        /// <param name="uploader"></param>
        /// <param name="fileName"></param>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static byte[] WriteWatermark(string uploader, string fileName, byte[] stream)
        {
            var watermark = Config[uploader].WatermarkList.FirstOrDefault();
            if (watermark != null)
            {
                var type = (WatermarkType)watermark.Type;
                if (type == WatermarkType.Text)
                    return WaterImageManage.DrawWords(stream, watermark.WaterWord, watermark.Alpha, (WatermarkPosition)watermark.Position, fileName,watermark.WordFont,watermark.FontSize);
                else
                    return WaterImageManage.DrawImage(stream, watermark.WaterImgUrl, watermark.Alpha, (WatermarkPosition)watermark.Position, fileName);
            }
            return stream;
        }

        static UploadStaticService()
        {
            Logger.Info("update static file config");
            StaticConfig c = ConfigManager.GetObjectConfig<StaticConfig>(PathHelper.MergePathName(PathHelper.GetConfigPath(), StaticConfigFile));

            Logger.Info("update strategy");
            lock (typeof(UploadStaticService))
            {
                if (Config != null)
                    return;

                Config = new Dictionary<string, StaticConfigEntity>();
                foreach (StaticConfigEntity entity in c.EntityList)
                    Config.Add(entity.name, entity);
            }

            Logger.Info($"load {Config.Count} config(s).");
        }

        static Dictionary<string, StaticConfigEntity> Config { get; set; }

        const string StaticConfigFile = "Static.config";
    }

    public class UploadFileResult
    {
        [JsonProperty("url")]
        public string Url { get; set; }
    }

    public class UploadFileModel
    {
        public string uploader { get; set; }
        public string filename { get; set; }
        public byte[] stream { get; set; }

    }
}