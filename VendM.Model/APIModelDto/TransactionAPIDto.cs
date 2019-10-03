using System.ComponentModel.DataAnnotations;

namespace VendM.Model.APIModelDto
{
    /// <summary>
    /// 交易
    /// </summary>
    public class TransactionAPIDto
    {
        /// <summary>
        /// 设备编号
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string MachineNo { get; set; }
        /// <summary>
        /// 会有卡号
        /// </summary>
        public string CardNo { get; set; }
        /// <summary>
        /// 产品VId
        /// </summary>
        public string ProductVId { get; set; }
        /// <summary>
        /// 销售类型
        /// </summary>
        public string ProductPayId { get; set; }
    }
    /// <summary>
    /// 交易确认
    /// </summary>
    public class TransactionCompleteAPIDto
    {
        /// <summary>
        /// 设备编号
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string MachineNo { get; set; }
        /// <summary>
        /// 会有卡号
        /// </summary>
        public string CardNo { get; set; }
        /// <summary>
        /// 交易Id
        /// </summary>
        public string TransactionId { get; set; }
        /// <summary>
        /// 是否交易成功
        /// </summary>
        public bool IsSuccess { get; set; }
    }
    /// <summary>
    /// 交易数据
    /// </summary>
    public class TransactionRepsoneAPIDto
    {
        /// <summary>
        /// 交易Id
        /// </summary>
        public string TransactionId { get; set; }
    }

}
