using System.Linq.Expressions;

namespace ConnectChain.Data.Repositories.Repository
{
    public interface IRepository<Entity>
    {
        void Add(Entity entity);
        void SaveInclude(Entity entity, params string[] properties);
        void Delete(Entity entity);
        void HardDelete(Entity entity);
        IQueryable<Entity> GetAll();
        IQueryable<Entity> GetAllWithDeleted();
        IQueryable<Entity> Get(Expression<Func<Entity, bool>> predicate);

        Task<bool> AnyAsync(Expression<Func<Entity, bool>> predicate);
        Entity GetByID(int id);

        Task<Entity> GetByIDAsync(int id);
        void SaveChanges();
    }
}
