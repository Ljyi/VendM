using AutoMapper;
using VendM.Model.DataModel;
using VendM.Model.DataModelDto;

namespace VendM.Web.ModelAutoMapper
{
    public class SystemProfile : Profile
    {
        public SystemProfile()
        {
            CreateMap<User, UserDto>();
            CreateMap<Role, RoleDto>()
                .ForMember(opt => opt.Id, ptd => ptd.MapFrom(src => src.Id))
                .ForMember(opt => opt.RoleName, ptd => ptd.MapFrom(src => src.RoleName))
                .ForMember(opt => opt.Status, ptd => ptd.MapFrom(src => src.Status));
            CreateMap<SysButton, ButtonDto>()
                .ForMember(opt => opt.Id, ptd => ptd.MapFrom(src => src.Id))
                .ForMember(opt => opt.ButtonName, ptd => ptd.MapFrom(src => src.ButtonName))
                .ForMember(opt => opt.ButtonIocn, ptd => ptd.MapFrom(src => src.ButtonIocn))
                .ForMember(opt => opt.ButtonCode, ptd => ptd.MapFrom(src => src.ButtonCode))
                .ForMember(opt => opt.InputType, ptd => ptd.MapFrom(src => src.InputType))
                .ForMember(opt => opt.Status, ptd => ptd.MapFrom(src => src.Status))
                .ForMember(opt => opt.ButtonStyle, ptd => ptd.MapFrom(src => src.ButtonStyle));

            CreateMap<SysMenu, SysMenuDto>();
            CreateMap<SysMenuDto, SysMenu>();
        }
    }
}
