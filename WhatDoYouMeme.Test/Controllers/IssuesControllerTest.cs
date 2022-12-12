using System.Linq;
using System.Security.Claims;
using MyTested.AspNetCore.Mvc;
using WhatDoYouMeme.Controllers;
using WhatDoYouMeme.Data.Models;
using WhatDoYouMeme.Models.Issues;
using Xunit;
using HttpMethod = System.Net.Http.HttpMethod;

namespace WhatDoYouMeme.Test.Controllers
{
    using static WebConstants;

    public class IssuesControllerTest
    {
        [Fact]
        public void GetLogShouldBeForAuthorizedUsersAndMemersAndRedirectToAction()
            => MyController<IssuesController>
                .Instance(controller => controller.WithUser())
                .Calling(c => c.Log())
                .ShouldHave()
                .ActionAttributes(attributes => attributes
                    .RestrictingForAuthorizedRequests())
                .AndAlso()
                .ShouldReturn()
                .RedirectToAction("Create", "Memers");

        [Fact]
        public void GetLogShouldBeForAuthorizedUsersAndMemersAndReturnView()
            => MyController<IssuesController>
                .Instance(controller => controller.WithUser().WithData(new Memer
                {
                    UserId = TestUser.Identifier,
                    Name = "Stivan Uzunov",
                    PhoneNumber = "+359123456789"
                }))
                .Calling(c => c.Log())
                .ShouldHave()
                .ActionAttributes(attributes => attributes
                    .RestrictingForAuthorizedRequests())
                .AndAlso()
                .ShouldReturn()
                .View();

        [Theory]
        [InlineData("TestTitle", "TestDescription")]
        public void PostLogShouldBeForAuthorizedUserAndMemerAndReturnRedirectToAction(string title, string description)
            => MyController<IssuesController>
                .Instance(controller => controller.WithUser(u => u.WithClaim(ClaimTypes.Email, "testemail@gmail.com")))
                .Calling(c => c.Log(new AddIssueFormModel
                {
                    Title = title,
                    Description = description
                }))
                .ShouldHave()
                .ActionAttributes(attributes => attributes.RestrictingForHttpMethod(HttpMethod.Post).RestrictingForAuthorizedRequests())
                .AndAlso()
                .ShouldReturn()
                .RedirectToAction("Create", "Memers");

        [Theory]
        [InlineData("TestTitle", "TestDescription")]
        public void PostLogShouldBeForAuthorizedUserAndReturnRedirectWithValidModel(string title, string description)
            => MyController<IssuesController>
                .Instance(controller => controller.WithUser(u => u.WithClaim(ClaimTypes.Email, "testemail@gmail.com").WithIdentifier("123456789"))
                    .WithData(new Memer
                    {
                        UserId = "123456789",
                        Name = "Stivan Uzunov",
                        PhoneNumber = "+359123456789"
                    }))
                .Calling(c => c.Log(new AddIssueFormModel
                {
                    Title = title,
                    Description = description,
                }))
                .ShouldHave()
                .ActionAttributes(attributes => attributes.RestrictingForHttpMethod(HttpMethod.Post).RestrictingForAuthorizedRequests())
                .ValidModelState()
                .Data(data => data
                    .WithSet<Issues>(issue => issue
                        .Any(m => m.Title == title && m.Description == description)))
                .TempData(tempData => tempData
                    .ContainingEntryWithKey(GlobalMessageKey))
                .AndAlso()
                .ShouldReturn()
                .RedirectToAction("All", "Memes");

        [Fact]
        public void GetIsReviewedShouldBeForAuthorizedUsersAndAdminsAndShouldReturnBadRequest()
            => MyController<Areas.Admin.Controllers.IssuesController>
                .Instance(controller => controller.WithUser().WithData(new Issues
                {
                    Id = 2,
                    MemerId = 2
                }).WithRouteData())
                .Calling(c => c.IsReviewed(2))
                .ShouldHave()
                .ActionAttributes(attributes => attributes
                    .RestrictingForAuthorizedRequests())
                .AndAlso()
                .ShouldReturn()
                .BadRequest();

        [Fact]
        public void GetIsReviewedShouldBeForAuthorizedUsersAndAdminsAndShouldRedirect()
            => MyController<Areas.Admin.Controllers.IssuesController>
                .Instance(controller => controller.WithUser(u => u.InRole("Administrator")).WithData(new Issues
                {
                    Id = 2,
                    MemerId = 2
                }).WithRouteData())
                .Calling(c => c.IsReviewed(2))
                .ShouldHave()
                .ActionAttributes(attributes => attributes
                    .RestrictingForAuthorizedRequests())
                .AndAlso()
                .ShouldReturn()
                .RedirectToAction("All");
        [Fact]
        public void GetDeleteShouldBeForAuthorizedUsersAndAdminsAndShouldReturnBadRequest()
            => MyController<Areas.Admin.Controllers.IssuesController>
                .Instance(controller => controller.WithUser().WithData(new Issues
                {
                    Id = 2,
                    MemerId = 2
                }).WithRouteData())
                .Calling(c => c.Delete())
                .ShouldHave()
                .ActionAttributes(attributes => attributes
                    .RestrictingForAuthorizedRequests())
                .AndAlso()
                .ShouldReturn()
                .BadRequest();
        [Fact]
        public void GetDeleteShouldBeForAuthorizedUsersAndAdminsAndShouldRedirect()
            => MyController<Areas.Admin.Controllers.IssuesController>
                .Instance(controller => controller.WithUser(u => u.InRole("Administrator")).WithData(new Issues
                {
                    Id = 2,
                    MemerId = 2
                }).WithRouteData())
                .Calling(c => c.Delete())
                .ShouldHave()
                .ActionAttributes(attributes => attributes
                    .RestrictingForAuthorizedRequests())
                .AndAlso()
                .ShouldReturn()
                .RedirectToAction("All");
        [Fact]
        public void GetAllShouldBeForAuthorizedUsersAndAdminsAndShouldReturnBadRequest()
            => MyController<Areas.Admin.Controllers.IssuesController>
                .Instance(controller => controller.WithUser().WithData(new Issues
                {
                    Id = 2,
                    MemerId = 2
                }).WithRouteData())
                .Calling(c => c.All())
                .ShouldHave()
                .ActionAttributes(attributes => attributes
                    .RestrictingForAuthorizedRequests())
                .AndAlso()
                .ShouldReturn()
                .BadRequest();
        [Fact]
        public void GetAllShouldBeForAuthorizedUsersAndAdminsAndShouldReturnData()
            => MyController<Areas.Admin.Controllers.IssuesController>
                .Instance(controller => controller.WithUser(u => u.InRole("Administrator")).WithData(new Issues
                {
                    Id = 2,
                    MemerId = 2
                }).WithRouteData())
                .Calling(c => c.All())
                .ShouldHave()
                .ActionAttributes(attributes => attributes
                    .RestrictingForAuthorizedRequests())
                .AndAlso()
                .ShouldReturn()
                .View();
    }
}
