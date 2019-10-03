namespace VendM.Core.MessageMQ
{
    /// <summary>
    /// 发送消息
    /// </summary>
    interface IActiveMQProducer : IMessageQueue
    {
        void Put<T>(T body);
    }
}
