using System.Linq;
using Xunit;
using MyTested.AspNetCore.Mvc;
using WhatDoYouMeme.Controllers;
using WhatDoYouMeme.Data.Models;
using WhatDoYouMeme.Models.Comments;


namespace WhatDoYouMeme.Test.Controllers
{
    using static WebConstants;
    public class CommentsControllerTest
    {
        [Fact]
        public void GetAddShouldBeForAuthorizedUsersAndMemersAndReturnView()
            => MyController<CommentsController>
                .Instance(controller => controller.WithUser().WithData(new Memer
                {
                    UserId = TestUser.Identifier,
                    Name = "Stivan Uzunov",
                    PhoneNumber = "+359123456789"
                }))
                .Calling(c => c.Add())
                .ShouldHave()
                .ActionAttributes(attributes => attributes
                    .RestrictingForAuthorizedRequests())
                .AndAlso()
                .ShouldReturn()
                .View();

        [Fact]
        public void GetAddShouldBeForAuthorizedUsersAndMemersAndRedirectToAction()
            => MyController<CommentsController>
                .Instance(controller => controller.WithUser())
                .Calling(c => c.Add())
                .ShouldHave()
                .ActionAttributes(attributes => attributes
                    .RestrictingForAuthorizedRequests())
                .AndAlso()
                .ShouldReturn()
                .RedirectToAction("Create", "Memers");

        [Theory]
        [InlineData(12, "test comment", 10)]
        public void PostAddShouldBeForAuthorizedUsersAndMemersAndRedirectToAction(int id, string commentText, int memerId)
            => MyController<CommentsController>
                .Instance(controller => controller.WithUser().WithData(new Memer
                {
                    UserId = TestUser.Identifier,
                    Name = "Stivan Uzunov",
                    PhoneNumber = "+359123456789"
                }))
                .Calling(c => c.Add(id, new AddCommentFormModel()
                {
                    CommentText = commentText,
                    MemerId = memerId,

                }))
                .ShouldHave()
                .ActionAttributes(attributes => attributes.RestrictingForHttpMethod(HttpMethod.Post).RestrictingForAuthorizedRequests())
                .ValidModelState()
                .Data(data => data
                    .WithSet<Comment>(comment => comment
                        .Any(c => c.CommentText == commentText)))
                .TempData(tempData => tempData
                    .ContainingEntryWithKey(GlobalMessageKey))
                .AndAlso()
                .ShouldReturn()
                .RedirectToAction(nameof(MemesController.Details));

        [Theory]
        [InlineData(12, "test comment", 10)]
        public void PostAddShouldBeForAuthorizedUsersAndMemersAndRedirectToActionForMemers(int id, string commentText, int memerId)
            => MyController<CommentsController>
                .Instance(controller => controller.WithUser())
                .Calling(c => c.Add(id, new AddCommentFormModel()
                {
                    CommentText = commentText,
                    MemerId = memerId,
                }))
                .ShouldReturn()
                .RedirectToAction(nameof(MemersController.Create));

        [Fact]
        public void GetDeleteShouldBeForAuthorizedUsersAndAdminsAndShouldReturnBadRequest()
            => MyController<CommentsController>
                .Instance(controller => controller.WithUser())
                .Calling(c => c.Delete(2))
                .ShouldHave()
                .ActionAttributes(attributes => attributes
                    .RestrictingForAuthorizedRequests())
                .AndAlso()
                .ShouldReturn()
                .BadRequest();

