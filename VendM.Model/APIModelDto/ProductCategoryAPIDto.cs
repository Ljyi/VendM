using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using VendM.Model.DataModel.Product;

namespace VendM.Model.APIModelDto
{
    public class ProductCategoryAPIDto
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
    }
}
