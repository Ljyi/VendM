using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using VendM.Core;
using VendM.Model.APIModelDto;
using VendM.Model.DataModel;

namespace VendM.Service
{
    /// <summary>
    /// API
    /// </summary>
    public partial class AdvertisementService
    {
        #region API
        /// <summary>
        /// 获取广告
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<AdvertisementAPIDto> GetAllAdvertisements(string machineNo)
        {
            Expression<Func<Advertisement, bool>> ex = t => true;
            ex = ex.And(t => !t.IsDelete);
            ex = ex.And(t => t.StartTime < DateTime.Now);
            ex = ex.And(t => t.EndTime > DateTime.Now);
            var advertisementList = advertisementRepository.GetEntities(ex);
            return Mapper.Map<List<Advertisement>, List<AdvertisementAPIDto>>(advertisementList.ToList());
        }
        #endregion
    }
}
