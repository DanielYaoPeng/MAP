using MD.ApkMAP.AuthHelper.OverWrite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MD.ApkMAP.Controllers
{
    public class LoginController: Controller
    {
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
                tokenModel.Role = "Admin";
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
    }
}
