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
using VendM.Service;
using VendM.Service.Stock;

namespace VendM.Web.TaskJobs
{
    public class SendEmailJob : IJob
    {
        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="context"></param>
        public async Task Execute(IJobExecutionContext context)
        {

            var service = new ReplenishmentService();
            await Task.Run(() =>
            {
                try
                {
                    service.ReplenishmentEmail();
                }
                catch (Exception ex)
                {
                    throw;
                }
            });
        }
    }
}