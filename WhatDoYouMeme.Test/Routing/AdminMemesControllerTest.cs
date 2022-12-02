using MyTested.AspNetCore.Mvc;
using WhatDoYouMeme.Areas.Admin.Controllers;
using Xunit;

namespace WhatDoYouMeme.Test.Routing
{
    using static MyMvc;
   public class AdminMemesControllerTest
    {
        [Fact]
        public void GetAllShouldBeMapped()
            => Routing()
                .ShouldMap("Admin/Memes/All")
                .To<MemesController>(c => c.All());
    }
}
