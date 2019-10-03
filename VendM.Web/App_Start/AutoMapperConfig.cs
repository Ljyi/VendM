using AutoMapper;
using VendM.Web.ModelAutoMapper;

namespace VendM.Web.App_Start
{
    public class AutoMapperConfig
    {
        public static void Config()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile<SystemProfile>();
                cfg.AddProfile<BasicsMapping>();
                cfg.AddProfile<AdvertMappingProfile>();
                cfg.AddProfile<ProductMapping>();
                cfg.AddProfile<OrderMapping>();
                cfg.AddProfile<StockMapping>();
            });
        }
    }
}