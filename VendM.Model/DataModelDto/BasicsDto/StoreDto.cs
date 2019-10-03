
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VendM.Model.DataModelDto;

namespace VendM.Model.DataModelDto
{
    /// <summary>
    /// 实体
    /// </summary>
    public class StoreDto : BaseDto
    {
        /// <summary>
        /// 新建实体时使用
        /// </summary>
        /// <returns>返回新建的实体</returns>
        public static StoreDto New()
        {
            StoreDto store = new StoreDto()
            {
            };
            return store;
        }
        #region 需持久化属性
        /// <summary>
		///	Name
		/// </summary>
        public string Name { get; set; }
        /// <summary>
		///	Code
		/// </summary>
        public string Code { get; set; }
        /// <summary>
		///	Status
		/// </summary>
        public int Status { get; set; }


        /// <summary>
        /// 状态名称
        /// </summary>
        public string StatusName { get; set; }

        /// <summary>
		///	Address
		/// </summary>
        public string Address { get; set; }
        #endregion
    }
}