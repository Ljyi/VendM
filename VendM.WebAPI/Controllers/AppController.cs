using System.Configuration;
using System.Web.Http;
using VendM.Model.APIModelDto;
using VendM.Service.BasicsService;

namespace VendM.WebAPI.Controllers
{
    /// <summary>
    /// app版本更新
    /// </summary>
    public class AppController : ApiController
    {
        static string httplink = ConfigurationManager.AppSettings["HttpUrl"].Trim();

        APPVersionService appVersionService = null;
        private AppController()
        {
            this.appVersionService = new APPVersionService();
        }
        /// <summary>
        /// 客户端App更新接口
        /// </summary>
        /// <param name="machine_no"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/App/get")]
        public ResultModel<AppDto> GetUpdateApp(string machine_no)
        {
            ResultModel<AppDto> resultModel = new ResultModel<AppDto>();
            var appVersion = appVersionService.GetAppVersionFirst();
            if (appVersion != null)
            {
                resultModel.data = new AppDto()
                {
                    url = httplink + appVersion.url,
                    version = appVersion.version
                };
            }
            return resultModel;
        }
    }
}
