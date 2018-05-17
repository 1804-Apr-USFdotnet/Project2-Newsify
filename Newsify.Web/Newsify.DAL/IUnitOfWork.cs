using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Newsify.DAL
{
    interface IUnitOfWork : IDisposable
    {
        IArticleRepo ArticleR { get; }
        IPostRepo PostR { get; }
        ICommentRepo CommentR { get; }
        ISourceRepo SourceR { get; }
        IUserRepo UserR { get; }
        IUserTypeRepo UserTypeR { get; }


        int Complete();
    }
}
