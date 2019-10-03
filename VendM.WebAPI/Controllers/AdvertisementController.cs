using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Http;
using VendM.Model.APIModelDto;
using VendM.Service;

namespace VendM.WebAPI.Controllers
{
    /// <summary>
    /// 广告
    /// </summary>
    public class AdvertisementController : ApiController
    {
        AdvertisementService advertisementService = null;
        /// <summary>
        /// 构造
        /// </summary>
        private AdvertisementController()
        {
            advertisementService = new AdvertisementService();
        }
        /// <summary>
        /// 获取广告信息
        /// </summary>
        /// <param name="machine_no">机器编号</param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/advertisement/get")]
        public ResultModels<AdvertisementAPIDto> GetList(string machine_no)
        {
            ResultModels<AdvertisementAPIDto> resultModel = new ResultModels<AdvertisementAPIDto>();
            try
            {
                List<AdvertisementAPIDto> advertisementDtoList = advertisementService.GetAllAdvertisements(machine_no);
                
                resultModel.data = advertisementDtoList;
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
