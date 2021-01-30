using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using API_MES.Helper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API_MES.Controllers
{
    [Authorize]
    //[ApiController]
    //[ApiVersion("1")]
    //[Route("api/V{version:apiVersion}/[controller]")]
    [Route("api/[controller]")]
    [ApiController]
    [SkipActionFilter]
    public class DefaultController : ControllerBase
    {
        #region MyRegion
        // GET: api/Default
        [HttpGet]
        public IEnumerable<string> DefaultGet()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Default/5
        [HttpGet("{id}", Name = "Get")]
        public string DefaultGet(int id)
        {
            return "value";
        }

        // POST: api/Default
        [HttpPost]
        public void DefaultPost([FromBody] string value)
        {
        }

        // PUT: api/Default/5
        [HttpPut("{id}")]
        public void DefaultPut(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void DefaultDelete(int id)
        {
        }
        #endregion

        private readonly ISecureDataFormat<string> _dataFormat;

        public DefaultController(IDataProtectionProvider _dataProtectionProvider)
        {
            var dataProtector = _dataProtectionProvider.CreateProtector(typeof(DefaultController).FullName);
            _dataFormat = new SecureDataFormat<string>(new StringSerializer(), dataProtector);
        }

        public string GenerateToken()
        {
            return _dataFormat.Protect(Guid.NewGuid().ToString() + ";" + DateTime.Now.AddHours(10));
        }

        public string DecryptToken(string token)
        {
            return _dataFormat.Unprotect(token);
        }

        private class StringSerializer : IDataSerializer<string>
        {
            public string Deserialize(byte[] data)
            {
                return Encoding.UTF8.GetString(data);
            }

            public byte[] Serialize(string model)
            {
                return Encoding.UTF8.GetBytes(model);
            }
        }

        // GET api/values1
        [HttpGet]
        [Route("api/value1")]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value1" };
        }

        // GET api/values2
        /**
         * 该接口用Authorize特性做了权限校验，如果没有通过权限校验，则http返回状态码为401
         * 调用该接口的正确姿势是：
         * 1.登陆，调用api/Auth接口获取到token
         * 2.调用该接口 api/value2 在请求的Header中添加参数 Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYmYiOiIxNTYwMzM1MzM3IiwiZXhwIjoxNTYwMzM3MTM3LCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiemhhbmdzYW4iLCJpc3MiOiJodHRwOi8vbG9jYWxob3N0OjUwMDAiLCJhdWQiOiJodHRwOi8vbG9jYWxob3N0OjUwMDAifQ.1S-40SrA4po2l4lB_QdzON_G5ZNT4P_6U25xhTcl7hI
         * Bearer后面有空格，且后面是第一步中接口返回的token值
         * */
        [HttpGet]
        [Route("api/value2")]
        [Authorize]
        public ActionResult<IEnumerable<string>> Get2()
        {
            //这是获取自定义参数的方法
            var auth = HttpContext.AuthenticateAsync().Result.Principal.Claims;
            var userName = auth.FirstOrDefault(t => t.Type.Equals(ClaimTypes.NameIdentifier))?.Value;
            return new string[] { "这个接口登陆过的用户都可以访问", $"userName={userName}" };
        }

        /**
         * 这个接口必须用admin
         **/
        [HttpGet]
        [Route("api/value3")]
        [Authorize("Permission")]
        public ActionResult<IEnumerable<string>> Get3()
        {
            //这是获取自定义参数的方法
            var auth = HttpContext.AuthenticateAsync().Result.Principal.Claims;
            var userName = auth.FirstOrDefault(t => t.Type.Equals(ClaimTypes.NameIdentifier))?.Value;
            var role = auth.FirstOrDefault(t => t.Type.Equals("Role"))?.Value;
            return new string[] { "这个接口有管理员权限才可以访问", $"userName={userName}", $"Role={role}" };
        }
        static async Task GetValueAsync()
        {
            await Task.Run(() =>
            {
                Thread.Sleep(1000);
                for (int i = 0; i < 5; ++i)
                {
                    Console.Out.WriteLine(String.Format("From task : {0}", i));
                }
            });
            Console.Out.WriteLine("Task End");
        }
    }
}
