using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VendM.Model.DataModel.Basics
{
    [Table("Store")]
    public class Store : BaseModel
    {
        /// <summary>
        /// 商店名称
        /// </summary>
        [MaxLength(100)]
        [Required]
        public string Name { get; set; }
        /// <summary>
        /// MarketId
        /// （CRM同步）
        /// </summary>
        [MaxLength(50)]
        [Required]
        public string Code { get; set; }
        /// <summary>
        /// 商店编号
        /// </summary>
        [Required]
        public int Status { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        [MaxLength(500)]
        [Required]
        public string Address { get; set; }
        /// <summary>
        /// Market
        /// </summary>
        public virtual ICollection<Machine> Machines { get; set; }
    }
}
