using MD.ApkMAP.IRepository;
using MD.ApkMAP.IServices;
using MD.ApkMAP.Repository;
using System;

namespace MD.ApkMAP.Services
{
    /// <summary>
    /// 业务层，处理具体业务逻辑
    /// </summary>
    public class AdvertisementServices: IAdvertisementServices
    {
        //实例化仓储层
        IAdvertisementRepository dal = new AdvertisementRepository();

        public int Sum(int i, int j)
        {
            return dal.Sum(i, j);

        }
    }
}
