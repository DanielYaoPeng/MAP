using MD.ApkMAP.Common;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MD.ApkMAP.AuthHelper.OverWrite
{
    /// <summary>
    /// 
    /// </summary>
    public class JwtTokenAuth
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly RequestDelegate _next;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="next"></param>
        public JwtTokenAuth(RequestDelegate next)
        {
            _next = next;
        }

        /// <summary>
        /// 自定义请求验证中间件（JWT官方不走这个）
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        public Task Invoke(HttpContext httpContext)
        {
            //检测是否包含'Authorization'请求头
            if (!httpContext.Request.Headers.ContainsKey("Authorization"))
            {
                return _next(httpContext);
            }
            var tokenHeader = httpContext.Request.Headers["Authorization"].ToString();

            TokenModel tm = JwtHelper.SerializeJWT(tokenHeader);//序列化token，获取授权

            //授权 注意这个可以添加多个角色声明，请注意这是一个 list
            var claimList = new List<Claim>();
            var claim = new Claim(ClaimTypes.Role, tm.Role);
            claimList.Add(claim);
            var identity = new ClaimsIdentity(claimList);
            var principal = new ClaimsPrincipal(identity);
            httpContext.User = principal;

            return _next(httpContext);
        }


        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="httpContext"></param>
        ///// <returns></returns>
        //public Task Invoke(HttpContext httpContext)
        //{
        //    //检测是否包含'Authorization'请求头
        //    if (!httpContext.Request.Headers.ContainsKey("Authorization"))
        //    {
        //        //httpContext.Response.StatusCode = 400;
        //        //return httpContext.Response.WriteAsync("Bad request.");
        //        return _next(httpContext);
        //    }
        //    var tokenHeader = httpContext.Request.Headers["Authorization"].ToString();

        //    var jwtHandler = new JwtSecurityTokenHandler();
        //    JwtSecurityToken jwtToken = jwtHandler.ReadJwtToken(tokenHeader);
        //    object role = new object();
        //    object str = new object();
        //    //object iat = new object();
        //    object exp = new object();
        //    //TokenModel tm = JwtHelper.SerializeJWT(tokenHeader);
        //    TokenModel tm = new TokenModel();
        //    try
        //    {
        //        jwtToken.Payload.TryGetValue("Role", out role);
        //        jwtToken.Payload.TryGetValue("JsonUsr", out str);
        //        //jwtToken.Payload.TryGetValue(JwtRegisteredClaimNames.Iat, out iat);
        //        jwtToken.Payload.TryGetValue(JwtRegisteredClaimNames.Exp, out exp);
        //        tm = Newtonsoft.Json.JsonConvert.DeserializeObject<TokenModel>(str.ToString());
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }

        //    //授权
        //    var claimList = new List<Claim>();
        //    var claim = new Claim(ClaimTypes.Role, "Client");
        //    claimList.Add(claim);
        //    var fir = Convert.ToInt64(exp).SecondToDateTime();
        //    bool flag = DateTime.Now.ToLocalTime() > fir;
        //    if (flag)
        //    {
        //        httpContext.Response.StatusCode = 401;
        //        return httpContext.Response.WriteAsync("Bad request.");
        //    }

        //    var identity = new ClaimsIdentity(claimList);
        //    var principal = new ClaimsPrincipal(identity);

        //    httpContext.User = principal;

        //    return _next(httpContext);
        //}



    }
}
