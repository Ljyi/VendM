using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VendM.Model.DataModel
{
    /// <summary>
    /// 扩展表（预留）
    /// </summary>
    [Table("Transaction")]
    public class Transaction : BaseModel
    {
        /// <summary>
        /// 订单号
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string OrderNo { get; set; }
        /// <summary>
        /// 交易信息
        /// </summary>
        [MaxLength(500)]
        public string TransactionInfo { get; set; }
    }
}
