using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using VendM.Core.Utils;


namespace VendM.Core.Upload
{
    public class DeleteStaticService
    {
        public ApiResultModel DeleteFile(DeleteFileModel ufm)
        {
            if (ufm == null)
                return ApiResultModel.Exception(ResultStatus.Error, "data is null");

            if (string.IsNullOrEmpty(ufm.OldUrl) && ufm.OldUrl.Length <= 0)
                return ApiResultModel.Exception(ResultStatus.Error, "can not Url");

            if (!CheckFileExt(Config[ufm.DelName].FileExtList, Path.GetExtension(ufm.OldUrl)))
            {
                Logger.Info("Delete : 文件扩展名不合法");
                return ApiResultModel.Exception(ResultStatus.Error, "文件扩展名不合法");
            }

            var res = Handle(ufm.OldUrl, Config[ufm.DelName]);
            return ApiResultModel.Success(new { delelteState = res });
        }

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="oldUrl">旧图片地址</param>
        /// <param name="entity"></param>
        /// <returns></returns>
        private bool Handle(string oldUrl, DeleteConfigEntity entity)
        {
            try
            {
                Logger.Info($"Delete:{entity}");
                //除去域名的地址
                var path = new Uri(oldUrl).AbsolutePath;
                //ftp域名
                var ftphost = $"{entity.ftpUrl}:{entity.ftpPort}";
                //文件地址
                var filePath = PathHelper.MergeUrl(ftphost, path);


                //检查文件是否存在
                if (FileCheckExist(filePath, entity.ftpUsername, entity.ftpPassword))
                    return DeleteFile(entity.ftpUsername, entity.ftpPassword, filePath);

                return false;
            }
            catch (Exception ex)
            {
                Logger.Error($"Delete Error{ex}");
                return false;
            }
        }

        /// <summary>
        /// 删除Ftp文件
        /// </summary>
        /// <param name="ftpUsername"></param>
        /// <param name="ftpPassword"></param>
        /// <param name="fullPath"></param>
        /// <param name="ftpPath"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private bool DeleteFile(string ftpUsername, string ftpPassword, string fullPath = "", string ftpPath = "", string fileName = "")
        {
            FtpWebResponse respose = null;
            try
            {
                //根据uri创建FtpWebRequest对象    
                FtpWebRequest reqFtp;
                if (string.IsNullOrEmpty(fullPath))
                    reqFtp = (FtpWebRequest)WebRequest.Create(new Uri($@"{ftpPath}{fileName}"));
                else
                    reqFtp = (FtpWebRequest)WebRequest.Create(new Uri(fullPath));


                //提供账号密码的验证    
                reqFtp.Credentials = new NetworkCredential(ftpUsername, ftpPassword);
                //默认为true是上传完后不会关闭FTP连接    
                reqFtp.KeepAlive = false;
                //执行删除操作  
                reqFtp.Method = WebRequestMethods.Ftp.DeleteFile;
                respose = (FtpWebResponse)reqFtp.GetResponse();
            }
            catch (Exception ex)
            {
                Logger.Error($"ftp DeleteFile {ex}");
                return false;
            }
            finally
            {
                //关闭连接跟流  
                respose?.Close();
            }
            return true;
        }

        static DeleteStaticService()
        {
            DeleteStaticConfig delConfig = ConfigManager.GetObjectConfig<DeleteStaticConfig>(PathHelper.MergePathName(PathHelper.GetConfigPath(), StaticConfigFile));
            lock (typeof(DeleteConfigEntity))
            {
                if (Config != null)
                    return;

                Config = new Dictionary<string, DeleteConfigEntity>();
                foreach (DeleteConfigEntity entity in delConfig.EntityList)
                    Config.Add(entity.name, entity);
            }
            Logger.Info(string.Format("load {0} config(s).", Config.Count));
        }

        static Dictionary<string, DeleteConfigEntity> Config { get; set; }

        /// <summary>
        /// 检查是否为合法的上传文件
        /// </summary> 
        /// <param name="allowExt"></param>
        /// <param name="fileExt"></param>
        /// <returns></returns>
        private bool CheckFileExt(List<Extension> allowExt, string fileExt)
        {
            //检查合法文件
            if (allowExt.Count > 0)
            {
                return allowExt.Any(item => item.fileExtension.ToLower() == fileExt.Substring(fileExt.LastIndexOf(".", StringComparison.Ordinal) + 1).ToLower());
            }
            return false;
        }

        const string StaticConfigFile = "Static.config";

        public class DeleteFileModel
        {
            /// <summary>
            /// 文件地址
            /// </summary>
            public string OldUrl { get; set; }
            /// <summary>
            /// 删除类别
            /// </summary>
            public string DelName { get; set; }
        }
        /// <summary>
        /// 检查文件是否存在
        /// </summary>
        /// <param name="ftpFullPath">完整路径</param>
        /// <param name="ftpUser"></param>
        /// <param name="ftpPassword"></param>
        /// <returns></returns>
        public bool FileCheckExist(string ftpFullPath, string ftpUser, string ftpPassword)
        {
            bool success = false;
            var ftpName = Path.GetFileName(ftpFullPath);
            WebResponse webResponse = null;
            StreamReader reader = null;
            try
            {
                var ftpWebRequest = (FtpWebRequest)FtpWebRequest.Create(new Uri(ftpFullPath.Replace(ftpName, "")));
                ftpWebRequest.Credentials = new NetworkCredential(ftpUser, ftpPassword);
                ftpWebRequest.Method = WebRequestMethods.Ftp.ListDirectory;
                ftpWebRequest.KeepAlive = false;
                webResponse = ftpWebRequest.GetResponse();
                reader = new StreamReader(webResponse.GetResponseStream());
                string line = reader.ReadLine();
                while (line != null)
                {
                    if (line == ftpName)
                    {
                        success = true;
                        break;
                    }
                    line = reader.ReadLine();
                }
            }
            catch (Exception)
            {
                success = false;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
                if (webResponse != null)
                {
                    webResponse.Close();
                }
            }
            return success;
        }

    }


}
