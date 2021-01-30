using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_COM.Helper
{
    public class ActionAttribute : ActionFilterAttribute
    {
        //private readonly IHeaderUserRepository _headerUser;

        //public ActionAttribute(IHeaderUserRepository headerUser)
        //{
        //    _headerUser = headerUser;
        //}

        /// <summary>
        /// 请求到达控制器之前
        /// </summary>
        /// <param name="context"></param>
        public async override void OnActionExecuting(ActionExecutingContext context)
        {
            bool isCheck = false;
            //判断Controller是否跳过验证
            if (context.Controller.GetType().GetCustomAttributes(typeof(SkipActionFilterAttribute), false).Any())
            {
                isCheck = true;
            }
            //判定Action是否跳过验证
            if (context.ActionDescriptor is ControllerActionDescriptor controllerActionDescriptor)
            {
                if (controllerActionDescriptor.MethodInfo.GetCustomAttributes(inherit: true)
                       .Any(a => a.GetType().Equals(typeof(SkipActionFilterAttribute))))
                {
                    isCheck = true;
                }

            }
            //需要验证
            if (!isCheck)
            {
                string secret = context.HttpContext.Request.Headers["Authorization"];
                if (string.IsNullOrEmpty(secret))
                {
                    secret = context.HttpContext.Request.Headers["cnceKey"];
                }
                string appMethod = context.HttpContext.Request.Headers["cnceMethod"];
                if (string.IsNullOrEmpty(secret))
                {
                    context.Result = new JsonResult(
                       new
                       {
                           success = false,
                           statusCode = "401",
                           msg = "未带cnceKey验证信息,请检查"
                       });
                    return;
                } 
                if (!secret.Equals("CNCE")&&!secret.Equals("0b0da5ad55958ad47ec0c59e870851a4"))
                {
                   
                    context.Result = new JsonResult(

                       new
                       {
                           success=false,
                           statusCode = "401",
                           msg = "验证cnceKey信息信息错误,请检查"//message
                       }
                   );
                    return;
                }
                if (!secret.Equals("CNCE"))
                {
                    if (string.IsNullOrEmpty(appMethod))
                    {
                        context.Result = new JsonResult(
                           new
                           {
                               success = false,
                               statusCode = "401",
                               msg = "未带cnceMethod验证信息,请检查"
                           });
                        return;
                    }
                    if (secret.Equals("0b0da5ad55958ad47ec0c59e870851a4") && !appMethod.Equals("0b0da5ad55958ad47ec0c59e870851a4"))
                    {
                        context.Result = new JsonResult(

                           new
                           {
                               success = false,
                               statusCode = "401",
                               msg = "验证cnceKey信息与cnceMethod不匹配"//message
                       }
                       );
                        return;
                    }
                }
              
            }

        }
    }
}
