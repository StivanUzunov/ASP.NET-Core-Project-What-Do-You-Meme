using MyTested.AspNetCore.Mvc;
using WhatDoYouMeme.Controllers;
using WhatDoYouMeme.Data.Models;
using WhatDoYouMeme.Models.Videos;
using Xunit;
using System.Linq;

namespace WhatDoYouMeme.Test.Controllers
{
    using static WebConstants;

    public class VideosControllerTest
    {
        [Fact]
        public void GetAllShouldReturnView()
            => MyController<VideosController>
                .Instance()
                .Calling(c => c.All())
                .ShouldReturn()
                .View();

        [Fact]
        public void GetMineShouldReturnView()
            => MyController<VideosController>
                .Instance(controller => controller.WithUser())
                .Calling(c => c.Mine())
                .ShouldHave()
                .ActionAttributes(attributes => attributes
                    .RestrictingForAuthorizedRequests())
                .AndAlso()
                .ShouldReturn()
                .View();

        [Theory]
        [InlineData(12)]
        public void GetDetailsShouldReturnView(int id)
            => MyController<VideosController>
                .Instance()
                .Calling(c => c.Details(id))
                .ShouldReturn()
                .View();

        [Fact]
        public void GetAddShouldRedirectToActionForMemers()
            => MyController<VideosController>
                .Instance(controller => controller.WithUser())
                .Calling(c => c.Add())
                .ShouldHave()
                .ActionAttributes(attributes => attributes
                    .RestrictingForAuthorizedRequests())
                .AndAlso()
                .ShouldReturn()
                .RedirectToAction("Create", "Memers");

        [Fact]
        public void GetAddShouldReturnView()
            => MyController<VideosController>
                .Instance(controller => controller.WithUser(u => u.WithIdentifier("123456789")).WithData(new Memer
                {
                    UserId = "123456789",
                    Name = "Stivan Uzunov",
                    PhoneNumber = "+359123456789"
                }))
                .Calling(c => c.Add())
                .ShouldHave()
                .ActionAttributes(attributes => attributes
                    .RestrictingForAuthorizedRequests())
                .AndAlso()
                .ShouldReturn()
                .View(new AddVideoFormModel());

        [Theory]
        [InlineData("https://www.youtube.com/embed/g6PSwYx3jA0", "This is a test description for the video", "Test title of a video")]
        public void PostAddShouldBeForAuthorizedUserAndReturnRedirectWithValidModel(string videoURL, string description, string title)
            => MyController<VideosController>
                .Instance(controller => controller.WithUser(u => u.WithIdentifier("123456789"))
                    .WithData(new Memer
                    {
                        UserId = "123456789",
                        Name = "Stivan Uzunov",
                        PhoneNumber = "+359123456789"
                    }))
                .Calling(c => c.Add(new AddVideoFormModel()
                {
                    VideoUrl = videoURL,
                    Title = title,
                    Description = description,
                }))
                .ShouldHave()
                .ActionAttributes(attributes =>
                    attributes.RestrictingForHttpMethod(HttpMethod.Post).RestrictingForAuthorizedRequests())
                .ValidModelState()
                .Data(data => data
                    .WithSet<Video>(video => video
                        .Any(m => m.VideoUrl == videoURL && m.Description == description && m.Title == title)))
                .TempData(tempData => tempData
                    .ContainingEntryWithKey(GlobalMessageKey))
                .AndAlso()
                .ShouldReturn()
                .RedirectToAction("All");

        [Theory]
        [InlineData("https://www.youtube.com/embed/g6PSwYx3jA0", "This is a test description for the video", "Test title of a video")]
        public void PostAddShouldBeForAuthorizedUserAndReturnRedirectToActionForMemers(string videoURL, string description, string title)
            => MyController<VideosController>
                .Instance(controller => controller.WithUser())
                .Calling(c => c.Add(new AddVideoFormModel()
                {
                    VideoUrl = videoURL,
                    Title = title,
                    Description = description,
                }))
                .ShouldReturn()
                .RedirectToAction("Create", "Memers");

