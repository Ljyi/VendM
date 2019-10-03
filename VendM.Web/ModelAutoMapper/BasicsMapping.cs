using AutoMapper;
using VendM.Model.DataModel;
using VendM.Model.DataModel.Basics;
using VendM.Model.DataModelDto;
using VendM.Model.EnumModel;

namespace VendM.Web.ModelAutoMapper
{
    public class BasicsMapping : Profile
    {
        public BasicsMapping()
        {
            //
            CreateMap<Store, StoreDto>()
               .ForMember(opt => opt.Id, ptd => ptd.MapFrom(src => src.Id))
               .ForMember(opt => opt.Name, ptd => ptd.MapFrom(src => src.Name))
               .ForMember(opt => opt.StatusName, ptd => ptd.MapFrom(src => VendM.Core.EnumExtension.GetDescription(typeof(StatusEnum), src.Status)))
               .ForMember(opt => opt.Code, ptd => ptd.MapFrom(src => src.Code))
               .ForMember(opt => opt.Address, ptd => ptd.MapFrom(src => src.Address));

            CreateMap<Machine, MachineDto>()
               .ForMember(opt => opt.Id, ptd => ptd.MapFrom(src => src.Id))
               .ForMember(opt => opt.Name, ptd => ptd.MapFrom(src => src.Name))
               .ForMember(opt => opt.MachineNo, ptd => ptd.MapFrom(src => src.MachineNo))
               .ForMember(opt => opt.StatusName, ptd => ptd.MapFrom(src => VendM.Core.EnumExtension.GetDescription(typeof(StatusEnum), src.Status)))
               .ForMember(opt => opt.Password, ptd => ptd.MapFrom(src => src.Password))
               .ForMember(opt => opt.Address, ptd => ptd.MapFrom(src => src.Address))
               .ForMember(opt => opt.StoreId, ptd => ptd.MapFrom(src => src.StoreId))
               .ForMember(opt => opt.StoreName, ptd => ptd.MapFrom(src => src.Store.Name))
               .ForMember(opt => opt.FaultType, ptd => ptd.MapFrom(src => src.FaultType))
               .ForMember(opt => opt.FaultTypeName, ptd => ptd.MapFrom(src => VendM.Core.EnumExtension.GetDescription(typeof(FaultEnum), src.FaultType)))
               .ForMember(opt => opt.FaultTime, ptd => ptd.MapFrom(src => src.FaultTime))
               .ForMember(opt => opt.HandleTime, ptd => ptd.MapFrom(src => src.HandleTime));


            CreateMap<StoreDto, Store>()
                .ForMember(opt => opt.Id, ptd => ptd.MapFrom(src => src.Id))
                .ForMember(opt => opt.Name, ptd => ptd.MapFrom(src => src.Name))
                .ForMember(opt => opt.Code, ptd => ptd.MapFrom(src => src.Code))
                .ForMember(opt => opt.Address, ptd => ptd.MapFrom(src => src.Address));

            CreateMap<MachineDto, Machine>()
                .ForMember(opt => opt.Id, ptd => ptd.MapFrom(src => src.Id))
                .ForMember(opt => opt.Name, ptd => ptd.MapFrom(src => src.Name))
                .ForMember(opt => opt.MachineNo, ptd => ptd.MapFrom(src => src.MachineNo))
                .ForMember(opt => opt.Password, ptd => ptd.MapFrom(src => src.Password))
                .ForMember(opt => opt.Address, ptd => ptd.MapFrom(src => src.Address))
                .ForMember(opt => opt.StoreId, ptd => ptd.MapFrom(src => src.StoreId))
                .ForMember(opt => opt.FaultType, ptd => ptd.MapFrom(src => src.FaultType))
                .ForMember(opt => opt.FaultTime, ptd => ptd.MapFrom(src => src.FaultTime))
                .ForMember(opt => opt.HandleTime, ptd => ptd.MapFrom(src => src.HandleTime));

            CreateMap<PayMent, PayMentDto>()
               .ForMember(opt => opt.StatusName, ptd => ptd.MapFrom(src => VendM.Core.EnumExtension.GetDescription(typeof(PaymentEnum), src.Status)));

            CreateMap<MachineDetail, MachineDetailDto>()
                .ForMember(opt => opt.Id, ptd => ptd.MapFrom(src => src.Id))
                .ForMember(opt => opt.StoreName, ptd => ptd.MapFrom(src => src.Machine.Store.Name))
                .ForMember(opt => opt.PassageNumber, ptd => ptd.MapFrom(src => src.PassageNumber))
                .ForMember(opt => opt.MachineId, ptd => ptd.MapFrom(src => src.Machine.Id))
                .ForMember(opt => opt.MachineName, ptd => ptd.MapFrom(src => src.Machine.Name))
                .ForMember(opt => opt.MachineNo, ptd => ptd.MapFrom(src => src.Machine.MachineNo))
                .ForMember(opt => opt.ProductId, ptd => ptd.MapFrom(src => src.Product.Id))
                .ForMember(opt => opt.ProductCode, ptd => ptd.MapFrom(src => src.Product.ProductCode))
                .ForMember(opt => opt.ProductName_CH, ptd => ptd.MapFrom(src => src.Product.ProductName_CH));


            CreateMap<MachineDetailDto, MachineDetail>()
                .ForMember(opt => opt.Id, ptd => ptd.MapFrom(src => src.Id))
                .ForMember(opt => opt.PassageNumber, ptd => ptd.MapFrom(src => src.PassageNumber))
                .ForMember(opt => opt.MachineId, ptd => ptd.MapFrom(src => src.MachineId))
                .ForMember(opt => opt.ProductId, ptd => ptd.MapFrom(src => src.ProductId));

            CreateMap<ReplenishmentUser, ReplenishmentUserDto>()
                .ForMember(opt => opt.StatusName, ptd => ptd.MapFrom(src => VendM.Core.EnumExtension.GetDescription(typeof(StatusEnum), src.Status)));

            CreateMap<SysMenuAction, SysMenuActionDto>()
             .ForMember(opt => opt.Id, ptd => ptd.MapFrom(src => src.Id))
             .ForMember(opt => opt.ControlName, ptd => ptd.MapFrom(src => src.ControlName))
             .ForMember(opt => opt.ActionName, ptd => ptd.MapFrom(src => src.ActionName))
             .ForMember(opt => opt.AuthorizeCode, ptd => ptd.MapFrom(src => src.AuthorizeCode))
             .ForMember(opt => opt.SortNumber, ptd => ptd.MapFrom(src => src.SortNumber))
             .ForMember(opt => opt.Status, ptd => ptd.MapFrom(src => src.Status))
             .ForMember(opt => opt.SysMenuId, ptd => ptd.MapFrom(src => src.SysMenuId))
             .ForMember(opt => opt.SysButtonId, ptd => ptd.MapFrom(src => src.SysButtonId));

            CreateMap<SysMenuActionDto, SysMenuAction>()
            .ForMember(opt => opt.Id, ptd => ptd.MapFrom(src => src.Id))
            .ForMember(opt => opt.ControlName, ptd => ptd.MapFrom(src => src.ControlName))
            .ForMember(opt => opt.ActionName, ptd => ptd.MapFrom(src => src.ActionName))
            .ForMember(opt => opt.AuthorizeCode, ptd => ptd.MapFrom(src => src.AuthorizeCode))
            .ForMember(opt => opt.SortNumber, ptd => ptd.MapFrom(src => src.SortNumber))
            .ForMember(opt => opt.Status, ptd => ptd.MapFrom(src => src.Status))
            .ForMember(opt => opt.SysMenuId, ptd => ptd.MapFrom(src => src.SysMenuId))
            .ForMember(opt => opt.SysButtonId, ptd => ptd.MapFrom(src => src.SysButtonId));

            CreateMap<SysMenuAction, SysMenuActionGridDto>()
             .ForMember(opt => opt.Id, ptd => ptd.MapFrom(src => src.Id))
             .ForMember(opt => opt.ButtonName, ptd => ptd.MapFrom(src => src.SysButton.ButtonName))
             .ForMember(opt => opt.ButtonCode, ptd => ptd.MapFrom(src => src.SysButton.ButtonCode))
             .ForMember(opt => opt.ActionName, ptd => ptd.MapFrom(src => src.ActionName))
             .ForMember(opt => opt.AuthorizeCode, ptd => ptd.MapFrom(src => src.AuthorizeCode))
             .ForMember(opt => opt.SortNumber, ptd => ptd.MapFrom(src => src.SortNumber))
             .ForMember(opt => opt.Status, ptd => ptd.MapFrom(src => src.Status))
             .ForMember(opt => opt.SysMenuId, ptd => ptd.MapFrom(src => src.SysMenuId))
             .ForMember(opt => opt.SysButtonId, ptd => ptd.MapFrom(src => src.SysButtonId));
        }
    }
}