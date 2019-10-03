using System.Collections.Generic;

namespace VendM.Web.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class ResultJsonInfo
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool Success { get; set; }
        /// <summary>
        /// 返回数据
        /// </summary>
        public string Data { get; set; }
        /// <summary>
        /// 返回异常信息
        /// </summary>
        public string ErrorMsg { get; set; }
    }
    public class ResultJsonInfo<T>
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool success { get; set; }
        /// <summary>
        /// 返回数据集合
        /// </summary>
        public List<T> data { get; set; }
        /// <summary>
        /// 返回异常信息
        /// </summary>
        public string errorMsg { get; set; }
    }
}