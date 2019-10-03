using AutoMapper;
using LinqKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using VendM.Model.APIModelDto;
using VendM.Model.DataModel.Basics;

namespace VendM.Service
{
    public partial class PayMentService
    {
        /// <summary>
        ///异步获取支付方式
        /// </summary>
        /// <returns></returns>
        public async Task<List<PayMentAPIDto>> GetPayMentAsync()
        {
            return await Task.Run(() =>
             {
                 Expression<Func<PayMent, bool>> ex = t => true;
                 ex = ex.And(t => !t.IsDelete);
                 var paymentList = paymentRepository.GetEntities(ex);
                 return Mapper.Map<List<PayMent>, List<PayMentAPIDto>>(paymentList.ToList());
             });
        }
    }
}
