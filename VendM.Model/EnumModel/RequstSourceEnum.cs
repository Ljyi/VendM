using System.ComponentModel;

namespace VendM.Model.EnumModel
{
    /// <summary>
    /// 请求来源
    /// </summary>
    public enum RequstSourceEnum
    {
        /// <summary>
        /// 客户端请求
        /// </summary>
        [Description("客户端")]
        Client = 0,
        /// <summary>
        /// 服务器请求
        /// </summary>
        [Description("服务端")]
        Server = 1,
    }
}
