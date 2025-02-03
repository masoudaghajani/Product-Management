

using Domain.IRepository;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Repository
{
    public class RepositoryAsync<T> : IRepositoryAsync<T> where T : class
    {

        private readonly ApplicationDbContext _db;
        internal DbSet<T> dbSet;

        public RepositoryAsync(ApplicationDbContext db)
        {
            _db = db;
            this.dbSet = _db.Set<T>();
        }
        public async Task AddAsync(T entity, CancellationToken cancellationToken)
        {
            await dbSet.AddAsync(entity, cancellationToken);
            await _db.SaveChangesAsync();
        }
        public async Task AddRangeAsync(IEnumerable<T> entity, CancellationToken cancellationToken)
        {
            await dbSet.AddRangeAsync(entity,cancellationToken);
        }
        public async Task<T> GetAsync(Guid id,CancellationToken cancellationToken)
        {
            var info= await dbSet.FindAsync(id, cancellationToken).ConfigureAwait(false); 
            return info;
        }      
        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = null)
        {

            IQueryable<T> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }
            
            if (includeProperties != null)
            {
                foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp).AsSplitQuery();
                }
            }

            if (orderBy != null)
            {
                return await orderBy(query).AsNoTracking().ToListAsync();
            }
            return await query.AsNoTracking().ToListAsync().ConfigureAwait(false);        
        }   
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            IQueryable<T> query = dbSet;    
            return await query.AsNoTracking().ToListAsync().ConfigureAwait(false); 
        }
       
        public async Task<T> GetFirstOrDefaultAsync(Expression<Func<T, bool>> filter = null, string includeProperties = null)
        {
            IQueryable<T> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includeProperties != null)
            {
                foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp).AsSplitQuery();
                }
            }


            return await query.AsNoTracking().FirstOrDefaultAsync().ConfigureAwait(false); 
        }
        public async Task<T> GetAsync(Expression<Func<T, bool>> filter = null, string includeProperties = null)
        {
            IQueryable<T> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includeProperties != null)
            {
                foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp).AsSplitQuery();
                }
            }


            return await query.AsNoTracking().FirstOrDefaultAsync().ConfigureAwait(false);
        }
        public async Task RemoveAsync(Guid id)
        {
            T entity = await dbSet.FindAsync(id);
            await RemoveAsync(entity);
            await _db.SaveChangesAsync();
        }
        public async Task RemoveAsync(T entity)
        {
            dbSet.Remove(entity);
        }    
        public async Task RemoveRangeAsync(IEnumerable<T> entity)
        {
            dbSet.RemoveRange(entity);
        }
        public IQueryable<T> SelectPaged(
          Expression<Func<T, bool>> filter = null,
          Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
          List<Expression<Func<T, object>>> includes = null,
          int? page = null,
          int? pageSize = null)
        {
            IQueryable<T> query = _db.Set<T>();

            if (includes != null)
            {
                query = includes.Aggregate(query, (current, include) => current.Include(include));
            }
            if (orderBy != null)
            {
                query = orderBy(query);
            }
            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (page != null && pageSize != null)
            {
                query = query.Skip((page.Value - 1) * pageSize.Value).Take(pageSize.Value);
            }
            return query;
        }

        public async Task<IEnumerable<T>> SelectPagedAsync(
           Expression<Func<T, bool>> filter = null,
           Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
           List<Expression<Func<T, object>>> includes = null,
           int? page = null,
           int? pageSize = null)
        {
            return await SelectPaged(filter, orderBy, includes, page, pageSize).ToListAsync();
        }

        public async Task<IEnumerable<TResult>> GetAllAsync<TResult>(
         Expression<Func<T, bool>> filter = null,
         Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
         Expression<Func<T, TResult>> selector = null,
         string includeProperties = null
         )
        {
            IQueryable<T> query = dbSet;

            // Apply filter if provided  
            if (filter != null)
            {
                query = query.Where(filter);
            }

            // Apply ordering if provided  
            if (orderBy != null)
            {
                query = orderBy(query);
            }
            if (includeProperties != null)
            {
                foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp).AsSplitQuery();
                }
            }
            // Apply selection if provided  
            if (selector != null)
            {
                return await query.AsNoTracking().Select(selector).ToListAsync().ConfigureAwait(false); 
            }

            // For no specific selection, return the entire entity  
            return (IEnumerable<TResult>)await query.AsNoTracking().ToListAsync().ConfigureAwait(false);
        }

        //example
        //public async Task<ActionResult<IEnumerable<PersonDto>>> GetAll(string name = null, string sortOrder = null)
        //{
        //    var people = await _repository.GetAllAsync<PersonDto>(
        //        filter: person => string.IsNullOrEmpty(name) || person.Name.Contains(name),
        //        orderBy: q => sortOrder == "age" ? q.OrderBy(p => p.Age) : q.OrderBy(p => p.Name),
        //        selector: person => new PersonDto
        //        {
        //            Id = person.Id,
        //            Name = person.Name,
        //            Age = person.Age
        //        });

        //    return Ok(people);
        //}
    }
}
