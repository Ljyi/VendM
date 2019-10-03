using AutoMapper;
using VendM.Model.DataModel;
using VendM.Model.DataModelDto;
using VendM.Model.EnumModel;

namespace VendM.Web.ModelAutoMapper
{
    public class StockMapping : Profile
    {
        public StockMapping()
        {
            CreateMap<StockDto, Stock>();
            CreateMap<Stock, StockDto>();
            CreateMap<MachineStock, MachineStockDto>();
            CreateMap<MachineStock, MachineStockGridDto>()
            .ForMember(opt => opt.StoreName, ptd => ptd.MapFrom(src => src.Machine.Store.Name))
            .ForMember(opt => opt.MachineNo, ptd => ptd.MapFrom(src => src.Machine.MachineNo))
            .ForMember(opt => opt.MachineAddress, ptd => ptd.MapFrom(src => src.Machine.Address));
            CreateMap<MachineStockDto, MachineStock>();
            CreateMap<MachineStockDetails, MachineStockDetailDto>()
            .ForMember(opt => opt.MachineStockId, ptd => ptd.MapFrom(src => src.MachineStock.Id))
            .ForMember(opt => opt.ProductName, ptd => ptd.MapFrom(src => src.Product.ProductName_CH))
            .ForMember(opt => opt.ProdcutCode, ptd => ptd.MapFrom(src => src.Product.ProductCode))
            .ForMember(opt => opt.LastTime, ptd => ptd.MapFrom(src => src.MachineStock.LastTime));
            // CreateMap<MachineStockDetailDto, MachineStockDetails>();

            CreateMap<InventoryChangeLog, InventoryChangeLogDto>()
            .ForMember(opt => opt.Id, ptd => ptd.MapFrom(src => src.Id))
            .ForMember(opt => opt.ProductName, ptd => ptd.MapFrom(src => src.Product.ProductName_CH))
            .ForMember(opt => opt.ChangeTypeName, ptd => ptd.MapFrom(src => VendM.Core.EnumExtension.GetDescription(typeof(ChangeLogType), src.ChangeType)))
            .ForMember(opt => opt.Quantity, ptd => ptd.MapFrom(src => src.Quantity))
            .ForMember(opt => opt.Content, ptd => ptd.MapFrom(src => src.Content));

            CreateMap<InventoryChangeLogDto, InventoryChangeLog>()
            .ForMember(opt => opt.Id, ptd => ptd.MapFrom(src => src.Id))
            .ForMember(opt => opt.ChangeType, ptd => ptd.MapFrom(src => src.ChangeType))
            .ForMember(opt => opt.Quantity, ptd => ptd.MapFrom(src => src.Quantity))
            .ForMember(opt => opt.Content, ptd => ptd.MapFrom(src => src.Content));
        }
    }
}