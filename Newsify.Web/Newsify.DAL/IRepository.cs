using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data;

namespace Newsify.DAL
{
    public interface IRepository<TEntity> where TEntity : class
    {
        DbContext DB { get; set; }

        TEntity Create(TEntity entity);

        TEntity Get(int id);
        IEnumerable<TEntity> GetAll();
        IEnumerable<TEntity> SearchFor(Expression<Func<TEntity, bool>> predicate);

        TEntity Update(int id, TEntity entity);

        TEntity Delete(TEntity entity);
        IEnumerable<TEntity> DeleteRange(IEnumerable<TEntity> entities);

        bool Validation(TEntity entity);
    }
}
