using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VendM.Model.DataModel
{
    /// <summary>
    /// 库存总表（后期扩展表）
    /// </summary>
    [Table("Stock")]
    public class Stock : BaseModel
    {
        /// <summary>
        /// 库存数量
        /// </summary>
        [Required]
        public int TotalQuantity { get; set; }
        /// <summary>
        /// 商品ID
        /// </summary>
        [Required]
        public int ProductId { get; set; }
        public virtual Product.Product Product { get; set; }
    }
}
