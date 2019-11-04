using MD.ApkMAP.IServices.Base;
using MD.ApkMAP.Model.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MD.ApkMAP.IServices
{
    public interface IRoleModulePermissionServices : IBaseServices<RoleModulePermission>
    {

        Task<List<RoleModulePermission>> GetRoleModule();
        Task<List<RoleModulePermission>> TestModelWithChildren();
    }
}
