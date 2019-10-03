using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VendM.Model.DataModel.Product
{
    /// <summary>
    /// 产品
    /// </summary>
    [Table("Product")]
    public class Product : BaseModel
    {
        [Required]
        public int StoreId { get; set; }
        /// <summary>
        /// 产品编码
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string ProductCode { get; set; }
        /// <summary>
        /// 产品名称（英文）
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string ProductName_EN { get; set; }
        /// <summary>
        /// 产品名称（中文）
        /// </summary>
        [Required]
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
        /// 英文描述
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
        /// 产品Id(第三方ID)
        /// </summary>
        public string ProductVId { get; set; }
        /// <summary>
        /// 产品分类
        /// </summary>
        public int? ProductCategoryId { get; set; }
        public virtual ProductCategory ProductCategory { get; set; }
        /// <summary>
        /// 产品图片
        /// </summary>
        public virtual ICollection<ProductImage> ProductImages { get; set; }
        /// <summary>
        /// 销售类型(扩展字段)
        /// 积分支付,钱支付，积分+钱支付
        /// </summary>
        public virtual ICollection<ProductPrice> ProductPrices { get; set; }
    }

}
