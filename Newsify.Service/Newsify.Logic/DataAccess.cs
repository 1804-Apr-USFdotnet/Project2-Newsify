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
                        comments.Add(uow.CommentR.Get(post.CommentID));
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
                    return uow.CommentR.SearchFor(c => c.ID == commentId).FirstOrDefault();
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
    }
}
