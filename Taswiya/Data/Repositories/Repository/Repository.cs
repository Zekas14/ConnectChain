﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;
using ConnectChain.Data.Context;
using ConnectChain.Data.Repositories.Repository;
using ConnectChain.Models;

namespace HotelSystem.Data.Repository
{
    public class Repository<Entity> : IRepository<Entity> where Entity : BaseModel
    {
        ConnectChainDbContext _context;
        DbSet<Entity> _dbSet;

        public Repository(ConnectChainDbContext context)
        {
            _context = context;
            _dbSet = context.Set<Entity>();
        }

        public void Add(Entity entity)
        {
            entity.CreatedDate = DateTime.Now;
            //entity.CreatedBy = userID;

            _dbSet.Add(entity);
        }

        public void SaveInclude(Entity entity, params string[] properties)
        {
            var local = _dbSet.Local.FirstOrDefault(x => x.ID == entity.ID);

            EntityEntry entry = null!;

            if(local is null)
            {
                entry = _context.Entry(entity);
            }
            else
            {
                entry = _context.ChangeTracker.Entries<Entity>()
                    .FirstOrDefault(x => x.Entity.ID == entity.ID)!;
            }

            foreach (var property in entry.Properties)
            {
                if(properties.Contains(property.Metadata.Name))
                {
                    property.CurrentValue = entity.GetType().GetProperty(property.Metadata.Name)!.GetValue(entity);
                    property.IsModified = true;
                }
            }
        }


        public void Delete(Entity entity)
        {
            entity.Deleted = true;
            SaveInclude(entity, nameof(BaseModel.Deleted));
        }

        public void HardDelete(Entity entity)
        {
            _dbSet.Remove(entity);
        }


        public IQueryable<Entity> Get(Expression<Func<Entity, bool>> predicate)
        {
            return GetAll().Where(predicate);
        }

        public IQueryable<Entity> GetAll()
        {
            return _dbSet.Where(x => ! x.Deleted);
        }

        public IQueryable<Entity> GetAllWithDeleted()
        {
            return _dbSet;
        }

        public Entity GetByID(int id)
        {
            return Get(x => x.ID == id).FirstOrDefault()??null!;
        }

        public async Task<Entity> GetByIDAsync(int id)
        {
            return await Get(x => x.ID == id).FirstOrDefaultAsync() ?? null!;
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public async Task<bool> AnyAsync(Expression<Func<Entity, bool>> predicate)
        {
            return await Get(predicate).AnyAsync();
        }

        public async Task AddAsync(Entity entity)
        {
            entity.CreatedDate = DateTime.Now;
            //entity.CreatedBy = userID;

            await _dbSet.AddAsync(entity);
        }

        public async Task SaveChangesAysnc()
        {
           await _context.SaveChangesAsync();
        }
    }
}
