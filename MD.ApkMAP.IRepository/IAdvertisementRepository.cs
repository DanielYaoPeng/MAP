using MD.ApkMAP.Model.DBModels;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MD.ApkMAP.IRepository
{
    /// <summary>
    /// 仓储定义接口    管理数据持久 负责数据的CRUD
    /// </summary>
    public interface IAdvertisementRepository
    {
        int Sum(int i, int j);

        Task<int> Add(AdvertTest model);
        Task<bool> Delete(AdvertTest model);
        Task<bool> Update(AdvertTest model);
        Task<List<AdvertTest>> Query(Expression<Func<AdvertTest, bool>> whereExpression);
    }
}
