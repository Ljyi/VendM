using AutoMapper;
using System.Linq;
using VendM.Model.DataModel.Order;
using VendM.Model.DataModelDto.Order;
using VendM.Model.ExportModel;

namespace VendM.Web.ModelAutoMapper
{
    public class OrderMapping : Profile
    {
        public OrderMapping()
        {
            CreateMap<OrderDto, Order>();
            CreateMap<Order, OrderDto>().ForMember(opt => opt.ProductName, ptd => ptd.MapFrom(src => src.OrderDetails == null ? "" : src.OrderDetails.Count > 0 ? src.OrderDetails.FirstOrDefault().ProductName : ""));
            CreateMap<OrderDetails, OrderDetailDto>();
            CreateMap<OrderDetailDto, OrderDetails>();
            CreateMap<Order, OrderView>().ForMember(opt => opt.OrderDetailDto, ptd => ptd.MapFrom(src => src.OrderDetails));
            //订单明细映射
            CreateMap<OrderDetails, StoreOrderDetailDto>()
            // .ForMember(opt => opt.ProdcuctImge, ptd => ptd.MapFrom(src => src.Product.ProductImages))
            .ForMember(opt => opt.StoreName, ptd => ptd.MapFrom(src => src.Order.StoreName))
            .ForMember(opt => opt.StoreNo, ptd => ptd.MapFrom(src => src.Order.StoreNo))
            .ForMember(opt => opt.MachineNo, ptd => ptd.MapFrom(src => src.Order.MachineNo))
            .ForMember(opt => opt.PayMent, ptd => ptd.MapFrom(src => src.Order.PayMent))
            .ForMember(opt => opt.SaleType, ptd => ptd.MapFrom(src => src.SaleType));
            //订单导出
            CreateMap<Order, OrderExport>()
                .ForMember(opt => opt.订单编号, ptd => ptd.MapFrom(src => src.OrderNo))
                .ForMember(opt => opt.Market编号, ptd => ptd.MapFrom(src => src.StoreNo))
                .ForMember(opt => opt.Market名称, ptd => ptd.MapFrom(src => src.StoreName))
                .ForMember(opt => opt.设备编号, ptd => ptd.MapFrom(src => src.MachineNo))
                .ForMember(opt => opt.支付类型, ptd => ptd.MapFrom(src => src.PayMent.ToString()))
                .ForMember(opt => opt.订单状态, ptd => ptd.MapFrom(src => src.OrderStatus.ToString()))
                .ForMember(opt => opt.数量, ptd => ptd.MapFrom(src => src.Quantity))
                .ForMember(opt => opt.金额, ptd => ptd.MapFrom(src => src.Amount))
                .ForMember(opt => opt.积分, ptd => ptd.MapFrom(src => src.Points));
        }
    }
}