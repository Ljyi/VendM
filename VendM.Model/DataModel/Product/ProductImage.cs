using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VendM.Model.DataModel.Product
{
    [Table("ProductImage")]
    public class ProductImage : BaseModel
    {
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
        public virtual Product Product { get; set; }
    }
}
