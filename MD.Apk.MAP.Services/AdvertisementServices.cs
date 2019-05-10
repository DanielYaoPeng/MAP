using MD.ApkMAP.IRepository;
using MD.ApkMAP.IServices;
using MD.ApkMAP.Model.DBModels;
using MD.ApkMAP.Repository;
using MD.ApkMAP.Services.Base;
using System;

namespace MD.ApkMAP.Services
{
    /// <summary>
    /// 业务层，处理具体业务逻辑
    /// </summary>
    public class AdvertisementServices : BaseServices<AdvertTest>, IAdvertisementServices
    {
        IAdvertisementRepository _dal;

        public AdvertisementServices(IAdvertisementRepository dal)
        {
            _dal = dal;
        }

        public int Sum(int i, int j)
        {
            //这里都是处理你的业务逻辑的，业务逻辑代码放在这里
            return _dal.Sum(i, j);

        }


    }
}
