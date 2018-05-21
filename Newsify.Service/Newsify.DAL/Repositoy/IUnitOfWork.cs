using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Newsify.DAL
{
    public interface IUnitOfWork : IDisposable
    {
        ArticleRepo ArticleR { get; }
        PostRepo PostR { get; }
        CommentRepo CommentR { get; }
        SourceRepo SourceR { get; }
        UserRepo UserR { get; }


        int Complete();
    }
}
