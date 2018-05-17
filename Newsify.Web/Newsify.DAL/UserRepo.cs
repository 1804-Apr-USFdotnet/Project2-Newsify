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

        public override bool Validation(User entity)
        {
            //TODO: Validation of properties.

            return true;
        }
    }
}
