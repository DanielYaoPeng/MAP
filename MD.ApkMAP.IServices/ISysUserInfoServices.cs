using MD.ApkMAP.IServices.Base;
using MD.ApkMAP.Model.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MD.ApkMAP.IServices
{
    public interface ISysUserInfoServices : IBaseServices<sysUserInfo>
    {
        Task<sysUserInfo> SaveUserInfo(string loginName, string loginPwd);
        Task<string> GetUserRoleNameStr(string loginName, string loginPwd);
    }
}
