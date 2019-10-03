using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using VendM.Web.App_Start;

namespace VendM.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AutoMapperConfig.Config();
            //   MiniProfilerEF6.Initialize();
            // Database.SetInitializer<VendMDbContext>(new DatabaseInitialize());
            //启用Job
           // JobConfig.Start();
        }
        protected void Application_BeginRequest()
        {
            //if (Request.IsLocal)
            //{
            //    MiniProfiler.Start();
            //}
        }
        protected void Application_EndRequest()
        {
            // MiniProfiler.Stop();
        }
        protected void Application_End()
        {
            //停止Job
            //JobConfig.Stop();
        }
    }
}
