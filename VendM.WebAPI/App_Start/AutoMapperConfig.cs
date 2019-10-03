using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VendM.WebAPI.ModelAutoMapper;

namespace VendM.WebAPI.App_Start
{
    public class AutoMapperConfig
    {
        public static void Config()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile<AdvertMappingProfile>();
                cfg.AddProfile<ProductMappingProfile>();
                cfg.AddProfile<MachineDetailMappingProfile>();
            });
        }
    }
}