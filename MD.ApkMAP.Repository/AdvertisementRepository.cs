using Dapper;
using MD.ApkMAP.IRepository;
using MD.ApkMAP.Model.DBModels;
using MD.ApkMAP.Repository.Base;
using MD.ApkMAP.Repository.sugar;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MD.ApkMAP.Repository
{
    /// <summary>
    /// 仓储层，具体数据库的实现
    /// </summary>
    public class AdvertisementRepository : BaseRepository<AdvertTest>, IAdvertisementRepository
    {
 
        public int Sum(int i, int j)
        {
            return i + j;
        }
        
    }
}
