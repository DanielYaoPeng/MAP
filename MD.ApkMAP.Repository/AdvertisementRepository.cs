using Dapper;
using MD.ApkMAP.IRepository;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace MD.ApkMAP.Repository
{
    /// <summary>
    /// 仓储层，具体数据库的实现
    /// </summary>
    public class AdvertisementRepository : IAdvertisementRepository
    {
        /// <summary>
        /// 数据库连接
        /// </summary>
        protected string DbConnection;

        protected SqlConnection OpenDbConnection()
        {
            if (DbConnection == null)
            {
                throw new Exception("Connection string \"DbConnection\" can not be null.");
            }

            return new SqlConnection(DbConnection);
        }

        #region 查
        ///// <summary>
        ///// 查询所有
        ///// </summary>
        ///// <param name="sql">查询语句</param>
        ///// <param name="param">参数</param>
        ///// <returns></returns>
        //public async Task<IEnumerable<T>> Query(string sql, object param = null)
        //{
        //    using (var conn = OpenDbConnection())
        //    {
        //        conn.Open();
        //        //return await Task.Run(()=> conn.Query<T>(sql, param).ToList()) ;
        //        return await conn.QueryAsync<T>(sql, param);
        //    }
        //}
        #endregion


        public int Sum(int i, int j)
        {
            return i + j;
        }
    }
}
