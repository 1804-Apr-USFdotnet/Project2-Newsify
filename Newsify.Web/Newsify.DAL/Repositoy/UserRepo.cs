using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Newsify.DAL
{
    class UserRepo : Repository<User>, IUserRepo
    {
        public UserRepo(NewsDBEntities context) : base(context)
        {

        }

        public override User Delete(User entity)
        {
            entity.Active = false;
            var EntityToDelete = DB.Set<User>().Find(entity.ID);
            if (EntityToDelete != null && EntityToDelete.Active)
            {
                DB.Entry(EntityToDelete).CurrentValues.SetValues(entity);
                return entity;
            }
            else
            {
                return null;
            }
        }

        public override bool Validation(User entity)
        {
            if (entity.UserName.Length <= 30 && DB.Set<User>().Where(x => x.UserName == entity.UserName) == null)
            {
                return true;
            } else
            {
                return false;
            }
        }
    }
}
