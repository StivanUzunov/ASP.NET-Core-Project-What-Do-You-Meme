using System;
using FluentAssertions;
using Xunit;
using MyTested.AspNetCore.Mvc;
using WhatDoYouMeme.Controllers;
using WhatDoYouMeme.Models.Home;
using static WhatDoYouMeme.Test.Data.Memes;

namespace WhatDoYouMeme.Test.Controllers
{
  public  class HomeControllerTest
  {
      [Fact]
      public void IndexShouldReturnViewWithCorrectViewWithModel()
          => MyController<HomeController>
              .Instance(instance => instance
                  .WithData(TenPublicMemes()))
              .Calling(c=>c.Index())
              .ShouldHave()
              .MemoryCache(cache=>cache.ContainingEntry(entry=>entry
                  .WithKey("latestMemesCacheKey")
                  .WithAbsoluteExpirationRelativeToNow(TimeSpan.FromMinutes(15))))
              .AndAlso()
              .ShouldReturn()
              .View(view => view
                  .WithModelOfType<IndexViewModel>()
                  .Passing(m => m.Posts.Should()
                      .HaveCount(3)));

      [Fact]
      public void ErrorShouldReturnView()
          => MyController<HomeController>
              .Instance()
              .Calling(c => c.Error())
              .ShouldReturn()
              .View();
  }
}
