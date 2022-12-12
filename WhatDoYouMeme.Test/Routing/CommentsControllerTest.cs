using MyTested.AspNetCore.Mvc;
using Xunit;
using WhatDoYouMeme.Controllers;
using WhatDoYouMeme.Models.Comments;

namespace WhatDoYouMeme.Test.Routing
{
    using static MyMvc;
    public class CommentsControllerTest
    {
        [Fact]
        public void GetAddShouldBeMapped()
            => Routing()
                .ShouldMap("/Comments/Add")
                .To<CommentsController>(c => c.Add());

        [Fact]
        public void PostAdShouldBeMapped()
            => Routing()
                .ShouldMap(request => request
                    .WithPath("/Comments/Add/3")
                    .WithMethod(HttpMethod.Post))
                .To<CommentsController>(c => c.Add(3, new AddCommentFormModel()));

        [Fact]
        public void GetDeleteShouldBeMapped()
            => Routing()
                .ShouldMap("/Comments/Delete/5")
                .To<CommentsController>(c => c.Delete(5));

        [Fact]
        public void GetLikeShouldBeMapped()
            => Routing()
                .ShouldMap("/Comments/Like/5")
                .To<CommentsController>(c => c.Like(5));

        [Fact]
        public void GetAddToVideoShouldBeMapped()
            => Routing()
                .ShouldMap("/Comments/AddToVideo")
                .To<CommentsController>(c => c.AddToVideo());

        [Fact]
        public void PostAddToVideoShouldBeMapped()
            => Routing()
                .ShouldMap(request => request
                    .WithPath("/Comments/AddToVideo/3")
                    .WithMethod(HttpMethod.Post))
                .To<CommentsController>(c => c.AddToVideo(3, new AddCommentFormModelVideo()));

        [Fact]
        public void GetDeleteOfVideoShouldBeMapped()
            => Routing()
                .ShouldMap("/Comments/DeleteOfVideo/5")
                .To<CommentsController>(c => c.DeleteOfVideo(5));

        [Fact]
        public void GetLikeofVideoShouldBeMapped()
            => Routing()
                .ShouldMap("/Comments/LikeofVideo/5")
                .To<CommentsController>(c => c.LikeofVideo(5));
    }
}
