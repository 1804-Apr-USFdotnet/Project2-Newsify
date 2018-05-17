using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Newsify.DAL
{
    class SourceRepo : Repository<Source>, ISourceRepo
    {
        public SourceRepo(NewsDBEntities context) : base(context)
        {

        }

        public override Source Delete(Source entity)
        {
            return base.Delete(entity);
        }

        public override bool Validation(Source entity)
        {
            //TODO: Validation of properties.

            return true;
        }
    }
}
