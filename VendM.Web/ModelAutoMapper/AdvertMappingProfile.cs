using AutoMapper;
using VendM.Core;
using VendM.Model.DataModel;
using VendM.Model.DataModelDto;
using VendM.Model.EnumModel;

namespace VendM.Web.ModelAutoMapper
{
    public class AdvertMappingProfile : Profile
    {
        public AdvertMappingProfile()
        {
            CreateMap<AdvertisementDto, Advertisement>();
            CreateMap<Advertisement, AdvertisementDto>()
                .ForMember(opt => opt.Videos, ptd => ptd.MapFrom(src => src.Videos))
                .ForMember(opt => opt.StatusName, ptd => ptd.MapFrom(src => EnumExtension.GetDescription(typeof(AdvertisementEnum), src.Status)));
        }
    }
}