using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using VendM.Core;
using VendM.Model;
using VendM.Model.APIModelDto;
using VendM.Service;

namespace VendM.WebAPI.Controllers
{
    /// <summary>
    /// 订单
    /// </summary>
    public class OrderController : ApiController
    {
        OrderService orderService = null;
        /// <summary>
        /// 构造
        /// </summary>
        private OrderController()
        {
            orderService = new OrderService();
        }
        /// <summary>
        /// 创建订单
        /// </summary>
        /// <param name="order">订单信息</param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/order/add")]
        public async Task<ResultModel> CreateOrderAsync(OrderAPIDto order)
        {
            ResultModel resultModel = new ResultModel();
            Result result = new Result() { Success = true };
            try
            {
                if (ModelState.IsValid)
                {
                    result = await orderService.AddAsync(order);
                    if (!result.Success)
                    {
                        resultModel.code = StatusCodeEnum.HttpRequestError.ToString();
                        resultModel.success = result.Success;
                    }
                    resultModel.message = result.ErrorMsg;
                }
                else
                {
                    List<string> errorMsg = new List<string>();
                    foreach (var key in ModelState.Keys)
                    {
                        var modelstate = ModelState[key];
                        if (modelstate.Errors.Any())
                        {
                            errorMsg.Add(modelstate.Errors.FirstOrDefault().ErrorMessage);
                        }
                    }
                    resultModel.code = StatusCodeEnum.HttpRequestError.ToString();
                    resultModel.success = false;
                    resultModel.message = errorMsg.ToSeparateString();
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
