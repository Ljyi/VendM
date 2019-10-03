using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using VendM.Core;
using VendM.Model;
using VendM.Model.APIModelDto;
using VendM.Model.DataModel;
using VendM.Service;

namespace VendM.WebAPI.Controllers
{
    /// <summary>
    /// 机器库存
    /// </summary>
    public class MachineStockController : ApiController
    {
        MachineStockDetailService machineStockDetailService = null;
        /// <summary>
        /// 构造
        /// </summary>
        private MachineStockController()
        {
            machineStockDetailService = new MachineStockDetailService();
        }
        /// <summary>
        /// 补货（更新库存）
        /// </summary>    
        /// <param name="machine_stock">库存</param>
        /// <returns></returns>
        [Route("api/machinestock/update")]
        [HttpPost]
        public async Task<ResultModel> UpdateMachineStockAsync(MachineStockAPIDto machine_stock)
        {
            ResultModel resultModel = new ResultModel();
            Result result = new Result() { Success = true };
            try
            {
                if (ModelState.IsValid)
                {
                    result = await machineStockDetailService.UpdataMachineStock(machine_stock);
                    if (!result.Success)
                    {
                        resultModel.code = StatusCodeEnum.HttpRequestError.ToString();
                    }
                    resultModel.success = result.Success;
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
