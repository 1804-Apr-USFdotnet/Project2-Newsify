using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newsify.DAL;

namespace Newsify.Logic
{
    public class DataAccess
    {
        #region Comments
        public void AddComment(Comment comment, string author, int articleId)
        {
            try
            {
                if (comment == null)
                    throw new NotImplementedException();

                using (var uow = new UnitOfWork(new NewsDBEntities()))
                {
                    uow.CommentR.Create(comment); // Add the new comment to the DB
                    uow.Complete(); // Save the changes

                    var post = new Post()
                    {
                        ArticleID = articleId,
                        UserID = uow.UserR.SearchFor(u => u.UserName == author).FirstOrDefault().ID,
                        CommentID = uow.CommentR.SearchFor(c => c.CommentedAt == comment.CommentedAt).LastOrDefault().ID
                    };
                    uow.PostR.Create(post);// Add record in the Posts Table
                    uow.Complete(); // save the changes
                }
            }
            catch (NotImplementedException nex)
            {
                // Log error here
            }
            catch (Exception ex)
            {
                // Log error here
            }
        }

        public List<Comment> GetComments(int articleId)
        {
            try
            {
                using (var uow = new UnitOfWork(new NewsDBEntities()))
                {
                    var posts = uow.PostR.SearchFor(p => p.ArticleID == articleId).ToList();
                    List<Comment> comments = new List<Comment>();
                    foreach (var post in posts)
                    {
                        var comm = uow.CommentR.Get(post.CommentID);
                        if (comm.Active)
                            comments.Add(comm);
                    }

                    return comments;
                }
            }
            catch (Exception ex)
            {
                // log error here
                return null;
            }
        }

        public Comment GetComment(int commentId)
        {
            try
            {
                using (var uow = new UnitOfWork(new NewsDBEntities()))
                {
                    return uow.CommentR.SearchFor(c => c.ID == commentId && c.Active).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                // log error here
                return null;
            }
        }

        public bool UpdateComment(Comment comment)
        {
            try
            {
                using (var uow = new UnitOfWork(new NewsDBEntities()))
                {
                    // Grab the comment from the database and update it
                    var comm = uow.CommentR.SearchFor(c => c.ID == comment.ID).FirstOrDefault();
                    comm.Comment1 = comment.Comment1;
                    comm.Modified = comment.Modified;
                    uow.Complete(); // save the changes to the database
                }
                return true; // successfully updated the comment
            }
            catch (Exception ex)
            {
                // log error here
                return false; // failed to update the comment
            }
        }

        public bool DeleteComment(int commentId)
        {
            try
            {
                using (var uow = new UnitOfWork(new NewsDBEntities()))
                {
                    // Grab the comment from the database and update it
                    var comm = uow.CommentR.SearchFor(c => c.ID == commentId).FirstOrDefault();
                    comm.Active = false; // mark the comment as deleted
                    uow.Complete(); // save the changes to the database
                }
                return true; // successfully updated the comment
            }
            catch (Exception ex)
            {
                // log error here
                return false; // failed to delete the comment
            }
        }
        #endregion Comments

        #region Articles
        // Search and return a list of articles from the given Source
        public List<Article> GetArticles(Source source)
        {
            try
            {
                var articles = new List<Article>();
                using (var uow = new UnitOfWork(new NewsDBEntities()))
                {
                    var sources = new List<Source>();
                    // Grab all matching sources in the database
                    if (source.Country != null)
                    {
                        sources = uow.SourceR.SearchFor(s => s.Country == source.Country).ToList(); // Articles from given country
                    }
                    else if (source.Language != null)
                    {
                        sources = uow.SourceR.SearchFor(s => s.Language == source.Language).ToList(); // Articles in given language
                    }
                    else
                    {
                        sources = uow.SourceR.SearchFor(s => s.Name == source.Name).ToList(); // Articles from given source
                    }

                    // Find all articles from the possible sources
                    foreach (var src in sources)
                    {
                        if (articles.Count == 100)
                        {
                            // Let's break out of the foreach loop; We've reached the maximum number of articles for the search
                            break;
                        }
                        var arts = uow.ArticleR.SearchFor(a => a.Source == src.PK && a.Active).ToList(); // All articles from the src
                        foreach (var article in arts)
                        {
                            if (articles.Count == 100)
                            {
                                // We've reached the maximum number of articles for the search
                                break;
                            }
                            articles.Add(article);
                        }
                    }
                }

                return articles;
            }
            catch (Exception ex)
            {
                // Log error here
                return null;
            }
        }

        // Search and return a list of articles published on the given date
        public List<Article> GetArticles(DateTime publishedDate)
        {
            try
            {
                var articles = new List<Article>();
                using (var uow = new UnitOfWork(new NewsDBEntities()))
                {
                    // Find all articles that match the search criteria
                    articles = uow.ArticleR.SearchFor(a => a.PublishAt == publishedDate && a.Active).ToList();
                    
                    if (articles.Count > 100)
                    {
                        // Only want to return a maximum of hundred articles
                        var itemsToRemove = articles.Count - 100;
                        articles.RemoveRange(100, itemsToRemove); // Remove all the extra articles
                    }
                }

                return articles;
            }
            catch (Exception ex)
            {
                // Log error here
                return null;
            }
        }

        // Search and return a list of articles with matching criteria
        public List<Article> GetArticles(string title = null, string topic = null)
        {
            try
            {
                var articles = new List<Article>();
                using (var uow = new UnitOfWork(new NewsDBEntities()))
                {
                    // Find all articles that match the search criteria
                    if (title != null)
                    {
                        articles = uow.ArticleR.SearchFor(a => a.Title.Contains(title) && a.Active).ToList();
                    }
                    else if (topic != null)
                    {
                        articles = uow.ArticleR.SearchFor(a => a.Topic.Contains(topic) && a.Active).ToList();
                    }

                    if (articles.Count > 100)
                    {
                        // Only want to return a maximum of hundred articles
                        var itemsToRemove = articles.Count - 100;
                        articles.RemoveRange(100, itemsToRemove); // Remove all the extra articles
                    }
                }

                return articles;
            }
            catch (Exception ex)
            {
                // Log error here
                return null;
            }
        }
        #endregion Articles
    }
}
