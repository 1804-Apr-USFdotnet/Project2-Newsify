using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Moq;
using Moq.Internals;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Mvc;

namespace Newsify.ASP.Tests
{
    public class NewsifyASPHomeControllerTests
    {

        [Fact]
        public void About_GetAboutView_ReturnsViewResult()
        {
            ASP.Controllers.HomeController hc = new ASP.Controllers.HomeController();

            var viewResult = hc.About();

            Assert.IsType<ViewResult>(viewResult);
        }

        [Fact]
        public void Contact_GetsContactView_ReturnsContactViewResult()
        {
            ASP.Controllers.HomeController hc = new ASP.Controllers.HomeController();

            var viewResult = hc.Contact();

            Assert.IsType<ViewResult>(viewResult);
        }

        [Fact]
        public void Login_GetsLoginView_ReturnsLoginViewResult()
        {
            ASP.Controllers.HomeController hc = new ASP.Controllers.HomeController();

            var viewResult = hc.Login();

            Assert.IsType<ViewResult>(viewResult);
        }

        [Fact]
        public void Register_GetsRegisterView_ReturnsRegisterViewResult()
        { 
            ASP.Controllers.HomeController hc = new ASP.Controllers.HomeController();

            var viewResult = hc.Register();

            Assert.IsType<ViewResult>(viewResult);
        }

        [Fact]
        public async Task Logout_LogsOffUser_ReturnsRedirectToRouteResult()
        {
            ASP.Controllers.HomeController hc = new ASP.Controllers.HomeController();

            var viewResult = await hc.Logout();

            Assert.IsType<RedirectToRouteResult>(viewResult);
            var newView = (RedirectToRouteResult)viewResult;
            Assert.True(newView.RouteValues.ContainsValue("Index"));
        }

        [Fact]
        public void Read_PassNullArticle_ReturnsErrorViewResult()
        {
            ASP.Controllers.HomeController hc = new ASP.Controllers.HomeController();
            var result = hc.Read(new Models.Article());

            Assert.IsType<ViewResult>(result);
            var newResult = (ViewResultBase)result;
            Assert.Equal("Error", newResult.ViewName);
        }

        [Fact]
        public void Read_PassValidArticle_ReturnsArticleViewResult()
        {
            var article = new Models.Article()
            {
                url = "https://www.npr.org/sections/alltechconsidered/2018/05/28/614419275/do-not-sell-my-personal-information-california-eyes-data-privacy-measure"
            };

            ASP.Controllers.HomeController hc = new ASP.Controllers.HomeController();
            var result = hc.Read(article);

            Assert.IsType<ViewResult>(result);
            var newResult = (ViewResultBase)result;
            Assert.IsType<Models.Article>(newResult.Model);
        }

        [Fact]
        public void CommentParamArticleId_GetsNewCommentViewNoSession_ReturnsErrorView()
        {
            var mockcontext = new Mock<ControllerContext>();
            mockcontext.Setup(x => x.HttpContext.Session["UserName"]).Returns("bob");


            ASP.Controllers.HomeController hc = new ASP.Controllers.HomeController() { ControllerContext = mockcontext.Object };


            var result = hc.Comment(1);

            Assert.IsType<ViewResult>(result);
            var newResult = (ViewResultBase)result;
            Assert.IsType<ASP.Models.WebComment>(newResult.Model);
        }
    }
}
