using ArcticWolfApi.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArcticWolfApi.Filters
{
    public class BaseExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            Console.WriteLine("Excpetion Message: " + context.Exception.Message);
            Console.WriteLine("Excpetion StackTrace: " + context.Exception.StackTrace);
            Console.WriteLine("Excpetion Source: " + context.Exception.Source);

            if (!(context.Exception is BaseException))
                context.Exception = (Exception)new UnhandledErrorException(context.Exception.Message ?? "");
            BaseException exception = (BaseException)context.Exception;
            context.HttpContext.Response.Headers.Add("X-Epic-Error-Name", (StringValues)exception.Code);
            context.HttpContext.Response.Headers.Add("X-Epic-Error-Code", (StringValues)exception.NumericCode.ToString());
            context.Result = (IActionResult)new JsonResult((object)exception)
            {
                StatusCode = new int?(exception.Status)
            };
        }
    }
}
