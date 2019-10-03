using System.Web.Mvc;

namespace VendM.Web.Areas.AdvertiseMent
{
    public class AdvertiseMentAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "AdvertiseMent";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "AdvertiseMent_default",
                "AdvertiseMent/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                new string[] { "VendM.Web.Areas.AdvertiseMent.Controllers" }
            );
        }
    }
}