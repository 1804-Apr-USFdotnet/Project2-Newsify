using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Newsify.DAL
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        public DbContext DB { get; set; }

        public Repository(DbContext db)
        {
            DB = db;
        }

        public TEntity Create(TEntity entity)
        {
            if (Validation(entity))
            {
                DB.Set<TEntity>().Add(entity);
                return entity;
            } else
            {
                return null;
            }
        }

        public TEntity Delete(TEntity entity)
        {
            DB.Set<TEntity>().Remove(entity);
            return entity;
        }

        public IEnumerable<TEntity> DeleteRange(IEnumerable<TEntity> entities)
        {
            DB.Set<TEntity>().RemoveRange(entities);
            return entities;
        }

        public TEntity Get(int id)
        {
            var temp = DB.Set<TEntity>().Find(id);
            return temp;
        }

        public IEnumerable<TEntity> GetAll()
        {
            return DB.Set<TEntity>();
        }

        public IEnumerable<TEntity> SearchFor(Expression<Func<TEntity, bool>> predicate)
        {
            return DB.Set<TEntity>().Where(predicate);
        }

        public TEntity Update(int id, TEntity entity)
        {
            if (Validation(entity))
            {
                var oldEntity = DB.Set<TEntity>().Find(id);
                DB.Entry(oldEntity).CurrentValues.SetValues(entity);
                return entity;
            }
            else
            {
                return null;
            }
        }

        public virtual bool Validation(TEntity entity)
        {
            return false;
        }
    }
}
