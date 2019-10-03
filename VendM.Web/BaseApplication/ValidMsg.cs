using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Remoting.Messaging;
using System.Web.Mvc;

namespace VendM.Web.BaseApplication
{
    //[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true)]
    //public class ValidMsg : ActionFilterAttribute
    //{
    //    public override void OnActionExecuting(System.Web.Http.Controllers.HttpActionContext actionContext)
    //    {
    //        var modelState = actionContext.ModelState;
    //        if (!modelState.IsValid)
    //        {
    //            string error = string.Empty;
    //            foreach (var key in modelState.Keys)
    //            {
    //                var state = modelState[key];
    //                if (state.Errors.Any())
    //                {
    //                    error = state.Errors.First().ErrorMessage;
    //                    break;
    //                }
    //            }
    //            ReturnMessage response = new ReturnMessage() { Status = ResultStatus.Failed, Message = error };
    //            actionContext.Response = new HttpResponseMessage(HttpStatusCode.Accepted)
    //            {
    //                Content = new StringContent(JsonConvert.SerializeObject(response), System.Text.Encoding.GetEncoding("UTF-8"), "application/json")
    //            };
    //        }
    //    }
    //}
}