        [Fact]
        public void GetEditShouldReturnView()
            => MyController<VideosController>
                .Instance(controller => controller.WithUser(u => u.WithIdentifier("123456789"))
                    .WithData(new Memer
                    {
                        Id = 2,
                        UserId = "123456789",
                        Name = "Stivan Uzunov",
                        PhoneNumber = "+359123456789"
                    }).WithData(new Video()
                    {
                        Id = 12,
                        VideoUrl = "https://www.youtube.com/embed/g6PSwYx3jA0",
                        Title = "Test title of a video",
                        Description = "This is a test description for the video",
                        IsPublic = true,
                        Date = "12-12-2022",
                        Likes = 0,
                        MemerId = 2
                    }))
                .Calling(c => c.Edit(12))
                .ShouldHave()
                .ActionAttributes(attributes => attributes
                    .RestrictingForAuthorizedRequests())
                .AndAlso()
                .ShouldReturn()
                .View();

        [Fact]
        public void GetEditShouldRedirectToActionForMemers()
            => MyController<VideosController>
                .Instance(controller => controller.WithUser().WithData(new Video()
                {
                    Id = 12,
                    VideoUrl = "https://www.youtube.com/embed/g6PSwYx3jA0",
                    Title = "Test title of a video",
                    Description = "This is a test description for the video",
                    IsPublic = true,
                    Date = "12-12-2022",
                    Likes = 0,
                    MemerId = 2
                }))
                .Calling(c => c.Edit(12))
                .ShouldHave()
                .ActionAttributes(attributes => attributes
                    .RestrictingForAuthorizedRequests())
                .AndAlso()
                .ShouldReturn()
                .RedirectToAction("Create", "Memers");

        [Fact]
        public void GetEditShouldReturnViewForAdmin()
            => MyController<VideosController>
                .Instance(controller => controller.WithUser(u => u.InRole("Administrator"))
                    .WithData(new Video()
                    {
                        Id = 12,
                        VideoUrl = "https://www.youtube.com/embed/g6PSwYx3jA0",
                        Description = "This is a test description for the video",
                        Title = "Test title of a video",
                        Date = "12-12-2022",
                        Likes = 0,
                        MemerId = 2
                    }))
                .Calling(c => c.Edit(12))
                .ShouldHave()
                .ActionAttributes(attributes => attributes
                    .RestrictingForAuthorizedRequests())
                .AndAlso()
                .ShouldReturn()
                .View();

        [Theory]
        [InlineData(
          "https://www.youtube.com/embed/g6PSwYx3jA0",
           "This is a test description for the video", "This is a test title for the video",
          "https://www.youtube.com/embed/3B66bQbXQUI",
          "This is a changed test description for the video", "This is a changed test title for the video")]

        public void PostEditShouldRedirectToAction(string videoURL, string description, string title, string newVideoURL, string newDescription, string newTitle)
          => MyController<VideosController>
              .Instance(controller => controller.WithUser(u => u.WithIdentifier("123456789"))
                  .WithData(new Memer
                  {
                      Id = 2,
                      UserId = "123456789",
                      Name = "Stivan Uzunov",
                      PhoneNumber = "+359123456789"
                  }).WithData(new Video()
                  {
                      Id = 12,
                      VideoUrl = videoURL,
                      Description = description,
                      Title = title,
                      Date = "12-12-2022",
                      Likes = 0,
                      MemerId = 2
                  }))
              .Calling(c => c.Edit(12, new EditVideoFormModel()
              {
                  VideoUrl = newVideoURL,
                  Title = newTitle,
                  Description = newDescription,
                  MemerId = 2
              }))
              .ShouldHave()
              .ActionAttributes(attributes => attributes.RestrictingForHttpMethod(HttpMethod.Post).RestrictingForAuthorizedRequests())
              .ValidModelState()
              .Data(data => data
                  .WithSet<Video>(video => video
                      .Any(v => v.VideoUrl == newVideoURL && v.Description == newDescription && v.Title == newTitle)))
              .TempData(tempData => tempData
                  .ContainingEntryWithKey(GlobalMessageKey))
              .AndAlso()
              .ShouldReturn()
              .RedirectToAction("All");

