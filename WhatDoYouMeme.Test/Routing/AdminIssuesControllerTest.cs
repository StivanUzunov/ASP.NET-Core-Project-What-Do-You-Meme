using MyTested.AspNetCore.Mvc;
using Xunit;
using IssuesController = WhatDoYouMeme.Areas.Admin.Controllers.IssuesController;


namespace WhatDoYouMeme.Test.Routing
{
    using static MyMvc;
  public  class AdminIssuesControllerTest
    {
        [Fact]
        public void GetAllShouldBeMapped()
            => Routing()
                .ShouldMap("/Admin/Issues/All")
                .To<IssuesController>(c => c.All());

        [Fact]
        public void GetDeleteShouldBeMapped()
            => Routing()
                .ShouldMap("/Admin/Issues/Delete")
                .To<IssuesController>(c => c.Delete());
    }
}
