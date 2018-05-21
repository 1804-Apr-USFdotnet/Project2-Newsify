using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Newsify.DAL
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly NewsDBEntities context;

        public UnitOfWork(NewsDBEntities newsDB)
        {
            context = newsDB;

            ArticleR = new ArticleRepo(newsDB);
            PostR = new PostRepo(newsDB);
            CommentR = new CommentRepo(newsDB);
            SourceR = new SourceRepo(newsDB);
            UserR = new UserRepo(newsDB);
            //UserTypeR = new UserTypeRepo(newsDB);
        }

        public IArticleRepo ArticleR { get; private set; }

        public IPostRepo PostR { get; private set; }

        public ICommentRepo CommentR { get; private set; }

        public ISourceRepo SourceR { get; private set; }

        public IUserRepo UserR { get; private set; }

        public IUserTypeRepo UserTypeR { get; private set; }

        public int Complete()
        {
            return context.SaveChanges();
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    context.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~UnitOfWork() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion

    }
}
