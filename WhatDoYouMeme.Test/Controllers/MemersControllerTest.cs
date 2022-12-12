using System.Linq;
using MyTested.AspNetCore.Mvc;
using Xunit;
using WhatDoYouMeme.Controllers;
using WhatDoYouMeme.Data.Models;
using WhatDoYouMeme.Models.Memers;
using HttpMethod = System.Net.Http.HttpMethod;

namespace WhatDoYouMeme.Test.Controllers
{
    using static WebConstants;
    public class MemersControllerTest
    {
        [Fact]
        public void GetCreateShouldBeForAuthorizedUsersAndReturnView()
            => MyController<MemersController>
                .Instance()
                .Calling(c => c.Create())
                .ShouldHave()
                .ActionAttributes(attributes => attributes
                    .RestrictingForAuthorizedRequests())
                .AndAlso()
                .ShouldReturn()
                .View();

        [Theory]
        [InlineData("Memer", "+359123456789")]
        public void PostCreateShouldBeForAuthorizedUserAndReturnRedirectWithValidModel(string memerName, string phoneNumber)
            => MyController<MemersController>
                .Instance(controller => controller.WithUser())
                .Calling(c => c.Create(new BecomeMemerFormModel
                {
                    Name = memerName,
                    PhoneNumber = phoneNumber
                }))
                .ShouldHave()
                .ActionAttributes(attributes => attributes.RestrictingForHttpMethod(HttpMethod.Post).RestrictingForAuthorizedRequests())
                .ValidModelState()
                .Data(data => data
                    .WithSet<Memer>(memers => memers
                        .Any(m => m.Name == memerName
                                        && m.PhoneNumber == phoneNumber
                                        && m.UserId == TestUser.Identifier)))
                .TempData(tempData => tempData
                    .ContainingEntryWithKey(GlobalMessageKey))
                .AndAlso()
                .ShouldReturn()
                .RedirectToAction("All", "Memes");
    }
}
