using MD.ApkMAP.IServices;
using MD.ApkMAP.Model.Models;
using MD.ApkMAP.Services.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MD.ApkMAP.Services
{
    public class RoleModulePermissionServices : BaseServices<RoleModulePermission>, IRoleModulePermissionServices
    {
        Task<List<RoleModulePermission>> IRoleModulePermissionServices.GetRoleModule()
        {
            throw new NotImplementedException();
        }

        Task<List<RoleModulePermission>> IRoleModulePermissionServices.TestModelWithChildren()
        {
            throw new NotImplementedException();
        }
    }
}
