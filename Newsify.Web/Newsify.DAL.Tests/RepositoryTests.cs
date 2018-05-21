using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Newsify.DAL;
using System.Data.Entity;
using Moq;
using Moq.Internals;

namespace Newsify.DAL.Tests
{
    public class RepositoryTests
    {
        class DbMockHelper<T> where T : class
        {
            public Mock<DbContext> DbMock { get; private set; }

            public DbMockHelper()
            {
                DbMock = new Mock<DbContext>();
                T temp = It.IsAny<T>();
                DbMock.Setup(x => x.Set<T>().Add(temp)).Returns(temp);
            }
        }

        [Fact]
        public void CreateArticle_CreateAnArticle_ReturnArticle()
        {
            var mockContext = new Mock<DbContext>();
            var article = new Article();
            mockContext.Setup(x => x.Set<Article>().Add(article)).Returns(article);
            

            Repository<Article> repository = new Repository<Article>(mockContext.Object);
            var expected = new Article();
            var temp = repository.Create(expected);

            Assert.Equal(expected, temp);
        }

        [Fact]
        public void CreateUser_CreateUser_ReturnUser()
        {
            var mockContext = new Mock<DbContext>();
            var user = new User();
            mockContext.Setup(x => x.Set<User>().Add(user)).Returns(user);


            Repository<User> repository = new Repository<User>(mockContext.Object);
            var expected = new User();
            var temp = repository.Create(expected);

            Assert.Equal(expected, temp);
        }

        
    }
}
