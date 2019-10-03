using System;
using System.Threading.Tasks;
using System.Web.Http;
using VendM.Service;

namespace VendM.WebAPI.Controllers
{
    /// <summary>
    /// 消息队列
    /// </summary>
    public class MessageQueueController : ApiController
    {
        MessageQueueService messageQueueService = null;
        private MessageQueueController()
        {
            messageQueueService = new MessageQueueService();
        }
        /// <summary>
        /// 更新消息
        /// </summary>
        /// <param name="mqid">消息ID</param>
        /// <param name="mqName">消息队列名称</param>
        /// <param name="machine_no">机器编号</param>
        /// <returns></returns>
        [Route("api/messagequeue/update")]
        [HttpPost]
        public async Task<ResultModel> TaskUpMessageQueueAsync(int mqid, string mqName, string machine_no)
        {
            ResultModel resultModel = new ResultModel();
            try
            {
                resultModel.success = await messageQueueService.UpDataMqAsync(mqid, mqName);
                if (!resultModel.success)
                {
                    resultModel.code = StatusCodeEnum.HttpRequestError.ToString();
                }
            }
            catch (Exception ex)
            {
                resultModel.success = false;
                resultModel.code = StatusCodeEnum.Error.ToString();
                resultModel.message = ex.Message;
            }
            return resultModel;
        }
    }
}
