using IPOS.Model.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using VendM.Application;
using VendM.Core;
using VendM.Model.APIModelDto;

namespace VendM.WebAPI.Controllers
{
    /// <summary>
    /// 交易接口
    /// </summary>
    public class TransactionController : ApiController
    {
        /// <summary>
        /// 交易
        /// </summary>
        /// <returns></returns>
        [Route("api/Transaction/Transaction")]
        [HttpPost]
        public async Task<ResultModel<TransactionRepsoneAPIDto>> VMTransactionAsync(TransactionAPIDto transactionAPIDto)
        {
            ResultModel<TransactionRepsoneAPIDto> resultModel = new ResultModel<TransactionRepsoneAPIDto>();
            try
            {
                if (ModelState.IsValid)
                {
                    Transaction transaction = new Transaction();
                    IPOS.Model.ResponseModel.ResultModel<TransactionRepsone> resultModelIPOS = await transaction.TransactionAsync(transactionAPIDto.CardNo, transactionAPIDto.ProductVId, transactionAPIDto.ProductPayId);
                    if (resultModelIPOS.Success)
                    {
                        if (resultModelIPOS.Result.Data != null && resultModelIPOS.Result.IsSuccess != false)
                        {
                            resultModel.data = new TransactionRepsoneAPIDto()
                            {
                                TransactionId = resultModelIPOS.Result.Data.redeemTransaction.id
                            };
                        }
                        else
                        {
                            resultModel.message = resultModelIPOS.Message;
                            resultModel.success = false;
                        }
                    }
                    else
                    {
                        resultModel.success = false;
                        resultModel.message = resultModelIPOS.Message;
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
        /// <summary>
        /// 确认交易
        /// </summary>
        /// <returns></returns>
        [Route("api/Transaction/Complete")]
        [HttpPost]
        public async Task<ResultModel> VMTransactionCompleteAsync(TransactionCompleteAPIDto transactionCompleteDto)
        {
            ResultModel resultModel = new ResultModel();
            try
            {
                if (ModelState.IsValid)
                {
                    Transaction transaction = new Transaction();
                    IPOS.Model.ResponseModel.ResultModel<TransactionCompleteRepsone> resultModelIPOS = await transaction.TransactionComplete(transactionCompleteDto.CardNo, transactionCompleteDto.TransactionId, transactionCompleteDto.IsSuccess);
                    if (resultModelIPOS.Success)
                    {
                        resultModel.success = resultModelIPOS.Result.IsSuccess;
                    }
                    else
                    {
                        resultModel.success = false;
                        resultModel.message = resultModelIPOS.Message;
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
