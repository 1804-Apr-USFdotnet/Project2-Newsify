using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Newsify.DAL
{
    public class CommentRepo : Repository<Comment>, ICommentRepo
    {
        public CommentRepo(NewsDBEntities context) : base(context)
        {

        }

        public override Comment Delete(Comment entity)
        {
            entity.Active = false;
            var EntityToDelete = DB.Set<Comment>().Find(entity.ID);
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

        public override bool Validation(Comment entity)
        {
            //TODO: Validation of properties.

            return true;
        }
    }
}
