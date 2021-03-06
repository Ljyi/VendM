﻿using System.ComponentModel;

namespace VendM.WebAPI
{
    /// <summary>
    /// API 请求异常
    /// </summary>
    public enum StatusCodeEnum
    {
        [Description("请求(或处理)成功")]
        Success = 200, //请求(或处理)成功

        [Description("内部请求出错")]
        Error = 500, //内部请求出错

        [Description("未授权标识")]
        Unauthorized = 401,//未授权标识

        [Description("请求参数不完整或不正确")]
        ParameterError = 400,//请求参数不完整或不正确

        [Description("请求TOKEN失效")]
        TokenInvalid = 403,//请求TOKEN失效

        [Description("HTTP请求类型不合法")]
        HttpMehtodError = 405,//HTTP请求类型不合法

        [Description("HTTP请求不合法,请求参数可能被篡改")]
        HttpRequestError = 406,//HTTP请求不合法

        [Description("该URL已经失效")]
        URLExpireError = 407,//HTTP请求不合法
    }
}