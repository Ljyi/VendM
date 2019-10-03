using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VendM.Model.DataModel
{
    /// <summary>
    /// 视频
    /// </summary>
    [Table("Video")]
    public class Video : BaseModel
    {
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
        /// 视频Id
        /// </summary>
        [Required]
        public int AdvertisementId { get; set; }
        public virtual Advertisement Advertisement { get; set; }
    }
}
