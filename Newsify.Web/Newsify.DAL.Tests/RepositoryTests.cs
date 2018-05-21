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
