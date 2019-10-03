using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VendM.Core.MessageMQ
{
    /// <summary>
    /// 消息队列接口
    /// </summary>
    public interface IMessageQueue
    {

        /// <summary>
        /// 打开连接
        /// </summary>
        void OpenConnection();

        /// <summary>
        /// 关闭连接
        /// </summary>
        void CloseConnection();
    }
}
