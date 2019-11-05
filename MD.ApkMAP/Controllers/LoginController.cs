using MD.ApkMAP.AuthHelper.OverWrite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using MD.ApkMAP.AuthHelper.Policys;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using MD.ApkMAP.IServices;
using MD.ApkMAP.Common.GlobalVar;

namespace MD.ApkMAP.Controllers
{
    public class LoginController : Controller
    {

        readonly ISysUserInfoServices _sysUserInfoServices;

        readonly PermissionRequirement _requirement;


        /// <summary>
        /// 构造函数注入
        /// </summary>
        /// <param name="sysUserInfoServices"></param>
        /// <param name="userRoleServices"></param>
        /// <param name="roleServices"></param>
        /// <param name="requirement"></param>
        public LoginController(ISysUserInfoServices sysUserInfoServices, PermissionRequirement requirement)
        {
            this._sysUserInfoServices = sysUserInfoServices;
            _requirement = requirement;
        }

        /// <summary>
        /// 获取JWT的重写方法，推荐这种，注意在文件夹OverWrite下
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="pass">密码</param>
        /// <returns></returns>
        [HttpGet]
        [Route("Token2")]
        public JsonResult GetJWTStr(string name, string pass)
        {
            string jwtStr = string.Empty;
            bool suc = false;
            //这里就是用户登陆以后，通过数据库去调取数据，分配权限的操作
            //这里直接写死了 尝试缓存
            if (name == "admins" && pass == "admins")
            {
                TokenModel tokenModel = new TokenModel();
                tokenModel.Uid = 1;
                tokenModel.Role = Permissions.Admin;
                tokenModel.Phone = "12342";
                tokenModel.Project = "上海明道";

                jwtStr = JwtHelper.IssueJWT(tokenModel);
                suc = true;
            }
            else
            {
                jwtStr = "login fail!!!";
            }
            var result = new
            {
                data = new { success = suc, token = jwtStr }
            };

            //这是在mac上写的注释ß
            return Json(result);
        }

        /// <summary>
        /// 获取JWT的方法3：整个系统主要方法
        /// </summary>
        /// <param name="name"></param>
        /// <param name="pass"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("JWTToken3.0")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<object> GetJwtToken3(string name = "", string pass = "")
        {
            //string jwtStr = string.Empty;

            //if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(pass))
            //{
            //    return new JsonResult(new
            //    {
            //        Status = false,
            //        message = "用户名或密码不能为空"
            //    });
            //}

            //pass = MD5Helper.MD5Encrypt32(pass);

            //var user = await _sysUserInfoServices.Query(d => d.uLoginName == name && d.uLoginPWD == pass);
            //if (user.Count > 0)
            //{
            //    var userRoles = await _sysUserInfoServices.GetUserRoleNameStr(name, pass);
            //    //如果是基于用户的授权策略，这里要添加用户;如果是基于角色的授权策略，这里要添加角色
            //    var claims = new List<Claim> {
            //        new Claim(ClaimTypes.Name, name),
            //        new Claim(JwtRegisteredClaimNames.Jti, user.FirstOrDefault().uID.ToString()),
            //        new Claim(ClaimTypes.Expiration, DateTime.Now.AddSeconds(_requirement.Expiration.TotalSeconds).ToString()) };
            //    claims.AddRange(userRoles.Split(',').Select(s => new Claim(ClaimTypes.Role, s)));

            //    //用户标识
            //    var identity = new ClaimsIdentity(JwtBearerDefaults.AuthenticationScheme);
            //    identity.AddClaims(claims);

            //    var token = JwtToken.BuildJwtToken(claims.ToArray(), _requirement);
            //    return new JsonResult(token);
            //}
            //else
            //{
            return new JsonResult(new
            {
                success = false,
                message = "认证失败"
            });
            //}



        }

        /// <summary>
        /// 请求刷新Token（以旧换新）
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("RefreshToken")]
        public async Task<object> RefreshToken(string token = "")
        {
            string jwtStr = string.Empty;

            if (string.IsNullOrEmpty(token))
            {
                return new JsonResult(new
                {
                    Status = false,
                    message = "token无效，请重新登录！"
                });
            }
            var tokenModel = JwtHelper.SerializeJwt(token);
            if (tokenModel != null && tokenModel.Uid > 0)
            {
                var user = await _sysUserInfoServices.QueryById(tokenModel.Uid);
                if (user != null)
                {
                    var userRoles = await _sysUserInfoServices.GetUserRoleNameStr(user.uLoginName, user.uLoginPWD);
                    //如果是基于用户的授权策略，这里要添加用户;如果是基于角色的授权策略，这里要添加角色
                    var claims = new List<Claim> {
                    new Claim(ClaimTypes.Name, user.uLoginName),
                    new Claim(JwtRegisteredClaimNames.Jti, tokenModel.Uid.ToString()),
                    new Claim(ClaimTypes.Expiration, DateTime.Now.AddSeconds(_requirement.Expiration.TotalSeconds).ToString()) };
                    claims.AddRange(userRoles.Split(',').Select(s => new Claim(ClaimTypes.Role, s)));

                    //用户标识
                    var identity = new ClaimsIdentity(JwtBearerDefaults.AuthenticationScheme);
                    identity.AddClaims(claims);

                    var refreshToken = JwtToken.BuildJwtToken(claims.ToArray(), _requirement);
                    return new JsonResult(refreshToken);
                }
            }

            return new JsonResult(new
            {
                success = false,
                message = "认证失败"
            });
        }


    }
}
