using ArcticWolfApi.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace ArcticWolfApi.Filters
{
    public class BaseExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            Console.WriteLine("Excpetion Message: " + context.Exception.Message);
            Console.WriteLine("Excpetion StackTrace: " + context.Exception.StackTrace);
            Console.WriteLine("Excpetion Source: " + context.Exception.Source);

            if (context.Exception is not BaseException)
            {
                context.Exception = new UnhandledErrorException(context.Exception.Message ?? "");
            }

            BaseException exception = (BaseException)context.Exception;
            context.HttpContext.Response.Headers.Add("X-Epic-Error-Name", exception.Code);
            context.HttpContext.Response.Headers.Add("X-Epic-Error-Code", exception.NumericCode.ToString());
            context.Result = new JsonResult(exception)
            {
                StatusCode = new int?(exception.Status)
            };
        }
    }
}
