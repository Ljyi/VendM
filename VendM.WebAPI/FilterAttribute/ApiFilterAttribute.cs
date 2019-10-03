using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http.Filters;
using VendM.Core;
using VendM.Core.Json;

namespace VendM.WebAPI.FilterAttribute
{
    /// <summary>
    /// 过滤器
    /// </summary>
    public class ApiFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            ResultModel<string> resultMsg = null;
            var request = actionContext.Request;
            string urlPath = request.RequestUri.AbsolutePath;
            string method = request.Method.Method;
            string timestamp = string.Empty;
            string signature = string.Empty;
            string secretKey = string.Empty;
            string secretKeyConfig = ConfigurationManager.AppSettings["secretKey"].Trim();
            Dictionary<string, Object> parameters = new Dictionary<string, Object>();
            if (request.Headers.Contains("signature"))
            {
                signature = HttpUtility.UrlDecode(request.Headers.GetValues("signature").FirstOrDefault());
            }
            if (request.Headers.Contains("secretKey"))
            {
                secretKey = HttpUtility.UrlDecode(request.Headers.GetValues("secretKey").FirstOrDefault());
            }
            //判断请求头是否包含以下参数
            if (string.IsNullOrEmpty(secretKey) || string.IsNullOrEmpty(signature))
            {
                resultMsg = new ResultModel<string>();
                resultMsg.code = StatusCodeEnum.ParameterError.ToString();
                resultMsg.message = EnumExtension.GetDescription(StatusCodeEnum.URLExpireError);
                actionContext.Response = new HttpResponseMessage { Content = new StringContent(JsonConvert.SerializeObject(resultMsg), Encoding.GetEncoding("UTF-8"), "application/json") };
                base.OnActionExecuting(actionContext);
                return;
            }
            if (secretKey != secretKeyConfig)
            {
                resultMsg = new ResultModel<string>();
                resultMsg.code = StatusCodeEnum.ParameterError.ToString();
                resultMsg.message = EnumExtension.GetDescription(StatusCodeEnum.URLExpireError);
                actionContext.Response = new HttpResponseMessage { Content = new StringContent(JsonConvert.SerializeObject(resultMsg), Encoding.GetEncoding("UTF-8"), "application/json") };
                base.OnActionExecuting(actionContext);
                return;
            }
            //根据请求类型拼接参数
            NameValueCollection form = HttpContext.Current.Request.QueryString;
            string data = string.Empty;
            try
            {
                switch (method)
                {
                    case "POST":
                        Stream stream = HttpContext.Current.Request.InputStream;
                        string responseJson = string.Empty;
                        StreamReader streamReader = new StreamReader(stream);
                        data = streamReader.ReadToEnd();
                        parameters = JToken.Parse(data).ToDictionary();
                        break;
                    case "GET":
                        for (int f = 0; f < form.Count; f++)
                        {
                            string key = form.Keys[f];
                            parameters.Add(key, form[key]);
                        }
                        break;
                    default:
                        resultMsg = new ResultModel<string>();
                        resultMsg.code = StatusCodeEnum.HttpMehtodError.ToString();
                        resultMsg.message = EnumExtension.GetDescription(StatusCodeEnum.HttpMehtodError);
                        actionContext.Response = new HttpResponseMessage { Content = new StringContent(JsonConvert.SerializeObject(resultMsg), Encoding.GetEncoding("UTF-8"), "application/json") };
                        base.OnActionExecuting(actionContext);
                        return;
                }
            }
            catch (Exception ex)
            {
                resultMsg = new ResultModel<string>();
                resultMsg.code = StatusCodeEnum.ParameterError.ToString();
                resultMsg.message = EnumExtension.GetDescription(StatusCodeEnum.URLExpireError) + "   " + ex.Message;
                actionContext.Response = new HttpResponseMessage { Content = new StringContent(JsonConvert.SerializeObject(resultMsg), Encoding.GetEncoding("UTF-8"), "application/json") };
                base.OnActionExecuting(actionContext);
                return;
            }
            bool result = APIHelper.ValidateSign(urlPath, parameters, secretKey, signature);
            result = true;
            if (!result)
            {
                resultMsg = new ResultModel<string>();
                resultMsg.code = StatusCodeEnum.HttpRequestError.ToString();
                resultMsg.message = EnumExtension.GetDescription(StatusCodeEnum.HttpRequestError);
                actionContext.Response = new HttpResponseMessage { Content = new StringContent(JsonConvert.SerializeObject(resultMsg), Encoding.GetEncoding("UTF-8"), "application/json") };
                base.OnActionExecuting(actionContext);
                return;
            }
            else
            {
                base.OnActionExecuting(actionContext);
            }
        }
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            base.OnActionExecuted(actionExecutedContext);
        }
    }
}