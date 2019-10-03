using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VendM.Model.DataModel
{
    /// <summary>
    /// 库存变更日志
    /// </summary>
    [Table("InventoryChangeLog")]
    public class InventoryChangeLog : BaseModel
    {
        /// <summary>
        /// 产品ID
        /// </summary>
        [Required]
        public int ProductId { get; set; }

        /// <summary>
        /// 变更类型（添加库存，扣减库存）
        /// </summary>
        [Required]
        public int ChangeType { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        [Required]
        public int Quantity { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        [Required]
        [MaxLength(200)]
        public string Content { get; set; }

        public virtual Product.Product Product { get; set; }

    }
}
