using System.Linq.Expressions;

namespace Domain.IRepository
{
    public interface IRepositoryAsync<T> where T : class
    {
        Task<T> GetAsync(Guid id, CancellationToken cancellationToken);

        Task<IEnumerable<T>> GetAllAsync(
            Expression<Func<T, bool>> filter = null,
            
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includeProperties = null


            );
        Task<IEnumerable<TResult>> GetAllAsync<TResult>(
       Expression<Func<T, bool>> filter = null,
       Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
       Expression<Func<T, TResult>> selector = null, string includeProperties = null);

       
       
        Task<T> GetFirstOrDefaultAsync(
            Expression<Func<T, bool>> filter = null,
            string includeProperties = null
            );
        Task<T> GetAsync(
           Expression<Func<T, bool>> filter = null,
           string includeProperties = null
           );
        Task AddAsync(T entity, CancellationToken cancellationToken);
        Task AddRangeAsync(IEnumerable<T> entity, CancellationToken cancellationToken);
        Task RemoveAsync(Guid id);
        Task RemoveAsync(T entity);
     
        Task RemoveRangeAsync(IEnumerable<T> entity);

        IQueryable<T> SelectPaged(
          Expression<Func<T, bool>> filter = null,
          Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
          List<Expression<Func<T, object>>> includes = null,
          int? page = null,
          int? pageSize = null);

        Task<IEnumerable<T>> SelectPagedAsync(
           Expression<Func<T, bool>> filter = null,
           Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
           List<Expression<Func<T, object>>> includes = null,
           int? page = null,
           int? pageSize = null);

    }
}
