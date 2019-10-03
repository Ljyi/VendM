using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VendM.Model.DataModel
{
    /// <summary>
    /// APP 实体
    /// </summary>
    [Table("APP")]
    public class APP : BaseModel
    {
        /// <summary>
        /// 版本号
        /// </summary>
        [MaxLength(10)]
        [Required]
        public string Version { get; set; }
        /// <summary>
        /// 下载链接
        /// </summary>
        [MaxLength(100)]
        [Required]
        public string Url { get; set; }
    }
}
