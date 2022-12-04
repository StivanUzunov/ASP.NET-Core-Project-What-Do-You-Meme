using MyTested.AspNetCore.Mvc;
using WhatDoYouMeme.Areas.Admin.Controllers;
using Xunit;

namespace WhatDoYouMeme.Test.Routing
{
    using static MyMvc;

   public class AdminVideosControllerTest
    {
        [Fact]
        public void GetAllShouldBeMapped()
            => Routing()
                .ShouldMap("Admin/Videos/All")
                .To<VideosController>(c => c.All());

        [Fact]
        public void GetMakePublicShouldBeMapped()
            => Routing()
                .ShouldMap("Admin/Videos/MakePublic/1")
                .To<VideosController>(c => c.MakePublic(1));
    }
}
