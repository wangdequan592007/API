using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using API_MES.Entities;
using API_MES.Helper;
using API_MES.Mappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens; 
namespace API_MES.Controllers
{
    //[Authorize]
    //[ApiVersion("1")]
    //[ApiController]
    //[Route("api/v{version:apiVersion}/[controller]")]
    [Route("api/[controller]")]
    [ApiController]
    [SkipActionFilter]
    public class AuthenticateController : ControllerBase
    {
        public IConfiguration _configuration { get; }

        public AuthenticateController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        ///// <summary>
        ///// 用户登录
        ///// </summary>
        ///// <param name="userName"></param>
        ///// <param name="password"></param>
        ///// <returns></returns>
        //[HttpGet]
        //public string Login(string userName, string password)
        //{
        //    //if (!ValidateUser(userName, password)) //用户名密码验证通过以后
        //    //{
        //    //    return Newtonsoft.Json.JsonConvert.SerializeObject(new { bRes = false });
        //    //}
        //    FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(0, userName, DateTime.Now,
        //                    DateTime.Now.AddHours(20), true, string.Format("{0}&{1}", userName, password),
        //                    FormsAuthentication.FormsCookiePath);
        //    return Newtonsoft.Json.JsonConvert.SerializeObject(new { bRes = true, Ticket = FormsAuthentication.Encrypt(ticket) }); ;
        //}
        [HttpPost]
        [Route("login")]
        public IActionResult Login([FromBody]LoginInput input)
        {
            //从数据库验证用户名，密码 
            //验证通过 否则 返回Unauthorized

            //创建claim
            var authClaims = new[] {
                new Claim(JwtRegisteredClaimNames.Sub,input.Username),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
            };
            IdentityModelEventSource.ShowPII = true;
            //签名秘钥 可以放到json文件中
            //var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("SecureKeySecureKeySecureKeySecureKeySecureKeySecureKey"));

            //var token = new JwtSecurityToken(
            //       //issuer: "https://www.cnblogs.com/chengtian",
            //      // audience: "https://www.cnblogs.com/chengtian",
            //       issuer: "https://www.enok.com",
            //       audience: "https://www.wangdequan.com",
            //       expires: DateTime.Now.AddHours(2),
            //       claims: authClaims,
            //       signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            //       );
            var key = new SymmetricSecurityKey(
               Encoding.UTF8.GetBytes(_configuration["Authentication:JwtBearer:SecurityKey"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                _configuration["Authentication:JwtBearer:Issuer"],
                _configuration["Authentication:JwtBearer:Audience"],
                claims: authClaims,
                expires: DateTime.Now.AddHours(2),
                signingCredentials: credentials
            );
            //返回token和过期时间
            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiration = token.ValidTo
            });
        }
        /// <summary>
        /// 通过SessionUser获取AccessToken
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private string GetAccessToken(SessionUser user)
        {
            var claims = new[]
            {
                new Claim(JwtClaimTypes.Id, user.Id.ToString()),
                new Claim(JwtClaimTypes.Name, user.Name),
                new Claim(JwtClaimTypes.Role, "user")
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Authentication:JwtBearer:SecurityKey"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                _configuration["Authentication:JwtBearer:Issuer"],
                _configuration["Authentication:JwtBearer:Audience"],
                claims,
                expires: DateTime.Now.AddHours(2),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        [HttpPost("/Logins")]
        public JsonResult Login([FromQuery]string username, string password, string rolename)
        {
            // 用户名密码是否正确	
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(rolename))
            {
                return new JsonResult(new
                {
                    Code = 0,
                    Message = "传入参数不完整",
                });
            }
            if (!((username == "aa" || username == "bb" || username == "cc") && password == "123456"))
            {
                return new JsonResult(new
                {
                    Code = 0,
                    Message = "账号或密码错误",
                });
            }
            // 你自己定义的角色/用户信息服务	
            RoleService roleService = new RoleService();
            // 检验用户是否属于此角色	
            var role = roleService.IsUserToRole(username, rolename);
            // CZGL.Auth 中一个用于加密解密的类	
            EncryptionHash hash = new EncryptionHash();
            // 设置用户标识	
            var userClaims = hash.BuildClaims(username, rolename);

            //// 自定义构建配置用户标识	
            /// 自定义的话，至少包含如下标识	
            //var userClaims = new Claim[]	
            //{	
            //new Claim(ClaimTypes.Name, userName),	
            //    new Claim(ClaimTypes.Role, roleName),	
            //    new Claim(JwtRegisteredClaimNames.Aud, Audience),	
            //    new Claim(ClaimTypes.Expiration, TimeSpan.TotalSeconds.ToString()),	
            //    new Claim(JwtRegisteredClaimNames.Iat, new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds().ToString())	
            //};	
            /*	
            iss (issuer)：签发人	
            exp (expiration time)：过期时间	
            sub (subject)：主题	
            aud (audience)：受众	
            nbf (Not Before)：生效时间	
            iat (Issued At)：签发时间	
            jti (JWT ID)：编号	
            */
            // 方法一，直接颁发 Token	
            ResponseToken token = hash.BuildToken(userClaims);
            //方法二，拆分多步，颁发 token，方便调试	
            //var identity = hash.GetIdentity(userClaims);	
            //var jwt = hash.BuildJwtToken(userClaims);	
            //var token = hash.BuildJwtResponseToken(jwt);	 
            return new JsonResult(token);
        }
        [AllowAnonymous]
        [HttpGet]
        [Route("api/nopermission")]
        public IActionResult NoPermission()
        {
            return Forbid("No Permission!");
        }

        /// <summary>
        /// login
        /// </summary>
        /// <param name="userName">只能用user或者</param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        [Route("api/auth")]
        public IActionResult Get(string userName, string pwd)
        {
            if (CheckAccount(userName, pwd, out string role))
            {
                //每次登陆动态刷新
                Const.ValidAudience = userName + pwd + DateTime.Now.ToString();
                // push the user’s name into a claim, so we can identify the user later on.
                //这里可以随意加入自定义的参数，key可以自己随便起
                var claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Nbf,$"{new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds()}") ,
                    new Claim (JwtRegisteredClaimNames.Exp,$"{new DateTimeOffset(DateTime.Now.AddMinutes(30)).ToUnixTimeSeconds()}"),
                    new Claim(ClaimTypes.NameIdentifier, userName),
                    new Claim("Role", role)
                };
                //sign the token using a secret key.This secret will be shared between your API and anything that needs to check that the token is legit.
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Const.SecurityKey));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                //.NET Core’s JwtSecurityToken class takes on the heavy lifting and actually creates the token.
                var token = new JwtSecurityToken(
                    //颁发者
                    issuer: Const.Domain,
                    //接收者
                    audience: Const.ValidAudience,
                    //过期时间
                    expires: DateTime.Now.AddMinutes(30),
                    //签名证书
                    signingCredentials: creds,
                    //自定义参数
                    claims: claims
                    );

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token)
                });
            }
            else
            {
                return BadRequest(new { message = "username or password is incorrect." });
            }
        }

        /// <summary>
        /// 模拟登陆校验，因为是模拟，所以逻辑很‘模拟’
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="pwd"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        private bool CheckAccount(string userName, string pwd, out string role)
        {
            role = "user";

            if (string.IsNullOrEmpty(userName))
                return false;

            if (userName.Equals("admin"))
                role = "admin";

            return true;
        }
    }
}