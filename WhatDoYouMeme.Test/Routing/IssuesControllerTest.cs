using MyTested.AspNetCore.Mvc;
using WhatDoYouMeme.Models.Issues;
using Xunit;
using IssuesController = WhatDoYouMeme.Controllers.IssuesController;

namespace WhatDoYouMeme.Test.Routing
{
    using static MyMvc;

  public  class IssuesControllerTest
    {
        [Fact]
        public void GetLogShouldBeMapped()
            => Routing()
                .ShouldMap("/Issues/Log")
                .To<IssuesController>(c => c.Log());
        [Fact]
        public void PostLogShouldBeMapped()
            => Routing()
                .ShouldMap(request => request
                    .WithPath("/Issues/Log")
                    .WithMethod(HttpMethod.Post))
                .To<IssuesController>(c => c.Log(With.Any<AddIssueFormModel>()));
        
    }
}
