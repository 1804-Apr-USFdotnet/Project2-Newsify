using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Newsify.DAL
{
    public class PostRepo : Repository<Post>, IPostRepo
    {
        public PostRepo(NewsDBEntities context) : base(context)
        { }

        public override bool Validation(Post entity)
        {
            //TODO: Add validation logic

            return true;
        }
    }
}
