using MyTested.AspNetCore.Mvc;
using WhatDoYouMeme.Controllers;
using WhatDoYouMeme.Models.Memes;
using Xunit;

namespace WhatDoYouMeme.Test.Routing
{
    using static MyMvc;
   public class MemesControllerTest
    {
        [Fact]
        public void GetAllShouldBeMapped()
            => Routing()
                .ShouldMap("/Memes/All")
                .To<MemesController>(c => c.All());

        [Fact]
        public void GetMineShouldBeMapped()
            => Routing()
                .ShouldMap("/Memes/Mine")
                .To<MemesController>(c => c.Mine());

        [Fact]
        public void GetDetailsShouldBeMapped()
            => Routing()
                .ShouldMap("/Memes/Details/5")
                .To<MemesController>(c => c.Details(5));
        [Fact]
        public void GetAddShouldBeMapped()
            => Routing()
                .ShouldMap("/Memes/Add")
                .To<MemesController>(c => c.Add());

        [Fact]
        public void PostAddShouldBeMapped()
            => Routing()
                .ShouldMap(request => request
                    .WithPath("/Memes/Add")
                    .WithMethod(HttpMethod.Post))
                .To<MemesController>(c => c.Add(With.Any<AddMemeFormModel>()));
        [Fact]
        public void GetEditShouldBeMapped()
            => Routing()
                .ShouldMap("/Memes/Edit/5")
                .To<MemesController>(c => c.Edit(5));
        [Fact]
        public void PostEditShouldBeMapped()
            => Routing()
                .ShouldMap(request => request
                    .WithPath("/Memes/Edit/3")
                    .WithMethod(HttpMethod.Post))
                .To<MemesController>(c => c.Edit(3,With.Any<EditMemeFormModel>()));

        [Fact]
        public void GetMakePublicShouldBeMapped()
            => Routing()
                .ShouldMap("/Memes/MakePublic/1")
                .To<MemesController>(c => c.MakePublic(1));

        [Fact]
        public void GetDeleteShouldBeMapped()
            => Routing()
                .ShouldMap("/Memes/Delete/2")
                .To<MemesController>(c => c.Delete(2));

        [Fact]
        public void GetLikeShouldBeMapped()
            => Routing()
                .ShouldMap("/Memes/Like/2")
                .To<MemesController>(c => c.Like(2));
    }
}
