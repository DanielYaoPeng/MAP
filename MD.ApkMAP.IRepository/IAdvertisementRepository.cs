using System;

namespace MD.ApkMAP.IRepository
{
    /// <summary>
    /// 仓储定义接口    管理数据持久 负责数据的CRUD
    /// </summary>
    public interface IAdvertisementRepository
    {
        int Sum(int i, int j);
    }
}
