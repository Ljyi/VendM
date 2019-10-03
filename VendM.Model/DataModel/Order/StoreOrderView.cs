using System;

namespace VendM.Model.DataModel.Order
{
    /// <summary>
    ///
    /// </summary>
    public class StoreOrderView : BaseModel
    {
        /// <summary>
        /// 订单时间
        /// </summary>
        public DateTime OrderTime { get; set; }
        /// <summary>
        /// Market名称
        /// </summary>
        public string StoreName { get; set; }
        /// <summary>
        /// 机器位置
        /// </summary>
        public string MachineNo { get; set; }
        /// <summary>
        /// 机器位置
        /// </summary>
        public string MachineAddress { get; set; }
        /// <summary>
        /// 产品编码
        /// </summary>
        public string ProductCode { get; set; }
        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName { get; set; }
        /// <summary>
        /// 图片地址
        /// </summary>
        public string ImgUrl { get; set; }
        /// <summary>
        /// 订单数量
        /// </summary>
        public int OrderQuantity { get; set; }
        /// <summary>
        /// 金额
        /// </summary>
        public decimal TotalAmount { get; set; }
        /// <summary>
        /// 积分
        /// </summary>
        public decimal TotalPoint { get; set; }
        /// <summary>
        /// Market編號
        /// </summary>
        public string StoreNo { get; set; }
    }
}
