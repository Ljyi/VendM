using System.Collections.Generic;
using System.Threading.Tasks;
using VendM.Core.MessageMQ;
using VendM.Model.DataModel;

namespace VendM.Service.EventHandler
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="log"></param>
    /// <returns></returns>
    public delegate Task<bool> SendMQMessage(List<MessageQueue> list);
    /// <summary>
    /// 消息事件
    /// </summary>
    public class ActiveMQEvent
    { //声明委托
        public event SendMQMessage SendMQMessageEvent;
        /// <summary>
        /// 添加日志
        /// </summary>
        /// <param name="log"></param>
        public void SendMQMessage(List<MessageQueue> list)
        {
            if (SendMQMessageEvent != null)
            {
                SendMQMessageEvent(list);
            }
        }
    }
}
