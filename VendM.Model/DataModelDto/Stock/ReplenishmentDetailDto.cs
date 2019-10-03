using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VendM.Model.DataModelDto
{
    /// <summary>
    /// 补货清单明细
    /// </summary>
    public class ReplenishmentDetailDto : BaseDto
    {
        /// <summary>
        /// 产品Id
        /// </summary>
        [Required]
        public int ProductId { get; set; }
        /// <summary>
        /// 产品编号
        /// </summary>
        [Required]
        public string ProductNo { get; set; }
        /// <summary>
        /// 产品名称
        /// </summary>
        [Required]
        public string ProductName { get; set; }
        /// <summary>
        /// 货道号
        /// </summary>
        [Required]
        public int PassageNumber { get; set; }
        /// <summary>
        /// 貨道容量
        /// </summary>
        [Required]
        public int TotalQuantity { get; set; }
        /// <summary>
        /// 库存数量
        /// </summary>
        [Required]
        public int InventoryQuantity { get; set; }
        /// <summary>
        /// 补货状态
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 发货主单Id
        /// </summary>
        [Required]
        public int ReplenishmentId { get; set; }
    }
}
