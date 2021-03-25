using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using SqlSugar;
using SqlSugar.IOC;
using WY.Model.Entity;

namespace WY.Data
{
    public class BaseRepository<TEntity> : SimpleClient<TEntity>, IBaseRepository<TEntity> where TEntity : class, new()
    {
        public BaseRepository(ISqlSugarClient context = null) : base(context)
        {
            base.Context = DbScoped.Sugar;

            //base.Context.DbMaintenance.CreateDatabase();

            base.Context.CodeFirst.InitTables
                (
                typeof(Admin)
                );
        }
        public async Task<bool> CrateAsync(TEntity entity)
        {
            return await base.InsertAsync(entity);
        }

        public async Task<bool> DeleteAsync(int Id)
        {
            return await base.DeleteByIdAsync(Id);
        }

        public async Task<bool> EditAsync(TEntity entity)
        {
            return await base.UpdateAsync(entity);
        }

        public virtual async Task<TEntity> GetAsync(int Id)
        {
            return await base.GetByIdAsync(Id);
        }

        public virtual async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> func)
        {
            return await base.GetSingleAsync(func);
        }

        public virtual async Task<List<TEntity>> QueryAsunc(Expression<Func<TEntity, bool>> func)
        {
            return await base.GetListAsync(func);
        }

        public virtual async Task<List<TEntity>> QueryAsunc(int page, int size, RefAsync<int> total)
        {
            return await base.Context.Queryable<TEntity>().ToPageListAsync(page, size, total);
        }

        public virtual async Task<List<TEntity>> QueryAsunc(Expression<Func<TEntity, bool>> func, int page, int size, RefAsync<int> total)
        {
            return await base.Context.Queryable<TEntity>().Where(func).ToPageListAsync(page, size, total);
        }
    }
}
