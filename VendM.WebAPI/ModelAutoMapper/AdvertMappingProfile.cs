using AutoMapper;
using System;
using System.Configuration;
using VendM.Model.APIModelDto;
using VendM.Model.DataModel;
using VendM.Model.DataModel.Basics;
using VendM.Model.DataModel.Product;

namespace VendM.WebAPI.ModelAutoMapper
{
    public class AdvertMappingProfile : Profile
    {
        public AdvertMappingProfile()
        {
            string httplink = ConfigurationManager.AppSettings["HttpUrl"].Trim();
            CreateMap<VideoAPIDto, Video>();
            CreateMap<Video, VideoAPIDto>().ForMember(model => model.VideoUrl, option => option.MapFrom(md => httplink + md.VideoUrl));
            CreateMap<AdvertisementAPIDto, Advertisement>();
            CreateMap<Advertisement, AdvertisementAPIDto>().ForMember(model => model.StartTime, option => option.MapFrom(md => GetTimeStamp(md.StartTime)))
                .ForMember(model => model.EndTime, option => option.MapFrom(md => GetTimeStamp(md.EndTime)));
        }

        private string GetTimeStamp(DateTime startTime)
        {
            DateTime dateStart = new DateTime(1970, 1, 1, 8, 0, 0);
            return Convert.ToInt32((startTime - dateStart).TotalSeconds).ToString();
        }
    }

    public class ProductMappingProfile : Profile
    {
        public ProductMappingProfile()
        {
            CreateMap<ProductAPIDto, Product>();
            CreateMap<Product, ProductAPIDto>();
            //   CreateMap<ProductImageAPIDto, ProductImage>();
            //    CreateMap<ProductImage, ProductImageAPIDto>();
            CreateMap<ProductCategoryAPIDto, ProductCategory>();
            CreateMap<ProductCategory, ProductCategoryAPIDto>();
        }
    }

    public class MachineDetailMappingProfile : Profile
    {
        public MachineDetailMappingProfile()
        {
            CreateMap<MachineDetailAPIDto, MachineDetail>();
            CreateMap<MachineDetail, MachineDetailAPIDto>();
            CreateMap<MachineDetailAPIDto, MachineDetailAPIOutput>()
                .ForMember(model => model.PassageNumber, option => option.MapFrom(md => md.PassageNumber))
                .ForMember(model => model.MachineNo, option => option.MapFrom(md => md.MachineId))
                // .ForMember(model => model.ProductImage, option => option.MapFrom(md => md.Product.ProductImages))
                .ForMember(model => model.ProductName, option => option.MapFrom(md => md.Product.ProductName_CH))
                .ForMember(model => model.ProductDetails, option => option.MapFrom(md => md.Product.ProductDetails_CH));
            CreateMap<MachineStockDetails, MachineStockDetailsAPIDto>()
                .ForMember(model => model.PassageNumber, option => option.MapFrom(md => md.PassageNumber))
                .ForMember(model => model.ProductName, option => option.MapFrom(md => md.Product.ProductName_CH))
                .ForMember(model => model.ProductImage, option => option.MapFrom(md => md.Product.ProductImages))
                .ForMember(model => model.ProductInventoryQuantity,
                    option => option.MapFrom(md => md.InventoryQuantity))
                .ForMember(model => model.TotalQuantity, option => option.MapFrom(md => md.TotalQuantity));
        }
    }
    public class PayMentMappingProfile : Profile
    {
        public PayMentMappingProfile()
        {
            CreateMap<PayMent, PayMentAPIDto>();
        }
    }
}