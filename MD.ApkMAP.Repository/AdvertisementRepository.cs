using Dapper;
using MD.ApkMAP.IRepository;
using MD.ApkMAP.Model.DBModels;
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
    public class AdvertisementRepository : IAdvertisementRepository
    {
        private DbContext context;
        private SqlSugarClient db;
        private SimpleClient<AdvertTest> entityDB;

        internal SqlSugarClient Db
        {
            get { return db; }
            private set { db = value; }
        }
        public DbContext Context
        {
            get { return context; }
            set { context = value; }
        }
        public AdvertisementRepository()
        {
            DbContext.Init(BaseDBConfig.ConnectionString);
            context = DbContext.GetDbContext();
            db = context.Db;
            entityDB = context.GetEntityDB<AdvertTest>(db);
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

        public async Task<int> Add(AdvertTest model)
        {
            // throw new NotImplementedException();
            //返回的i是long类型,这里你可以根据你的业务需要进行处理
            var i = await db.Insertable(model).ExecuteCommandIdentityIntoEntityAsync();
            return i.ObjToInt();
        }

        public async Task<bool> Delete(AdvertTest model)
        {
            var i = await db.Deleteable(model).ExecuteCommandAsync();
            return i > 0;
        }

        public async Task<bool> Update(AdvertTest model)
        {
            var i = await db.Updateable(model).ExecuteCommandAsync();
            return i > 0;
        }

        public async Task<List<AdvertTest>> Query(Expression<Func<AdvertTest, bool>> whereExpression)
        {
           // return await Task.Run(()=> entityDB.GetList(whereExpression));
            return await db.Queryable<AdvertTest>().WhereIF(whereExpression != null, whereExpression).ToListAsync();
        }
    }
}
