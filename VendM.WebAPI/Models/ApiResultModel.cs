using System.Collections.Generic;

namespace VendM.WebAPI
{
    /// <summary>
    /// API返回信息
    /// </summary>
    public class ResultModels<T>
    {
        /// <summary>
        /// 返回类型
        /// </summary>
        public ResultModels()
        {
            this.api_version = "v1";
            code = "200";
            this.success = true;
        }
        /// <summary>
        /// 版本号
        /// </summary>
        public string api_version { get; set; }
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool success { get; set; }
        /// <summary>
        /// code
        /// </summary>
        public string code { get; set; }
        /// <summary>
        /// 返回信息
        /// </summary>
        public string message { get; set; }
        /// <summary>
        /// 返回数据集合
        /// </summary>
        public List<T> data { get; set; }
    }
    /// <summary>
    /// API返回信息
    /// </summary>
    public class ResultModel
    {
        /// <summary>
        /// 返回类型
        /// </summary>
        public ResultModel()
        {
            this.api_version = "v1";
            code = "200";
            this.success = true;
        }
        /// <summary>
        /// 版本号
        /// </summary>
        public string api_version { get; set; }
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool success { get; set; }
        /// <summary>
        /// code
        /// </summary>
        public string code { get; set; }
        /// <summary>
        /// 返回信息
        /// </summary>
        public string message { get; set; }
    }
    /// <summary>
    /// API返回信息
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ResultModel<T>
    {
        /// <summary>
        /// 返回类型
        /// </summary>
        public ResultModel()
        {
            this.api_version = "v1";
            code = "200";
            this.success = true;
        }
        /// <summary>
        /// 版本号
        /// </summary>
        public string api_version { get; set; }
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool success { get; set; }
        /// <summary>
        /// code
        /// </summary>
        public string code { get; set; }
        /// <summary>
        /// 返回信息
        /// </summary>
        public string message { get; set; }
        /// <summary>
        /// 返回数据集合
        /// </summary>
        public T data { get; set; }
    }
}