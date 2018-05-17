using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Newsify.DAL
{
    public class ArticleRepo : Repository<Article>, IArticleRepo
    {
        public ArticleRepo(NewsDBEntities context) : base(context)
        {
        }

        public override bool Validation(Article entity)
        {
            //TODO: Validation of properties.
            
            return true;
        }
    }
}
