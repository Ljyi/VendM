using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Description;
using System.Web.Http.Dispatcher;

namespace VendM.WebAPI.Controllers
{
    /// <summary>
    /// 过滤API不需要生成文档
    /// </summary>
    [ApiExplorerSettings(IgnoreApi = true)]
    public class VersionController : DefaultHttpControllerSelector
    {
        private readonly HttpConfiguration _config;
        public VersionController(HttpConfiguration config) : base(config)
        {
            _config = config;
        }
        public override IDictionary<string, HttpControllerDescriptor> GetControllerMapping()
        {
            Dictionary<string, HttpControllerDescriptor> dict = new Dictionary<string, HttpControllerDescriptor>();
            foreach (var asm in _config.Services.GetAssembliesResolver().GetAssemblies())
            {
                //获取所有继承自ApiController的非抽象类 
                var controllerTypes = asm.GetTypes()
                    .Where(t => !t.IsAbstract && typeof(ApiController)
                    .IsAssignableFrom(t)).ToArray();
                foreach (var ctrlType in controllerTypes)
                {
                    //从namespace中提取出版本号                  命名空间，有可能不是当前的weiapi项目
                    var match = Regex.Match(ctrlType.Namespace, GetType().Namespace + @".Controllers.v(\d+)");
                    if (match.Success)
                    {
                        string verNum = match.Groups[1].Value;//获取版本号  
                        //从PersonController中拿到Person
                        string ctrlName = Regex.Match(ctrlType.Name, "(.+)Controller").Groups[1].Value;
                        //Personv2为key
                        string key = ctrlName + "v" + verNum;
                        dict[key] = new HttpControllerDescriptor(_config, ctrlName, ctrlType);
                    }
                }
            }
            return dict;
        }

        //设计就是返回HttpControllerDesriptor的过程 
        public override HttpControllerDescriptor SelectController(HttpRequestMessage request)
        {
            //获取所有的controller键值集合 
            var controllers = GetControllerMapping();
            //获取路由数据 
            var routeData = request.GetRouteData();
            //从路由中获取当前controller的名称 
            var controllerName = (string)routeData.Values["controller"];
            var verNum = request.Headers.TryGetValues("ApiVersion", out var versions) ?
                versions.Single() :
                Regex.Match(request.RequestUri.PathAndQuery, @"api/v(\d+)").Groups[1].Value;
            //获取版本号 
            var key = controllerName + "v" + verNum;//获取Personv2             
            return controllers.ContainsKey(key) ? controllers[key] : null;
        }
    }
}