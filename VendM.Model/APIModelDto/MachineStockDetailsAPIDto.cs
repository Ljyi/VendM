using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VendM.Model.DataModel.Basics;
using VendM.Model.DataModel.Product;
using VendM.Model.DataModelDto.Product;

namespace VendM.Model.APIModelDto
{
    /// <summary>
    /// 实体
    /// </summary>
    public class MachineStockDetailsAPIDto 
    {
        #region 需持久化属性

        /// <summary>
        /// 商品ID
        /// </summary>
        public string MachineNo { get; set; }
        /// <summary>
        /// 通道编号
        /// </summary>
        public int PassageNumber { get; set; }
       
        public string ProductName { get; set; }
       
        /// <summary>
        /// 商品库存
        /// </summary>
        public int ProductInventoryQuantity { get; set; }

        public List<ProductImageAPIDto> ProductImage { get; set; }
        /// <summary>
		///	貨道容量
		/// </summary>
        public int TotalQuantity { get; set; }

        /// <summary>
        /// 商品单价
        /// </summary>
        public decimal ProductPrice { get; set; }

        /// <summary>
        /// 商品编码
        /// </summary>
        public string ProductCode { get; set; }
        #endregion
    }
}