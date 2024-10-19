using System.Linq.Expressions;

namespace CandidateHubApi.Repository
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task AddAsync(TEntity entity);
        Task UpdateAsync(TEntity entity);
        Task<bool> DeleteAsync(int Id);
        Task<TEntity?> GetByIdAsync(int Id);
        Task<IEnumerable<TEntity>> GetAllAsync(int pageNumber = 1, int pageSize = 10);
        Task<IEnumerable<TEntity>> GetByFilterAsync(Expression<Func<TEntity, bool>> expression);
        IQueryable<TEntity> GetByFilterQuery(Expression<Func<TEntity, bool>> expression);
    }
}
