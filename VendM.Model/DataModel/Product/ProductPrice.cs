using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VendM.Model.DataModel.Product
{
    /// <summary>
    /// 产品价格
    /// </summary>
    [Table("ProductPrice")]
    public class ProductPrice : BaseModel
    {
        /// <summary>
        /// 价格
        /// </summary>
        public decimal? Price { get; set; }
        /// <summary>
        /// 积分
        /// </summary>
        public int? Point { get; set; }
        /// <summary>
        /// 销售类型（枚举类型）
        /// </summary>
        [Required]
        public int SaleType { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 第三方销售方式
        /// </summary>
        public string ProductWayVId { get; set; }
        /// <summary>
        /// 产品
        /// </summary>
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
    }
}
