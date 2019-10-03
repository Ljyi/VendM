namespace VendM.Model.ExportModel
{
    /// <summary>
    /// 导出模板
    /// </summary>
    public class OrderExport
    {
        /// <summary>
        /// 订单编号
        /// </summary>
        public string 订单编号 { get; set; }
        /// <summary>
        /// Market编号
        /// </summary>
        public string Market编号 { get; set; }
        /// <summary>
        /// Market名称
        /// </summary>
        public string Market名称 { get; set; }
        /// <summary>
        /// 设备编号
        /// </summary>
        public string 设备编号 { get; set; }
        /// <summary>
        /// 支付类型
        /// </summary>
        public string 支付类型 { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public int 数量 { get; set; }
        /// <summary>
        /// 金额
        /// </summary>
        public decimal? 金额 { get; set; }
        /// <summary>
        /// 积分
        /// </summary>
        public decimal? 积分 { get; set; }
        /// <summary>
        /// 订单状态
        /// </summary>
        public string 订单状态 { get; set; }
    }
}
