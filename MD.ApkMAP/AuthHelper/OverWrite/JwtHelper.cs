using MD.ApkMAP.AOP;
using MD.ApkMAP.Common;
using MD.ApkMAP.Common.Helper;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MD.ApkMAP.AuthHelper.OverWrite
{
    public class JwtHelper
    {
        /// <summary>
        /// 获取JWTkey
        /// </summary>
       // public static readonly string secretKey = Appsettings.app(new string[] { "JwtAuth", "SecurityKey" });

        /// <summary>
        /// 颁发JWT字符串
        /// </summary>
        /// <param name="tokenModel"></param>
        /// <returns></returns>
        public static string IssueJWT(TokenModel tokenModel)
        {
            var dateTime = DateTime.UtcNow;

            string iss = Appsettings.app(new string[] { "Audience", "Issuer" });
            string aud = Appsettings.app(new string[] { "Audience", "Audience" });
            string secret = Appsettings.app(new string[] { "Audience", "Secret" });

            //var claims = new Claim[]
            //{
            //    new Claim(JwtRegisteredClaimNames.Jti,tokenModel.Uid.ToString()),//Id
            //    new Claim("Role", tokenModel.Role),//角色
            //    new Claim(JwtRegisteredClaimNames.Iat,$"{new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds()}"),  
            new Claim(JwtRegisteredClaimNames.Exp, $"{new DateTimeOffset(DateTime.Now.AddSeconds(10)).ToUnixTimeSeconds()}");
            //};

            var claims = new Claim[]
                {
                    //下边为Claim的默认配置
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, $"{new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds()}"),
                new Claim(JwtRegisteredClaimNames.Nbf,$"{new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds()}") ,
                //这个就是过期时间，目前是过期100秒，可自定义，注意JWT有自己的缓冲过期时间
                new Claim (JwtRegisteredClaimNames.Exp,$"{new DateTimeOffset(DateTime.Now.AddSeconds(100)).ToUnixTimeSeconds()}"),
                new Claim(JwtRegisteredClaimNames.Iss,iss),
                new Claim(JwtRegisteredClaimNames.Aud,aud),
                //这个Role是官方UseAuthentication要要验证的Role，我们就不用手动设置Role这个属性了
                new Claim(ClaimTypes.Role,tokenModel.Role),
               };


            //秘钥
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var jwt = new JwtSecurityToken(
                issuer: "MD.ApkMAP",
                claims: claims,
                signingCredentials: creds);

            var jwtHandler = new JwtSecurityTokenHandler();
            var encodedJwt = jwtHandler.WriteToken(jwt);

            return encodedJwt;
        }

        #region 老方法，过期时间无效


        ///// <summary>
        ///// 获取JWT字符串并存入缓存
        ///// </summary>
        ///// <param name="tokenModel"></param>
        ///// <param name="expiresSliding"></param>
        ///// <param name="expiresAbsoulte"></param>
        ///// <returns></returns>
        //public static string IssueJWT(TokenModel tokenModel)
        //{
        //    DateTime UTC = DateTime.UtcNow;
        //    //过期时间
        //    double exp = Appsettings.app(new string[] { "JwtAuth", "WebExp" }).ObjToMoney();
        //    Claim[] claims = new Claim[]
        //    {
        //        new Claim(JwtRegisteredClaimNames.Jti,tokenModel.Uid.ToString()),//用户Id

        //        new Claim("Role", "Client"),//身份
        //        new Claim("JsonUsr", Newtonsoft.Json.JsonConvert.SerializeObject(tokenModel)),//身份
        //       //new Claim(JwtRegisteredClaimNames.Exp, DateTime.Now.AddMinutes(exp).ToString("yyyy-MM-dd hh:mm:ss")),
        //        new Claim(JwtRegisteredClaimNames.Iat, UTC.ToLocalTime().ToString(), ClaimValueTypes.Integer64),//Issued At，JWT颁发的时间，采用标准unix时间，用于验证过期
        //    };




        //    //秘钥
        //    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtHelper.secretKey));
        //    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        //    JwtSecurityToken jwt = new JwtSecurityToken(
        //    issuer: "MDApkMAP",//jwt签发者,非必须
        //    //audience: tokenModel.Uname,//jwt的接收该方，非必须
        //    claims: claims,//声明集合
        //    expires: UTC.AddMinutes(exp).ToLocalTime(),//指定token的生命周期，非必须
        //    //expires: DateTime.Now.AddMinutes(exp),
        //    notBefore: UTC.ToLocalTime(),

        //    signingCredentials: creds);

        //    var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);//生成最后的JWT字符串

        //    //TODO考虑加入缓存
        //    //ApkMapMemoryCache.AddMemoryCache(encodedJwt, tokenModel, expiresSliding, expiresAbsoulte);//将JWT字符串和tokenModel作为key和value存入缓存

        //    return encodedJwt;
        //}

        #endregion


        /// <summary>
        /// 解析
        /// </summary>
        /// <param name="jwtStr"></param>
        /// <returns></returns>
        public static TokenModel SerializeJWT(string jwtStr)
        {
            var jwtHandler = new JwtSecurityTokenHandler();
            JwtSecurityToken jwtToken = jwtHandler.ReadJwtToken(jwtStr);
            object role = new object(); ;
            try
            {
                jwtToken.Payload.TryGetValue("Role", out role);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            var tm = new TokenModel
            {
                Uid = (jwtToken.Id).ObjToInt(),
                Role = role != null ? role.ObjToString() : "",
            };
            return tm;
        }

        ///// <summary>
        ///// 解析
        ///// </summary>
        ///// <param name="jwtStr"></param>
        ///// <returns></returns>
        //public static TokenModel SerializeJWT(string jwtStr)
        //{
        //    var jwtHandler = new JwtSecurityTokenHandler();
        //    JwtSecurityToken jwtToken = jwtHandler.ReadJwtToken(jwtStr);
        //    object role = new object();
        //    object str = new object();
        //    var tm = new TokenModel();
        //    try
        //    {
        //        jwtToken.Payload.TryGetValue("Role", out role);
        //        jwtToken.Payload.TryGetValue("JsonUsr", out str);
        //        tm = Newtonsoft.Json.JsonConvert.DeserializeObject<TokenModel>(str.ToString());
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e);
        //        throw;
        //    }
        //    //var tm = new TokenModel
        //    //{
        //    //    Uid = (jwtToken.Id).ObjToInt(),
        //    //    Role = role != null ? role.ObjToString() : "",
        //    //};
        //    return tm;
        //}
    }

    /// <summary>
    /// 令牌类
    /// </summary>
    public class TokenModel
    {
        public TokenModel()
        {
            this.Uid = 0;
        }
        /// <summary>
        /// 用户Id
        /// </summary>
        public long Uid { get; set; }


        /// <summary>
        /// 身份
        /// </summary>
        public string Role { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string Uname { get; set; }
        /// <summary>
        /// 手机
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 头像
        /// </summary>
        public string Icon { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        public string UNickname { get; set; }
        /// <summary>
        /// 身份
        /// </summary>
        public string Sub { get; set; }

        /// <summary>
        /// 网络
        /// </summary>
        public string Project { get; set; } = "mingdao";
    }
}
