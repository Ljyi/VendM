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
    /// 货道
    /// </summary>
    public class PassageController : ApiController
    {
        MachineService machineService = null;
        private PassageController()
        {
            machineService = new MachineService();
        }
        /// <summary>
        /// 获取货道信息
        /// 根据机器编号，获取全部货道以及货道对应的产品
        /// </summary>
        /// <param name="machine_no">机器编号</param>
        /// <returns></returns>
        [Route("api/passages/get")]
        [HttpGet]
        public async Task<ResultModels<MachinePassageProductAPIDto>> GetPassageNumber(string machine_no)
        {
            ResultModels<MachinePassageProductAPIDto> resultModels = new ResultModels<MachinePassageProductAPIDto>();
            try
            {
                resultModels.data = await machineService.GetPassageNumber(machine_no);
            }
            catch (Exception ex)
            {
                resultModels.success = false;
                resultModels.code = StatusCodeEnum.Error.ToString();
                resultModels.message = ex.Message;
            }
            return resultModels;
        }
        /// <summary>
        /// 添加货道
        /// </summary>
        /// <param name="machine_passage"></param>
        /// <returns></returns>
        [Route("api/passages/add")]
        [HttpPost]
        public async Task<ResultModel<bool>> PassageAdd(MachinePassageAPIDto machine_passage)
        {
            ResultModel<bool> resultModel = new ResultModel<bool>();
            try
            {
                if (ModelState.IsValid)
                {
                    await Task.Run(() =>
                    {
                        resultModel.data = machineService.MachineDetailAdd(machine_passage);
                    });
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
        /// <summary>
        /// 删除货道
        /// </summary>
        /// <param name="machine_passage">货道信息</param>
        /// <returns></returns>
        [Route("api/passages/delete")]
        [HttpPost]
        public async Task<ResultModel<bool>> MachineProductDelete(MachinePassageDelAPIDto machine_passage)
        {
            ResultModel<bool> resultModel = new ResultModel<bool>();
            try
            {
                if (ModelState.IsValid)
                {
                    await Task.Run(() =>
                    {
                        resultModel.data = machineService.MachineDetailDelete(machine_passage);
                    });
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
