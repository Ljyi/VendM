using System.ComponentModel.DataAnnotations;

namespace VendM.Model.DataModelDto
{
    public class VideoDto : BaseDto
    {
        /// <summary>
        /// 新建实体时使用
        /// </summary>
        /// <returns>返回新建的实体</returns>
        public static VideoDto New()
        {
            VideoDto video = new VideoDto()
            {
            };
            return video;
        }
        /// <summary>
        /// 视频链接
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string VideoUrl { get; set; }
        /// <summary>
        /// 状态（删除，更新，添加）
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public int AdvertisementId { get; set; }
    }
}
