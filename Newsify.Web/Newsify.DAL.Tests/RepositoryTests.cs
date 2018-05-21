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

                T temp1 = It.IsAny<T>();
                DbMock.Setup(x => x.Set<T>().Remove(temp1)).Returns(temp1);

                IEnumerable<T> tempList = It.IsAny<IEnumerable<T>>();
                DbMock.Setup(x => x.Set<T>().RemoveRange(tempList)).Returns(tempList);

                T temp2 = It.IsAny<T>();
                DbMock.Setup(x => x.Set<T>().Find(It.IsAny<int>())).Returns(temp);

                List<T> tempList2 = It.IsAny<List<T>>();
                DbMock.Setup(x => x.Set<T>().ToList<T>()).Returns(tempList2);
            }
        }

        [Fact]
        public void CreateArticle_CreateAnArticle_ReturnArticle()
        {
            var dbm = new DbMockHelper<Article>();
            

            Repository<Article> repository = new Repository<Article>(dbm.DbMock.Object);
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
