using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Giris_Sayfasi
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            ISchedulerFactory schedFact = new StdSchedulerFactory();

            IScheduler sched = schedFact.GetScheduler().Result;
            sched.Start();

            IJobDetail job = JobBuilder.Create<Models.Jobs.jobEPosta>().Build();

            ITrigger trigger = TriggerBuilder.Create()
            .WithIdentity("MailGondermeJob", "Grup1")
            .StartNow()
            .WithSimpleSchedule(x => x
            .WithIntervalInMinutes(1)
            .RepeatForever())
            .Build();

            sched.ScheduleJob(job, trigger);
        }
    }
}
