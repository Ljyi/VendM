using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VendM.Model.DataModel
{
    /// <summary>
    /// 机器库存明细
    /// </summary>
    [Table("MachineStockDetails")]
    public class MachineStockDetails : BaseModel
    {
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
        /// 通道编号
        /// </summary>
        [Required]
        public int PassageNumber { get; set; }
        /// <summary>
        /// 商品ID
        /// </summary>
        public int? ProductId { get; set; }
        public virtual Product.Product Product { get; set; }
        /// <summary>
        /// 机器ID
        /// </summary>
        [Required]
        public int MachineStockId { get; set; }
        public virtual MachineStock MachineStock { get; set; }
    }
}
