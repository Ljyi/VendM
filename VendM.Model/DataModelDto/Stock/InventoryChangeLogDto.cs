using System.ComponentModel.DataAnnotations;

namespace VendM.Model.DataModelDto
{
    /// <summary>
    /// 
    /// </summary>
    public class InventoryChangeLogDto : BaseDto
    {
        /// <summary>
        /// 产品ID
        /// </summary>
        [Required]
        public int ProductId { get; set; }

        /// <summary>
        /// 产品ID
        /// </summary>
        [Required]
        public string ProductName { get; set; }

        /// <summary>
        /// 变更类型（添加库存，扣减库存）
        /// </summary>
        [Required]
        public int ChangeType { get; set; }

        /// <summary>
        /// 变更类型名称（添加库存，扣减库存）
        /// </summary>
        [Required]
        public string ChangeTypeName { get; set; }
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
    }
}
