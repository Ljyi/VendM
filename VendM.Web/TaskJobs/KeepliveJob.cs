using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using VendM.Core.Utils;

namespace VendM.Web.TaskJobs
{
    public class KeepliveJob
    {
        /// <summary>
        /// 防止IIS回收
        /// </summary>
        /// <param name="context"></param>
        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                string url = WebConfigurationManager.AppSettings["VendMUrl"];
                new WebClient
                {
                    Encoding = Encoding.UTF8
                }.DownloadString(url);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}