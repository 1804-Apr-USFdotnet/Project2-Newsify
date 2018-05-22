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
        
        [Fact]
        public void CreateArticle_CreateAnArticle_ReturnArticle()
        {

            var DbMock = new Mock<DbContext>();
            var entity = new Article();
            DbMock.Setup(x => x.Set<Article>().Add(entity)).Returns(entity);
            
            

            Repository<Article> repository = new Repository<Article>(DbMock.Object);
            var expected = new Article();
            var temp = repository.Create(expected);

            Assert.Equal(expected, temp);
        }

        [Theory]
        [InlineData("ValidName")]
        [InlineData("InValidNameIsMoreThan30CharactersLong")]
        public void CreateUser_CreateUser_ReturnUserOrNull(string uname)
        {
            var mockContext = new Mock<NewsDBEntities>();
            var user = new User();
            mockContext.Setup(x => x.Set<User>().Add(user)).Returns(user);

            List<User> list = new List<User>() { new User(), new User() };
            IQueryable<User>  tempIQuery = list.AsQueryable();

            var mockSet = new Mock<DbSet<User>>();
            mockSet.As<IQueryable<User>>().Setup(x => x.Provider).Returns(tempIQuery.Provider);
            mockSet.As<IQueryable<User>>().Setup(x => x.Expression).Returns(tempIQuery.Expression);
            mockSet.As<IQueryable<User>>().Setup(x => x.ElementType).Returns(tempIQuery.ElementType);
            mockSet.As<IQueryable<User>>().Setup(x => x.GetEnumerator()).Returns(() => tempIQuery.GetEnumerator());
            mockContext.Setup(x => x.Set<User>()).Returns(mockSet.Object);

            UserRepo repository = new UserRepo(mockContext.Object);
            var expected = new User() { UserName = uname };



            var temp = repository.Create(expected);
            if (uname.Length <= 30)
            {
                Assert.Equal(expected, temp);
            }
            else
            {
                Assert.Null(temp);
            }
        }

        [Fact]
        public void DeleteUser_DeletesUser_UserActiveIsFalse()
        {
            List<User> UserList = new List<User>() { new User() { ID = 100000, UserName = "b", Active = true },
                new User() { ID = 200000, UserName = "a", Active = true },
                new User() { ID =300000, UserName = "c", Active = true } };

            UserRepo repository = new UserRepo(new NewsDBEntities());
            foreach (var item in UserList)
            {
                repository.Create(item);
            }

            var expected = new User() { ID = 100000, Active = true };

            var temp = repository.Delete(expected);

            Assert.Equal(100000, temp.ID);
            Assert.False(temp.Active);
        }
        
    }
}
