using MyTested.AspNetCore.Mvc;
using Xunit;
using WhatDoYouMeme.Controllers;
using WhatDoYouMeme.Models.Memers;


namespace WhatDoYouMeme.Test.Routing
{
    using static MyMvc;

  public class MemersControllerTest
  {
      [Fact]
      public void GetCreateShouldBeMapped()
          => Routing()
              .ShouldMap("/Memers/Create")
              .To<MemersController>(c=>c.Create());
      [Fact]
      public void PostCreateShouldBeMapped()
          => Routing()
              .ShouldMap(request => request
                  .WithPath("/Memers/Create")
                  .WithMethod(HttpMethod.Post))
              .To<MemersController>(c => c.Create(With.Any<BecomeMemerFormModel>()));
  }
}
