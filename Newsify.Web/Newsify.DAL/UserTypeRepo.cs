using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Newsify.DAL
{
    class UserTypeRepo : Repository<UserType>, IUserTypeRepo
    {
        public UserTypeRepo(NewsDBEntities context) : base(context)
        {

        }

        public override bool Validation(UserType entity)
        {
            //TODO: Validation of properties.

            return true;
        }
    }
}