        [Fact]
        public void GetDeleteShouldBeForAuthorizedUsersAndAdminsAndShouldRedirectToAction()
            => MyController<CommentsController>
                .Instance(controller => controller.WithUser(u => u.InRole("Administrator")).WithData(new Comment
                {
                    Id = 2,
                    PostId = 2,
                    Date = "08-12-2022",
                    Likes = 2,
                    MemerId = 3,
                    MemerName = "Stivan Uzunov",
                }).WithData(new Post
                {
                    Id = 2,
                    Date = "08-12-2022",
                    Description = "Test description of a meme",
                    ImageUrl = "https://media.sproutsocial.com/uploads/meme-example.jpg",
                    isPublic = true,
                    Likes = 0,
                    MemerId = 3
                }))
                .Calling(c => c.Delete(2))
                .ShouldHave()
                .ActionAttributes(attributes => attributes
                    .RestrictingForAuthorizedRequests())
                .AndAlso()
                .ShouldReturn()
                .RedirectToAction(nameof(MemesController.Details));
        [Fact]
        public void GetLikeShouldBeForAuthorizedUsersAndAdminsAndShouldRedirectToActionForMemers()
            => MyController<CommentsController>
                .Instance(controller => controller.WithUser())
                .Calling(c => c.Like(2))
                .ShouldHave()
                .ActionAttributes(attributes => attributes
                    .RestrictingForAuthorizedRequests())
                .AndAlso()
                .ShouldReturn()
                .RedirectToAction(nameof(MemersController.Create));

        [Fact]
        public void GetLikeShouldBeForAuthorizedUsersAndAdminsAndShouldRedirectToAction()
            => MyController<CommentsController>
                .Instance(controller => controller.WithUser().WithData(new Memer
                {
                    UserId = TestUser.Identifier,
                    Name = "Stivan Uzunov",
                    PhoneNumber = "+359123456789"
                })
                    .WithData(new Comment
                    {
                        Id = 2,
                        PostId = 2,
                        Date = "08-12-2022",
                        Likes = 2,
                        MemerId = 3,
                        MemerName = "Stivan Uzunov",
                    }).WithData(new Post
                    {
                        Id = 2,
                        Date = "08-12-2022",
                        Description = "Test description of a meme",
                        ImageUrl = "https://media.sproutsocial.com/uploads/meme-example.jpg",
                        isPublic = true,
                        Likes = 0,
                        MemerId = 3
                    }))
                .Calling(c => c.Like(2))
                .ShouldHave()
                .ActionAttributes(attributes => attributes
                    .RestrictingForAuthorizedRequests())
                .AndAlso()
                .ShouldReturn()
                .RedirectToAction(nameof(MemesController.Details));

        [Fact]
        public void GetAddToVideoShouldBeForAuthorizedUsersAndMemersAndReturnView()
            => MyController<CommentsController>
                .Instance(controller => controller.WithUser().WithData(new Memer
                {
                    UserId = TestUser.Identifier,
                    Name = "Stivan Uzunov",
                    PhoneNumber = "+359123456789"
                }))
                .Calling(c => c.AddToVideo())
                .ShouldHave()
                .ActionAttributes(attributes => attributes
                    .RestrictingForAuthorizedRequests())
                .AndAlso()
                .ShouldReturn()
                .View();

        [Fact]
        public void GetAddToVideoShouldBeForAuthorizedUsersAndMemersAndRedirectToAction()
            => MyController<CommentsController>
                .Instance(controller => controller.WithUser())
                .Calling(c => c.AddToVideo())
                .ShouldHave()
                .ActionAttributes(attributes => attributes
                    .RestrictingForAuthorizedRequests())
                .AndAlso()
                .ShouldReturn()
                .RedirectToAction("Create", "Memers");

        [Theory]
        [InlineData(12, "test comment", 10)]
        public void PostAddToVideoShouldBeForAuthorizedUsersAndMemersAndRedirectToAction(int id, string commentText, int memerId)
            => MyController<CommentsController>
                .Instance(controller => controller.WithUser().WithData(new Memer
                {
                    UserId = TestUser.Identifier,
                    Name = "Stivan Uzunov",
                    PhoneNumber = "+359123456789"
                }))
                .Calling(c => c.AddToVideo(id, new AddCommentFormModelVideo()
                {
                    CommentText = commentText,
                    MemerId = memerId,

                }))
                .ShouldHave()
                .ActionAttributes(attributes => attributes.RestrictingForHttpMethod(HttpMethod.Post).RestrictingForAuthorizedRequests())
                .ValidModelState()
                .Data(data => data
                    .WithSet<Comment>(comment => comment
                        .Any(c => c.CommentText == commentText)))
                .TempData(tempData => tempData
                    .ContainingEntryWithKey(GlobalMessageKey))
                .AndAlso()
                .ShouldReturn()
                .RedirectToAction(nameof(VideosController.Details));