        [Theory]
        [InlineData(
            "https://www.youtube.com/embed/g6PSwYx3jA0",
            "This is a test description for the video", "This is a test title for the video",
            "https://www.youtube.com/embed/3B66bQbXQUI",
            "This is a changed test description for the video", "This is a changed test title for the video")]
        public void PostEditShouldRedirectToActionForMemers(string videoURL, string description, string title, string newVideoURL, string newDescription, string newTitle)
            => MyController<VideosController>
                .Instance(controller => controller.WithUser()
                    .WithData(new Video()
                    {
                        Id = 12,
                        VideoUrl = videoURL,
                        Description = description,
                        Title = title,
                        Date = "12-12-2022",
                        Likes = 0,
                        MemerId = 2
                    }))
                .Calling(c => c.Edit(12, new EditVideoFormModel()
                {
                    VideoUrl = newVideoURL,
                    Title = newTitle,
                    Description = newDescription,
                    MemerId = 2
                }))
                .ShouldHave()
                .ActionAttributes(attributes =>
                    attributes.RestrictingForHttpMethod(HttpMethod.Post).RestrictingForAuthorizedRequests())
                .AndAlso()
                .ShouldReturn()
                .RedirectToAction("Create", "Memers");

        [Theory]
        [InlineData(
            "https://www.youtube.com/embed/g6PSwYx3jA0",
            "This is a test description for the video", "This is a test title for the video",
            "https://www.youtube.com/embed/3B66bQbXQUI",
            "This is a changed test description for the video", "This is a changed test title for the video")]

        public void PostEditShouldRedirectToActionForAdmins(string videoURL, string description, string title, string newVideoURL, string newDescription, string newTitle)
            => MyController<VideosController>
                .Instance(controller => controller.WithUser(u => u.InRole("Administrator"))
                    .WithData(new Video()
                    {
                        Id = 12,
                        VideoUrl = videoURL,
                        Description = description,
                        Title = title,
                        Date = "12-12-2022",
                        Likes = 0,
                        MemerId = 2
                    }))
                .Calling(c => c.Edit(12, new EditVideoFormModel
                {
                    VideoUrl = newVideoURL,
                    Title = newTitle,
                    Description = newDescription,
                    MemerId = 2
                }))
                .ShouldHave()
                .ActionAttributes(attributes => attributes.RestrictingForHttpMethod(HttpMethod.Post).RestrictingForAuthorizedRequests())
                .ValidModelState()
                .Data(data => data
                    .WithSet<Video>(meme => meme
                        .Any(v => v.VideoUrl == newVideoURL && v.Description == newDescription && v.Title == newTitle)))
                .TempData(tempData => tempData
                    .ContainingEntryWithKey(GlobalMessageKey))
                .AndAlso()
                .ShouldReturn()
                .RedirectToAction("All");

        [Fact]
        public void GetDeleteShouldRedirectToAction()
            => MyController<VideosController>
                .Instance(controller => controller.WithUser(u => u.WithIdentifier("123456789"))
                    .WithData(new Memer
                    {
                        Id = 2,
                        UserId = "123456789",
                        Name = "Stivan Uzunov",
                        PhoneNumber = "+359123456789"
                    }).WithData(new Video()
                    {
                        Id = 12,
                        VideoUrl = "https://www.youtube.com/embed/g6PSwYx3jA0",
                        Description = "This is a test description for the video",
                        Title = "This is a test title for the video",
                        Date = "12-12-2022",
                        Likes = 0,
                        MemerId = 2
                    }))
                .Calling(c => c.Delete(12))
                .ShouldHave()
                .ActionAttributes(attributes => attributes
                    .RestrictingForAuthorizedRequests())
                .AndAlso()
                .ShouldReturn()
                .RedirectToAction("All");

        [Fact]
        public void GetDeleteShouldReturnBadRequest()
            => MyController<VideosController>
                .Instance(controller => controller.WithUser()
                    .WithData(new Video()
                    {
                        Id = 12,
                        VideoUrl = "https://www.youtube.com/embed/g6PSwYx3jA0",
                        Description = "This is a test description for the video",
                        Title = "This is a test title for the video",
                        Date = "12-12-2022",
                        Likes = 0,
                        MemerId = 2
                    }))
                .Calling(c => c.Delete(12))
                .ShouldHave()
                .ActionAttributes(attributes => attributes
                    .RestrictingForAuthorizedRequests())
                .AndAlso()
                .ShouldReturn()
                .BadRequest();

