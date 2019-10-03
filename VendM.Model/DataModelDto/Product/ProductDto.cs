using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using VendM.Model.DataModel.Product;

namespace VendM.Model.DataModelDto.Product
{
    /// <summary>
    /// 实体
    /// </summary>
    public class ProductDto : BaseDto
    {
        /// <summary>
        /// 新建实体时使用
        /// </summary>
        /// <returns>返回新建的实体</returns>
        public static ProductDto New()
        {
            ProductDto product = new ProductDto()
            {
            };
            return product;
        }
        #region 需持久化属性
        [Required]
        public int StoreId { get; set; }
        public string StoreName { get; set; }
        /// <summary>
        /// 产品编码
        /// </summary>
        [MaxLength(50)]
        public string ProductCode { get; set; }
        /// <summary>
        /// 产品英文名称
        /// </summary>
        [MaxLength(100)]
        public string ProductName_EN { get; set; }
        /// <summary>
        /// 产品中文名称
        /// </summary>
        [MaxLength(100)]
        public string ProductName_CH { get; set; }
        /// <summary>
        /// 产品规格（中文）
        /// </summary>
        [MaxLength(500)]
        public string Specification_CH { get; set; }
        /// <summary>
        /// 产品规格（英文）
        /// </summary>
        [MaxLength(500)]
        public string Specification_EN { get; set; }
        /// <summary>
        /// 全价格
        /// </summary>
        [Required]
        public decimal ProductPrice { get; set; }
        /// <summary>
        /// 全积分
        /// </summary>
        [Required]
        public int AllIntegral { get; set; }
        /// <summary>
        /// 积分
        /// </summary>
        [Required]
        public int PartIntegral { get; set; }
        /// <summary>
        /// 价格
        /// </summary>
        [Required]
        public decimal PartIPrice { get; set; }
        /// <summary>
        /// 价格积分
        /// </summary>
        public string PriceAndIntegral { get; set; }
        /// <summary>
        /// 英文描述
        /// </summary>
        [MaxLength(500)]
        public string ProductDetails_EN { get; set; }
        /// <summary>
        /// 中文描述
        /// </summary>
        [MaxLength(500)]
        public string ProductDetails_CH { get; set; }
        /// <summary>
        /// 产品Id(第三方ID)
        /// </summary>
        public string ProductVId { get; set; }
        /// <summary>
        /// 产品分类
        /// </summary>
        public int? ProductCategoryId { get; set; }
        public virtual ProductCategory ProductCategory { get; set; }
        [MaxLength(500)]
        public string PhotoUrl { get; set; }
        public List<string> ListImage { get; set; }
        /// <summary>
        /// 产品图片
        /// </summary>
        public virtual ICollection<ProductImgeDto> ProductImages { get; set; }

        /// <summary>
        /// 购买方式
        /// </summary>
        public List<string> ProductPriceStrLis { get; set; }

        /// <summary>
        /// 产品价格
        /// </summary>
        public virtual ICollection<ProductPriceDto> ProductPrices { get; set; }
        #endregion
    }
}
