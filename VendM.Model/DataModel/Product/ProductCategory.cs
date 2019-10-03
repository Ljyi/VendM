using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VendM.Model.DataModel.Product
{
    [Table("ProductCategory")]
    public class ProductCategory : BaseModel
    {
        /// <summary>
        /// 分类中文名称
        /// </summary>
        [Required]
        [MaxLength(200)]
        public string CategoryName_CN { get; set; }
        /// <summary>
        /// 分类英文名称
        /// </summary>
        [Required]
        [MaxLength(200)]
        public string CategoryName_EN { get; set; }
        /// <summary>
        /// Code
        /// </summary>
        [Required]
        [MaxLength(20)]
        public string CategoryCode { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int? SortNumber { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public int? Status { get; set; }
        /// <summary>
        /// 产品分类
        /// </summary>
        public int? ProductCategoryVId { get; set; }
    }
}
