namespace VendM.Model.APIModelDto
{
    /// <summary>
    /// 支付类型
    /// </summary>
    public class PayMentAPIDto
    {
        /// <summary>
        /// 支付Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 支付名称
        /// </summary>
        public string PaymentName { get; set; }
        /// <summary>
        /// 支付Code
        /// </summary>
        public string PaymentCode { get; set; }

        /// <summary>
        /// 状态，0表示禁用，1表示使用中
        /// </summary>
        public int Status { get; set; }

    }
}