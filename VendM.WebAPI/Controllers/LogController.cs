using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using VendM.Core;
using VendM.Model;
using VendM.Model.DataModelDto.Log;
using VendM.Service;
using VendM.Service.Log;

namespace VendM.WebAPI.Controllers
{
    /// <summary>
    /// 日志
    /// </summary>
    public class LogController : ApiController
    {
        LogApiService logapiService = null; 
        private LogController()
        {
            logapiService = new LogApiService();
        }
        // GET api/values

        /// <summary>
        /// 添加API客户端日志  
        /// <param name="log">日志信息</param>
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("api/log/add")]
        public async Task<ResultModel> InsertApiLog(APILogDto log)
        {
            ResultModel resultModel = new ResultModel();
            Result result = new Result() { Success = true };
            try
            {
                if (ModelState.IsValid)
                {
                    result = await logapiService.AddAPILog(log);
                    if (!result.Success)
                    {
                        resultModel.code = StatusCodeEnum.HttpRequestError.ToString();
                        resultModel.success = result.Success;
                    }
                    resultModel.message = result.ErrorMsg;
                    if (!result.Success)
                    {
                        resultModel.code = StatusCodeEnum.HttpRequestError.ToString();
                    }
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
                    resultModel.code = StatusCodeEnum.Error.ToString();
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
