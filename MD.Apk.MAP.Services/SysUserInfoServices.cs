using MD.ApkMAP.IServices;
using MD.ApkMAP.Model.Models;
using MD.ApkMAP.Services.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MD.ApkMAP.Services
{
    public class SysUserInfoServices : BaseServices<sysUserInfo>, ISysUserInfoServices
    {
        Task<string> ISysUserInfoServices.GetUserRoleNameStr(string loginName, string loginPwd)
        {
            throw new NotImplementedException();
        }

        Task<sysUserInfo> ISysUserInfoServices.SaveUserInfo(string loginName, string loginPwd)
        {
            throw new NotImplementedException();
        }
    }
}
