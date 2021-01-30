using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;

namespace API_MES.Helper
{
    public class ActionAttribute: ActionFilterAttribute
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
                    context.Result = new JsonResult(
                       new
                       {
                           statusCode = "401",
                           message = "未带验证信息,请检查"
                       });
                    return;
                }
                //if (string.IsNullOrEmpty(secret))
                //{
                //    context.Result = new JsonResult(
                //       new
                //       {
                //           statusCode = "401",
                //           message = "未带验证信息,请检查"
                //       }
                //   );
                //    return;
                //}
                //var apiValue = await _headerUser.GetApiUserValueById(secret);
                //if (!secret.Equals(apiValue))
                //{
                //    context.Result = new JsonResult(

                //       new
                //       {
                //           statusCode = "401",
                //           message = "验证信息信息错误,请检查"
                //       }
                //   );
                //    return;
                //}
            }

        }
    }
}
