using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VendM.Model.DataModel.Basics
{
    /// <summary>
    /// 支付类型
    /// </summary>
    [Table("PayMent")]
    public class PayMent : BaseModel
    {
        /// <summary>
        /// 支付名称
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string PaymentName { get; set; }
        /// <summary>
        /// 支付Code
        /// </summary>
        public string PaymentCode { get; set; }
        /// <summary>
        /// 费率
        /// </summary>
        [MaxLength(20)]
        public string Charge { get; set; }
        /// <summary>
        /// 商家费率
        /// </summary>
        public string Fee { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }
    }
}