        [Theory]
        [InlineData(12, "test comment", 10)]
        public void PostAddToVideoShouldBeForAuthorizedUsersAndMemersAndRedirectToActionForMemers(int id, string commentText, int memerId)
            => MyController<CommentsController>
                .Instance(controller => controller.WithUser())
                .Calling(c => c.AddToVideo(id, new AddCommentFormModelVideo()
                {
                    CommentText = commentText,
                    MemerId = memerId,
                }))
                .ShouldReturn()
                .RedirectToAction(nameof(MemersController.Create));
        [Fact]
        public void GetDeleteFromVideoShouldBeForAuthorizedUsersAndAdminsAndShouldReturnBadRequest()
            => MyController<CommentsController>
                .Instance(controller => controller.WithUser())
                .Calling(c => c.DeleteOfVideo(2))
                .ShouldHave()
                .ActionAttributes(attributes => attributes
                    .RestrictingForAuthorizedRequests())
                .AndAlso()
                .ShouldReturn()
                .BadRequest();

        [Fact]
        public void GetDeleteFromShouldBeForAuthorizedUsersAndAdminsAndShouldRedirectToAction()
            => MyController<CommentsController>
                .Instance(controller => controller.WithUser(u => u.InRole("Administrator")).WithData(new Comment
                {
                    Id = 2,
                    VideoId = 2,
                    Date = "08-12-2022",
                    Likes = 2,
                    MemerId = 3,
                    MemerName = "Stivan Uzunov",
                }).WithData(new Video()
                {
                    Id = 2,
                    Date = "08-12-2022",
                    Description = "Test description of a video",
                    VideoUrl = "https://www.youtube.com/embed/g6PSwYx3jA0",
                    IsPublic = true,
                    Likes = 0,
                    MemerId = 3
                }))
                .Calling(c => c.DeleteOfVideo(2))
                .ShouldHave()
                .ActionAttributes(attributes => attributes
                    .RestrictingForAuthorizedRequests())
                .AndAlso()
                .ShouldReturn()
                .RedirectToAction(nameof(VideosController.Details));

        [Fact]
        public void GetLikeVideoShouldBeForAuthorizedUsersAndAdminsAndShouldRedirectToActionForMemers()
            => MyController<CommentsController>
                .Instance(controller => controller.WithUser())
                .Calling(c => c.LikeofVideo(2))
                .ShouldHave()
                .ActionAttributes(attributes => attributes
                    .RestrictingForAuthorizedRequests())
                .AndAlso()
                .ShouldReturn()
                .RedirectToAction(nameof(MemersController.Create));

        [Fact]
        public void GetLikeVideoShouldBeForAuthorizedUsersAndAdminsAndShouldRedirectToAction()
            => MyController<CommentsController>
                .Instance(controller => controller.WithUser().WithData(new Memer
                    {
                        UserId = TestUser.Identifier,
                        Name = "Stivan Uzunov",
                        PhoneNumber = "+359123456789"
                    })
                    .WithData(new Comment
                    {
                        Id = 2,
                        VideoId = 2,
                        Date = "08-12-2022",
                        Likes = 2,
                        MemerId = 3,
                        MemerName = "Stivan Uzunov",
                    }).WithData(new Video
                    {
                        Id = 2,
                        Date = "08-12-2022",
                        Description = "Test description of a video",
                        VideoUrl = "https://www.youtube.com/embed/g6PSwYx3jA0",
                        IsPublic = true,
                        Likes = 0,
                        MemerId = 3
                    }))
                .Calling(c => c.LikeofVideo(2))
                .ShouldHave()
                .ActionAttributes(attributes => attributes
                    .RestrictingForAuthorizedRequests())
                .AndAlso()
                .ShouldReturn()
                .RedirectToAction(nameof(MemesController.Details));

    }
}
