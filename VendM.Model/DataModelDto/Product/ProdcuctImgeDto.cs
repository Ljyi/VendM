using VendM.Model.DataModel;

namespace VendM.Model.DataModelDto.Product
{
    public class ProductImgeDto : BaseDto
    {
        /// <summary>
        /// 新建实体时使用
        /// </summary>
        /// <returns>返回新建的实体</returns>
        public static ProductImgeDto New()
        {
            ProductImgeDto productImg = new ProductImgeDto()
            {
            };
            return productImg;
        }
        public string Url { get; set; }
        /// <summary>
        /// 首图
        /// </summary>

        public bool Main { get; set; }
        /// <summary>
        /// 状态
        /// </summary>

        public int Status { get; set; }

        /// <summary>
        /// 产品
        /// </summary>
        public int ProductId { get; set; }

    }
}
