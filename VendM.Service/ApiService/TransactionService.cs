using System;
using System.Threading.Tasks;
using VendM.Model;
using VendM.Model.DataModel;

namespace VendM.Service
{
    public partial class TransactionService
    {  /// <summary>
       ///添加交易信息
       /// </summary>
       /// <param name="orderNo"></param>
       /// <param name="transactionInfo"></param>
       /// <returns></returns>
        public async Task<Result> AddTransaction(string orderNo, string transactionInfo)
        {
            return await Task.Run(() =>
            {
                Result result = new Result() { Success = true };
                try
                {
                    Transaction transaction = new Transaction()
                    {
                        OrderNo = orderNo,
                        TransactionInfo = transactionInfo,
                        IsDelete = false,
                        CreateUser = "system",
                        CredateTime = DateTime.Now,
                        UpdateUser = "system",
                        UpdateTime = DateTime.Now
                    };
                    transactionRepository.Insert(transaction);
                    return result;
                }
                catch (Exception ex)
                {
                    result.ErrorMsg = "请求数据{orderNo:" + orderNo + "transactionInfo:" + transactionInfo + "} 添加交易信息异常" + ex.Message;
                    result.Success = false;
                }
                return result;
            });
        }
    }
}
