using System.Web.Mvc;

namespace VendM.Web.Areas.Basics
{
    public class BasicsAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Basics";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Basics_default",
                "Basics/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                new string[] { "VendM.Web.Areas.Basics.Controllers" }
            );
        }
    }
}