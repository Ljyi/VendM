using Newtonsoft.Json;

namespace VendM.Core.Upload
{
    public class ApiResultModel
    {
        /// <summary>
        /// 状态
        /// </summary>
        [JsonProperty("status")]
        public ResultStatus Status { get; set; }

        /// <summary>
        /// 业务数据
        /// </summary>
        [JsonProperty("data")]
        public string Data { get; set; }

        /// <summary>
        /// 异常信息
        /// </summary>
        [JsonProperty("messgae")]
        public string ExceptionMessage { get; set; }


        /// <summary>
        /// 成功
        /// </summary>
        /// <param name="data">业务数据</param>
        /// <returns>业务返回实体</returns>
        public static ApiResultModel Success(string data = "")
        {
            return new ApiResultModel { Status = ResultStatus.Ok, Data = data };
        }

        /// <summary>
        /// 成功
        /// </summary>
        /// <param name="data">业务数据</param>
        /// <returns>业务返回实体</returns>
        public static ApiResultModel Success<T>(T data)
        {
            return new ApiResultModel { Status = ResultStatus.Ok, Data = JsonConvert.SerializeObject(data) };
        }

        /// <summary>
        /// 异常情况
        /// </summary>
        /// <param name="status">状态码</param>
        /// <param name="exceptionStr">异常信息</param>
        /// <returns></returns>
        public static ApiResultModel Exception(ResultStatus status, string exceptionStr)
        {
            return new ApiResultModel { Status = status, ExceptionMessage = exceptionStr };
        }
      
    }

    public enum ResultStatus
    {
        /// <summary>
        /// 成功，正常
        /// </summary>
        Ok = 0,

        /// <summary>
        /// 提示
        /// </summary>
        Tip = 1,

        /// <summary>
        /// 程序异常
        /// </summary>
        Error = 2,
    }
}
