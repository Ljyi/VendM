using System;
using System.Threading.Tasks;
using System.Web.Http;
using VendM.Model.APIModelDto;
using VendM.Service;

namespace VendM.WebAPI.Controllers
{
    /// <summary>
    /// 支付方式
    /// </summary>
    public class PayMentController : ApiController
    {
        PayMentService payMentService = null;
        /// <summary>
        /// 构造
        /// </summary>
        private PayMentController()
        {
            payMentService = new PayMentService();
        }

        /// <summary>
        /// 获取支付方式
        /// </summary>
        /// <param name="machine_no">机器编号</param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/payment/multi-get")]
        public async Task<ResultModels<PayMentAPIDto>> Get(string machine_no)
        {
            ResultModels<PayMentAPIDto> resultModel = new ResultModels<PayMentAPIDto>();
            try
            {
                resultModel.data = await payMentService.GetPayMentAsync();
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
