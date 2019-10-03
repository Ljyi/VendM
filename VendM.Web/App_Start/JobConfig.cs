using Quartz;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using VendM.Web.TaskJobs;

namespace VendM.Web.App_Start
{
    public class JobConfig
    {
        private static IScheduler _scheduler;

        public static async void Start()
        {
            _scheduler = await Quartz.Impl.StdSchedulerFactory.GetDefaultScheduler();

            await _scheduler.Start();

            var cronStr = ConfigurationManager.AppSettings["SendEmailJobCron"].Trim();
            IJobDetail sendemailJob = JobBuilder.Create<SendEmailJob>().WithIdentity("SendEmailJob", "MinuteGroup").Build();

            ITrigger sendTrigger = TriggerBuilder.Create()
                .WithIdentity("SendEmailJob", "MinuteGroup")
                .WithCronSchedule(cronStr)
                .ForJob(sendemailJob)
                .Build();

            await _scheduler.ScheduleJob(sendemailJob, sendTrigger);
        }

        public static async void Stop()
        {
            if (_scheduler != null)
            {
                await _scheduler.Shutdown();
            }
        }
    }
}