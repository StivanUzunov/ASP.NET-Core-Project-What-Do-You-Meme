using WhatDoYouMeme.Controllers;
using Xunit;

namespace WhatDoYouMeme.Test.Routing
{
    using static MyTested.AspNetCore.Mvc.MyMvc;
    public class HomeControllerTest
    {
        [Fact]
        public void GetIndexRouteShouldBeMapped()
            => Routing()
                .ShouldMap("/")
                .To<HomeController>(c => c.Index());
        [Fact]
        public void GetErrorRouteShouldBeMapped()
           => Routing()
               .ShouldMap("/Home/Error")
               .To<HomeController>(c => c.Error());
    }
}
