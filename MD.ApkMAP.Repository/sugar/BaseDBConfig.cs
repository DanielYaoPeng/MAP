using MD.ApkMAP.Common.Helper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Text;

namespace MD.ApkMAP.Repository.sugar
{
    public class BaseDBConfig
    {
        //public static string ConnectionString = File.ReadAllText(@"D:\my-file\dbCountPsw1.txt").Trim();

        public static string ConnectionString = Appsettings.app(new string[] { "AppSettings", "SqlServer", "SqlServerConnection" });
        // Configuration.GetSection("SqlServer")["SqlServerConnection"];
        //正常格式是

        //public static string ConnectionString = "server=.;uid=sa;pwd=sa;database=BlogDB"; 

        //我用配置文件的形式，因为我直接调用的是我的服务器账号和密码，安全起见


    }

}
