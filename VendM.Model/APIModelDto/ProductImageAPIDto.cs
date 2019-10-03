using System.ComponentModel.DataAnnotations;

namespace VendM.Model.APIModelDto
{
    public class ProductImageAPIDto
    {
        /// <summary>
        /// 图片链接
        /// </summary>
        [Required]
        [MaxLength(200)]
        public string Url { get; set; }
        /// <summary>
        /// 首图
        /// </summary>
        [Required]
        public bool Main { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        [Required]
        public int Status { get; set; }
        /// <summary>
        /// 产品
        /// </summary>
        [Required]
        public int ProductId { get; set; }

    }
}
