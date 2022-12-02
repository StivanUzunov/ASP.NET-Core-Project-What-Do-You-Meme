using MyTested.AspNetCore.Mvc;
using WhatDoYouMeme.Controllers;
using WhatDoYouMeme.Models.Videos;
using Xunit;

namespace WhatDoYouMeme.Test.Routing
{
    using static MyMvc;
    public   class VideosControllerTest
    {
        [Fact]
        public void GetAllShouldBeMapped()
          => Routing()
              .ShouldMap("/Videos/All")
              .To<VideosController>(c => c.All());

        [Fact]
        public void GetMineShouldBeMapped()
            => Routing()
                .ShouldMap("/Videos/Mine")
                .To<VideosController>(c => c.Mine());

        [Fact]
        public void GetDetailsShouldBeMapped()
            => Routing()
                .ShouldMap("/Videos/Details/5")
                .To<VideosController>(c => c.Details(5));
        [Fact]
        public void GetAddShouldBeMapped()
            => Routing()
                .ShouldMap("/Videos/Add")
                .To<VideosController>(c => c.Add());

        [Fact]
        public void PostAddShouldBeMapped()
            => Routing()
                .ShouldMap(request => request
                    .WithPath("/Videos/Add")
                    .WithMethod(HttpMethod.Post))
                .To<VideosController>(c => c.Add(With.Any<AddVideoFormModel>()));
        [Fact]
        public void GetEditShouldBeMapped()
            => Routing()
                .ShouldMap("/Videos/Edit/5")
                .To<VideosController>(c => c.Edit(5));
        [Fact]
        public void PostEditShouldBeMapped()
            => Routing()
                .ShouldMap(request => request
                    .WithPath("/Videos/Edit/3")
                    .WithMethod(HttpMethod.Post))
                .To<VideosController>(c => c.Edit(3, With.Any<EditVideoFormModel>()));

        [Fact]
        public void GetMakePublicShouldBeMapped()
            => Routing()
                .ShouldMap("/Videos/MakePublic/1")
                .To<VideosController>(c => c.MakePublic(1));

        [Fact]
        public void GetDeleteShouldBeMapped()
            => Routing()
                .ShouldMap("/Videos/Delete/2")
                .To<VideosController>(c => c.Delete(2));

        [Fact]
        public void GetLikeShouldBeMapped()
            => Routing()
                .ShouldMap("/Videos/Like/2")
                .To<VideosController>(c => c.Like(2));
    }
}
