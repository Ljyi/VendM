using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Web.Http.ModelBinding;

namespace VendM.WebAPI.FilterAttribute
{
    /// <summary>
    /// API验证
    /// </summary>
    public class ModelValidationFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (actionContext.ModelState.IsValid == false)
            {
                var errors = new Dictionary<string, IEnumerable<string>>();
                foreach (KeyValuePair<string, ModelState> keyValue in actionContext.ModelState)
                {
                    errors[keyValue.Key] = keyValue.Value.Errors.Select(e => e.ErrorMessage);
                }
                ResultModel response = new ResultModel() { code = HttpStatusCode.Accepted.ToString(), success = false, message = JsonConvert.SerializeObject(errors) };
                actionContext.Response = new HttpResponseMessage(HttpStatusCode.Accepted)
                {
                    Content = new StringContent(JsonConvert.SerializeObject(response), System.Text.Encoding.GetEncoding("UTF-8"), "application/json")
                };
            }
        }
    }
}