using IPOS.API;
using IPOS.Model.Request;
using IPOS.Model.ResponseModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace VendM.Application
{
    public class Transaction
    {
        /// <summary>
        /// 交易
        /// </summary>
        /// <param name="cardNo">卡号</param>
        /// <param name="productId">产品Id</param>
        /// <param name="ProductWayId">支付Id</param>
        /// <returns></returns>
        public async Task<ResultModel<TransactionRepsone>> TransactionAsync(string cardNo, string productVId, string ProductWayId)
        {
            ResultModel<TransactionRepsone> resultModel = new ResultModel<TransactionRepsone>();
            try
            {

                TransactionAPI transactionAPI = new TransactionAPI();
                TransactionRequest transactionRequest = new TransactionRequest()
                {
                    UserDetailId = "40BD81CC-7B86-4666-BBD2-12A292D6ED8A",
                    PhysicalCardId = "BBC00007",
                    MarketId = 160,
                    CurrencyId = 1,
                    RedeemTypeId = 3,
                    TransationMethodId = 3,
                    RedeemProudctList = new List<RedeemProudct>(){
                                        new RedeemProudct() {
                                            RedeemProductId = productVId,
                                            RedeemProductWayId = ProductWayId,// "a46f4557-9db1-47fc-8d65-7a5dd465517e",
                                            Quantity =  1
                                        }
                                        }
                };
                if (!string.IsNullOrEmpty(cardNo))
                {
                    UserRequst userRequst = new UserRequst();
                    userRequst.CardNo = cardNo;
                    UserAPI userAPI = new UserAPI();
                    ResultModel<UserRespone> userRespone = await userAPI.GetUserInfo(userRequst);
                    if (userRespone.Success)
                    {
                        if (userRespone.Result.IsSuccess && userRespone.Result.Data != null)
                        {
                            transactionRequest.UserDetailId = userRespone.Result.Data.verifiedSqlId;
                            transactionRequest.PhysicalCardId = cardNo;
                        }
                        else
                        {
                            resultModel.Success = false;
                            resultModel.Message = "外部接口失败!";
                            return resultModel;
                        }
                    }
                    else
                    {
                        resultModel.Success = false;
                        resultModel.Message = "内部接口失败!";
                        return resultModel;
                    }
                }
                resultModel = await transactionAPI.Transaction(transactionRequest);
            }
            catch (System.Exception ex)
            {
                resultModel.Message = ex.Message;
                resultModel.Success = false;
            }
            return resultModel;
        }
        /// <summary>
        /// 确认交易
        /// </summary>
        /// <param name="transactionId"></param>
        /// <param name="isSuccess"></param>
        /// <returns></returns>
        public async Task<ResultModel<TransactionCompleteRepsone>> TransactionComplete(string cardNo, string transactionId, bool isSuccess)
        {
            ResultModel<TransactionCompleteRepsone> resultModel = new ResultModel<TransactionCompleteRepsone>();
            try
            {
                TransactionCompleteAPI transactionCompleteAPI = new TransactionCompleteAPI();
                TransactionCompleteRequest transactionCompleteRequest = new TransactionCompleteRequest()
                {
                    Success = isSuccess,
                    UserDetailId = "40BD81CC-7B86-4666-BBD2-12A292D6ED8A",
                    RedeemTransactionId = transactionId,
                    TransationMethodId = 3,
                    Currency = 1,
                    Amount = 1,
                    N5_PaymentType = ""
                };
                if (!string.IsNullOrEmpty(cardNo))
                {
                    UserRequst userRequst = new UserRequst();
                    userRequst.CardNo = cardNo;
                    UserAPI userAPI = new UserAPI();
                    ResultModel<UserRespone> userRespone = await userAPI.GetUserInfo(userRequst);
                    if (userRespone.Success)
                    {
                        if (userRespone.Result.IsSuccess && userRespone.Result.Data != null)
                        {
                            transactionCompleteRequest.UserDetailId = userRespone.Result.Data.verifiedSqlId;
                        }
                        else
                        {
                            resultModel.Success = false;
                            resultModel.Message = "外部接口失败!" + userRespone.Message;
                            return resultModel;
                        }
                    }
                    else
                    {
                        resultModel.Success = false;
                        resultModel.Message = "内部接口失败!" + userRespone.Message;
                        return resultModel;
                    }
                }
                resultModel = await transactionCompleteAPI.TransactionComplete(transactionCompleteRequest);
            }
            catch (System.Exception ex)
            {
                resultModel.Message = ex.Message;
                resultModel.Success = false;
            }
            return resultModel;
        }
    }
}
