namespace VendM.Model.DataModelDto.Product
{
    /// <summary>
    /// 实体
    /// </summary>
    public class ProductCategoryDto : BaseDto
    {
        /// <summary>
        /// 新建实体时使用
        /// </summary>
        /// <returns>返回新建的实体</returns>
        public static ProductCategoryDto New()
        {
            ProductCategoryDto productCategory = new ProductCategoryDto()
            {
            };
            return productCategory;
        }
        #region 需持久化属性
        /// <summary>
        ///	CategoryName_CN
        /// </summary>
        public string CategoryName_CN { get; set; }
        /// <summary>
        ///	CategoryName_EN
        /// </summary>
        public string CategoryName_EN { get; set; }
        /// <summary>
        ///	CategoryCode
        /// </summary>
        public string CategoryCode { get; set; }
        /// <summary>
        ///	SortNumber
        /// </summary>
        public int? SortNumber { get; set; }
        /// <summary>
        ///	Status
        /// </summary>
        public int? Status { get; set; }
        /// <summary>
        /// 产品分类
        /// </summary>
        public int? ProductCategoryVId { get; set; }
        #endregion
    }
}