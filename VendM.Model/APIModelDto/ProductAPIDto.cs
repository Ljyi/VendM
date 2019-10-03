using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VendM.Model.APIModelDto
{
    /// <summary>
    /// 产品信息
    /// </summary>
    public class ProductAPIDto
    {
        #region 需持久化属性
        /// <summary>
        /// 产品Id
        /// </summary>
        [Required]
        public int Id { get; set; }
        /// <summary>
        /// 产品编码
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string ProductCode { get; set; }
        /// <summary>
        /// 产品英文名称
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string ProductName_EN { get; set; }
        /// <summary>
        /// 产品中文名称
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string ProductName_CH { get; set; }
        /// <summary>
        /// 产品规格（中文）
        /// </summary>
        public string Specification_CH { get; set; }
        /// <summary>
        /// 产品规格（英文）
        /// </summary>
        public string Specification_EN { get; set; }
        /// <summary>
        /// 产品描述
        /// </summary>
        [Required]
        [MaxLength(500)]
        public string ProductDetails_EN { get; set; }
        /// <summary>
        /// 中文描述
        /// </summary>
        [Required]
        [MaxLength(500)]
        public string ProductDetails_CH { get; set; }
        /// <summary>
        /// 产品图片
        /// </summary>
        public string ImageUrl { get; set; }
        /// <summary>
        /// 产品Id(第三方ID)
        /// </summary>
        public string ProductVId { get; set; }
        /// <summary>
        /// 价格
        /// </summary>
        public List<ProductPriceAPIDto> SalePriceType { get; set; }
        // <summary>
        // 通道编号
        // </summary>
        //   [Required]
        //   public int PassageNumber { get; set; }
        // <summary>
        // 库存数量
        // </summary>
        //   [Required]
        //   public int InventoryQuantity { get; set; }
        #endregion
    }
    /// <summary>
    /// 价格类型
    /// </summary>
    public class ProductPriceAPIDto
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
        /// 是否有效
        /// </summary>
        public bool IsEffective { get; set; }
        /// <summary>
        /// 第三方销售方式
        /// </summary>
        public string ProductWayVId { get; set; }
    }
}
