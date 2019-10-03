using System.Collections.Generic;

namespace VendM.Model.DataModelDto.Order
{
    /// <summary>
    /// 订单预览
    /// </summary>
    public class OrderView
    {
        /// <summary>
        /// 主单
        /// </summary>
        public OrderDto OrderDto { get; set; }
        /// <summary>
        /// 订单明细
        /// </summary>
        public List<OrderDetailDto> OrderDetailDto { get; set; }
    }
}
