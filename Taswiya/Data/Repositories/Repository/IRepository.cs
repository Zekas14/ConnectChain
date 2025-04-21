using ConnectChain.Helpers;
using ConnectChain.Models;
using System.Linq.Expressions;

namespace ConnectChain.Data.Repositories.Repository
{
    public interface IRepository<Entity>
    {
        void Add(Entity entity);
        Task AddAsync(Entity entity);
        void SaveInclude(Entity entity, params string[] properties);
        void Delete(Entity entity);
        void HardDelete(Entity entity);
        IQueryable<Entity> GetAll();
        IQueryable<Entity> GetAllWithDeleted();
        IQueryable<Entity> Get(Expression<Func<Entity, bool>> predicate);
        IQueryable<Entity> GetAllByPage(PaginationHelper paginationParams);
        IQueryable<Entity> GetByPage(Expression<Func<Entity, bool>> predicate, PaginationHelper paginationParams);
        public IQueryable<Entity> GetAllWithIncludes(Func<IQueryable<Entity>, IQueryable<Entity>> includeExpression);
        Task<bool> AnyAsync(Expression<Func<Entity, bool>> predicate);
        Entity GetByID(int id);
        Entity GetByIDWithIncludes(int id, Func<IQueryable<Entity>, IQueryable<Entity>> includeExpression);
        Task<Entity> GetByIDAsync(int id);
        void SaveChanges();
        Task SaveChangesAsync();
        void AddRange(ICollection<Entity> images);
    }
}
