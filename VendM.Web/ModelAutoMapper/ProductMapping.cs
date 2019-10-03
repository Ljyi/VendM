using AutoMapper;
using System.Linq;
using VendM.Model.DataModel.Product;
using VendM.Model.DataModelDto.Product;
using VendM.Model.EnumModel;

namespace VendM.Web.ModelAutoMapper
{
    public class ProductMapping : Profile
    {
        public ProductMapping()
        {
            CreateMap<ProductDto, Product>();
            CreateMap<Product, ProductDto>()
            .ForMember(opt => opt.StoreId, ptd => ptd.MapFrom(src => src.StoreId))
            .ForMember(opt => opt.ProductImages, ptd => ptd.MapFrom(src => src.ProductImages))
            .ForMember(opt => opt.ListImage, ptd => ptd.MapFrom(src => src.ProductImages.Select(p => p.Url)))
            .ForMember(opt => opt.ProductPrices, ptd => ptd.MapFrom(src => src.ProductPrices))
            .ForMember(opt => opt.ProductPrice, ptd => ptd.MapFrom(src => src.ProductPrices.FirstOrDefault(it => it.SaleType == (int)SaleTypeEnum.Money).Price))
            .ForMember(opt => opt.AllIntegral, ptd => ptd.MapFrom(src => src.ProductPrices.FirstOrDefault(it => it.SaleType == (int)SaleTypeEnum.Point).Point))
            .ForMember(opt => opt.PartIPrice, ptd => ptd.MapFrom(src => src.ProductPrices.FirstOrDefault(it => it.SaleType == (int)SaleTypeEnum.MoneyAndPoint).Price))
            .ForMember(opt => opt.PartIntegral, ptd => ptd.MapFrom(src => src.ProductPrices.FirstOrDefault(it => it.SaleType == (int)SaleTypeEnum.MoneyAndPoint).Point))
            .ForMember(opt => opt.PriceAndIntegral, ptd => ptd.MapFrom(src => src.ProductPrices.Where(it=>it.SaleType== (int)SaleTypeEnum.MoneyAndPoint).Select(it => "积分："+it.Point.ToString() + "，金额：" + it.Price.ToString()).FirstOrDefault()))
            .ForMember(opt => opt.ProductPriceStrLis, ptd => ptd.MapFrom(src => src.ProductPrices.Where(it => !it.IsDelete && it.Status == (int)ProPriceStatusEnum.Enabled).Select(it => it.SaleType).ToList()));

            CreateMap<ProductImgeDto, ProductImage>();
            CreateMap<ProductImage, ProductImgeDto>();
            CreateMap<ProductPrice, ProductPriceDto>();
            CreateMap<ProductPriceDto, ProductPrice>();
        }

    }
}