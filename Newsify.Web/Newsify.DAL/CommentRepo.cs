using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Newsify.DAL
{
    class CommentRepo : Repository<Comment>, ICommentRepo
    {
        public CommentRepo(NewsDBEntities context) : base(context)
        {

        }

        public override bool Validation(Comment entity)
        {
            //TODO: Validation of properties.

            return true;
        }
    }
}