        [Fact]
        public void GetDeleteShouldRedirectToActionForAdmins()
            => MyController<VideosController>
                .Instance(controller => controller.WithUser(u => u.InRole("Administrator"))
                    .WithData(new Video()
                    {
                        Id = 12,
                        VideoUrl = "https://www.youtube.com/embed/g6PSwYx3jA0",
                        Description = "This is a test description for the video",
                        Title = "This is a test title for the video",
                        Date = "12-12-2022",
                        Likes = 0,
                        MemerId = 2
                    }))
                .Calling(c => c.Delete(12))
                .ShouldHave()
                .ActionAttributes(attributes => attributes
                    .RestrictingForAuthorizedRequests())
                .AndAlso()
                .ShouldReturn()
                .RedirectToAction("All");

        [Fact]
        public void GetLikeShouldRedirectToAction()
            => MyController<VideosController>
                .Instance(controller => controller.WithUser(u => u.WithIdentifier("123456789"))
                    .WithData(new Memer
                    {
                        Id = 2,
                        UserId = "123456789",
                        Name = "Stivan Uzunov",
                        PhoneNumber = "+359123456789"
                    }).WithData(new Video()
                    {
                        Id = 12,
                        VideoUrl = "https://www.youtube.com/embed/g6PSwYx3jA0",
                        Description = "This is a test description for the video",
                        Title = "This is a test title for the video",
                        Date = "12-12-2022",
                        Likes = 0,
                        MemerId = 2
                    }))
                .Calling(c => c.Like(12))
                .ShouldHave()
                .ActionAttributes(attributes => attributes
                    .RestrictingForAuthorizedRequests())
                .AndAlso()
                .ShouldReturn()
                .RedirectToAction("All", "Videos");

        [Fact]
        public void GetLikeShouldRedirectToActionForMemers()
            => MyController<VideosController>
                .Instance(controller => controller.WithUser()
                    .WithData(new Video()
                    {
                        Id = 12,
                        VideoUrl = "https://www.youtube.com/embed/g6PSwYx3jA0",
                        Description = "This is a test description for the video",
                        Title = "This is a test title for the video",
                        Date = "12-12-2022",
                        Likes = 0,
                        MemerId = 2
                    }))
                .Calling(c => c.Like(12))
                .ShouldHave()
                .ActionAttributes(attributes => attributes
                    .RestrictingForAuthorizedRequests())
                .AndAlso()
                .ShouldReturn()
                .RedirectToAction("Create", "Memers");

        [Fact]
        public void GetAdminAllShouldReturnView()
            => MyController<Areas.Admin.Controllers.VideosController>
                .Instance(controller => controller.WithUser(u => u.InRole("Administrator")))
                .Calling(c => c.All())
                .ShouldReturn()
                .View();

        [Fact]
        public void GetAdminAllShouldReturnBadRequest()
            => MyController<Areas.Admin.Controllers.VideosController>
                .Instance()
                .Calling(c => c.All())
                .ShouldReturn()
                .BadRequest();

        [Fact]
        public void GetMakePublicShouldReturnBadRequest()
            => MyController<Areas.Admin.Controllers.VideosController>
                .Instance()
                .Calling(c => c.MakePublic(12))
                .ShouldReturn()
                .BadRequest();

        [Fact]
        public void GetMakePublicShouldRedirectToAction()
            => MyController<Areas.Admin.Controllers.VideosController>
                .Instance(controller => controller.WithUser(u => u.InRole("Administrator"))
                    .WithData(new Video()
                    {
                        Id = 12,
                        VideoUrl = "https://www.youtube.com/embed/g6PSwYx3jA0",
                        Description = "This is a test description for the video",
                        Title = "This is a test title for the video",
                        Date = "12-12-2022",
                        Likes = 0,
                        MemerId = 2
                    }))
                .Calling(c => c.MakePublic(12))
                .ShouldHave()
                .ActionAttributes(attributes => attributes.RestrictingForAuthorizedRequests())
                .TempData(tempData => tempData
                    .ContainingEntryWithKey(GlobalMessageKey))
                .AndAlso()
                .ShouldReturn()
                .RedirectToAction("All");
    }
}
