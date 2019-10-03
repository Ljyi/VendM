using System;

namespace VendM.Model
{
    public class Result
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool Success { get; set; }
        /// <summary>
        /// 返回异常信息
        /// </summary>
        public string ErrorMsg { get; set; }

        public static implicit operator bool(Result v)
        {
            throw new NotImplementedException();
        }
    }
}
