using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VendM.Model.DataModelDto;

namespace VendM.Model.DataModel.Product
{
    /// <summary>
    /// 产品价格
    /// </summary>
    public class ProductPriceDto  : BaseDto
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
        /// 价格
        /// </summary>
        public decimal? PartPrice { get; set; }
        /// <summary>
        /// 积分
        /// </summary>
        public int? PartPoint { get; set; }

        /// <summary>
        /// 销售类型（枚举类型）
        /// </summary>
        [Required]
        public int SaleType { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }

        public int ProductId { get; set; }

    }
}
