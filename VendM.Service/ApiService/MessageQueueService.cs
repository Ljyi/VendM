using LinqKit;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using VendM.Model.DataModel;
using VendM.Model.EnumModel;

namespace VendM.Service
{
    public partial class MessageQueueService
    {
        /// <summary>
        /// 异步更新
        /// </summary>
        /// <param name="mqid">消息ID</param>
        /// <param name="mqName">消息队列名称</param>
        /// <returns></returns>
        public async Task<bool> UpDataMqAsync(int mqid, string mqName)
        {
            return await Task.Run(() =>
            {
                try
                {
                    Expression<Func<MessageQueue, bool>> ex = t => true;
                    ex = ex.And(t => t.Id == mqid);
                    ex = ex.And(t => t.QueueName == mqName);
                    ex = ex.And(t => !t.IsDelete);
                    var messageQueue = messageQueueRepository.GetEntities(ex).FirstOrDefault();
                    if (messageQueue != null)
                    {
                        messageQueue.Status = (int)MQMessageStatusEnum.AcceptSuccess;
                        messageQueue.UpdateTime = DateTime.Now;
                        messageQueue.UpdateUser = "Client";
                        return messageQueueRepository.Update(messageQueue) > 0;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            });
        }
    }
}
