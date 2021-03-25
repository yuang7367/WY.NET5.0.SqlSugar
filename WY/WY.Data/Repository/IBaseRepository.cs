using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using SqlSugar;

namespace WY.Data
{
    public interface IBaseRepository<TEntity> where TEntity : class, new()
    {
        Task<bool> CrateAsync(TEntity entity);

        Task<bool> DeleteAsync(int Id);

        Task<bool> EditAsync(TEntity entity);

        Task<TEntity> GetAsync(int Id);

        Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> func);

        Task<List<TEntity>> QueryAsunc(Expression<Func<TEntity, bool>> func);

        Task<List<TEntity>> QueryAsunc(int page, int size, RefAsync<int> total);

        Task<List<TEntity>> QueryAsunc(Expression<Func<TEntity, bool>> func, int page, int size, RefAsync<int> total);
    }
}
