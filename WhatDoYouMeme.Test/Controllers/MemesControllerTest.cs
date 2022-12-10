using MyTested.AspNetCore.Mvc;
using WhatDoYouMeme.Controllers;
using WhatDoYouMeme.Data.Models;
using WhatDoYouMeme.Models.Memes;
using Xunit;
using System.Linq;

namespace WhatDoYouMeme.Test.Controllers
{
    using static WebConstants;
    public class MemesControllerTest
    {
        [Fact]
        public void GetAllShouldReturnView()
            => MyController<MemesController>
                .Instance()
                .Calling(c => c.All())
                .ShouldReturn()
                .View();
        [Fact]
        public void GetMineShouldReturnView()
            => MyController<MemesController>
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
            => MyController<MemesController>
                .Instance()
                .Calling(c => c.Details(id))
                .ShouldReturn()
                .View();
        [Fact]
        public void GetAddShouldRedirectToActionForMemers()
            => MyController<MemesController>
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
            => MyController<MemesController>
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
                .View(new AddMemeFormModel());

        [Theory]
        [InlineData("https://static.wixstatic.com/media/bb1bd6_5798c09022ba43249a38bfea9be1db34~mv2.png/v1/fill/w_640,h_366,al_c,q_85,usm_0.66_1.00_0.01,enc_auto/bb1bd6_5798c09022ba43249a38bfea9be1db34~mv2.png", "This is a test description for the meme")]
        public void PostAddShouldBeForAuthorizedUserAndReturnRedirectWithValidModel(string imageURL, string description)
            => MyController<MemesController>
                .Instance(controller => controller.WithUser(u => u.WithIdentifier("123456789"))
                    .WithData(new Memer
                    {
                        UserId = "123456789",
                        Name = "Stivan Uzunov",
                        PhoneNumber = "+359123456789"
                    }))
                .Calling(c => c.Add(new AddMemeFormModel()
                {
                    ImageUrl = imageURL,
                    Description = description,
                }))
                .ShouldHave()
                .ActionAttributes(attributes => attributes.RestrictingForHttpMethod(HttpMethod.Post).RestrictingForAuthorizedRequests())
                .ValidModelState()
                .Data(data => data
                    .WithSet<Post>(meme => meme
                        .Any(m => m.ImageUrl == imageURL && m.Description == description)))
                .TempData(tempData => tempData
                    .ContainingEntryWithKey(GlobalMessageKey))
                .AndAlso()
                .ShouldReturn()
                .RedirectToAction("All");

        [Theory]
        [InlineData("https://static.wixstatic.com/media/bb1bd6_5798c09022ba43249a38bfea9be1db34~mv2.png/v1/fill/w_640,h_366,al_c,q_85,usm_0.66_1.00_0.01,enc_auto/bb1bd6_5798c09022ba43249a38bfea9be1db34~mv2.png", "This is a test description for the meme")]
        public void PostAddShouldBeForAuthorizedUserAndReturnRedirectToActionForMemers(string imageURL, string description)
            => MyController<MemesController>
                .Instance(controller => controller.WithUser())
                .Calling(c => c.Add(new AddMemeFormModel()
                {
                    ImageUrl = imageURL,
                    Description = description,
                }))
                .ShouldReturn()
                .RedirectToAction("Create", "Memers");

        [Fact]
        public void GetEditShouldReturnView()
            => MyController<MemesController>
                .Instance(controller => controller.WithUser(u => u.WithIdentifier("123456789"))
                    .WithData(new Memer
                    {
                        Id = 2,
                        UserId = "123456789",
                        Name = "Stivan Uzunov",
                        PhoneNumber = "+359123456789"
                    }).WithData(new Post
                    {
                        Id = 12,
                        ImageUrl = "https://static.wixstatic.com/media/bb1bd6_5798c09022ba43249a38bfea9be1db34~mv2.png/v1/fill/w_640,h_366,al_c,q_85,usm_0.66_1.00_0.01,enc_auto/bb1bd6_5798c09022ba43249a38bfea9be1db34~mv2.png",
                        Description = "This is a test description for the meme",
                        isPublic = true,
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
            => MyController<MemesController>
                .Instance(controller => controller.WithUser().WithData(new Post
                {
                    Id = 12,
                    ImageUrl = "https://static.wixstatic.com/media/bb1bd6_5798c09022ba43249a38bfea9be1db34~mv2.png/v1/fill/w_640,h_366,al_c,q_85,usm_0.66_1.00_0.01,enc_auto/bb1bd6_5798c09022ba43249a38bfea9be1db34~mv2.png",
                    Description = "This is a test description for the meme",
                    isPublic = true,
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
            => MyController<MemesController>
                .Instance(controller => controller.WithUser(u => u.InRole("Administrator"))
                    .WithData(new Post
                    {
                        Id = 12,
                        ImageUrl = "https://static.wixstatic.com/media/bb1bd6_5798c09022ba43249a38bfea9be1db34~mv2.png/v1/fill/w_640,h_366,al_c,q_85,usm_0.66_1.00_0.01,enc_auto/bb1bd6_5798c09022ba43249a38bfea9be1db34~mv2.png",
                        Description = "This is a test description for the meme",
                        isPublic = true,
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
            "https://static.wixstatic.com/media/bb1bd6_5798c09022ba43249a38bfea9be1db34~mv2.png/v1/fill/w_640,h_366,al_c,q_85,usm_0.66_1.00_0.01,enc_auto/bb1bd6_5798c09022ba43249a38bfea9be1db34~mv2.png"
            , "This is a test description for the meme",
            "https://static01.nyt.com/images/2021/04/30/multimedia/30xp-meme/29xp-meme-mediumSquareAt3X-v5.jpg",
            "This is a changed test description for the meme")]

        public void PostEditShouldRedirectToAction(string imageURL, string description, string newImageURL, string newDescription)
            => MyController<MemesController>
                .Instance(controller => controller.WithUser(u => u.WithIdentifier("123456789"))
                    .WithData(new Memer
                    {
                        Id = 2,
                        UserId = "123456789",
                        Name = "Stivan Uzunov",
                        PhoneNumber = "+359123456789"
                    }).WithData(new Post
                    {
                        Id = 12,
                        ImageUrl = imageURL,
                        Description = description,
                        isPublic = true,
                        Date = "12-12-2022",
                        Likes = 0,
                        MemerId = 2
                    }))
                .Calling(c => c.Edit(12, new EditMemeFormModel
                {
                    ImageUrl = newImageURL,
                    Description = newDescription,
                    MemerId = 2
                }))
                .ShouldHave()
                .ActionAttributes(attributes => attributes.RestrictingForHttpMethod(HttpMethod.Post).RestrictingForAuthorizedRequests())
                .ValidModelState()
                .Data(data => data
                    .WithSet<Post>(meme => meme
                        .Any(m => m.ImageUrl == newImageURL && m.Description == newDescription)))
                .TempData(tempData => tempData
                    .ContainingEntryWithKey(GlobalMessageKey))
                .AndAlso()
                .ShouldReturn()
                .RedirectToAction("All");

        [Theory]
        [InlineData(
            "https://static.wixstatic.com/media/bb1bd6_5798c09022ba43249a38bfea9be1db34~mv2.png/v1/fill/w_640,h_366,al_c,q_85,usm_0.66_1.00_0.01,enc_auto/bb1bd6_5798c09022ba43249a38bfea9be1db34~mv2.png"
            , "This is a test description for the meme",
            "https://static01.nyt.com/images/2021/04/30/multimedia/30xp-meme/29xp-meme-mediumSquareAt3X-v5.jpg",
            "This is a changed test description for the meme")]

        public void PostEditShouldRedirectToActionForMemers(string imageURL, string description, string newImageURL,
            string newDescription)
            => MyController<MemesController>
                .Instance(controller => controller.WithUser()
                   .WithData(new Post
                   {
                       Id = 12,
                       ImageUrl = imageURL,
                       Description = description,
                       isPublic = true,
                       Date = "12-12-2022",
                       Likes = 0,
                       MemerId = 2
                   }))
                .Calling(c => c.Edit(12, new EditMemeFormModel
                {
                    ImageUrl = newImageURL,
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
          "https://static.wixstatic.com/media/bb1bd6_5798c09022ba43249a38bfea9be1db34~mv2.png/v1/fill/w_640,h_366,al_c,q_85,usm_0.66_1.00_0.01,enc_auto/bb1bd6_5798c09022ba43249a38bfea9be1db34~mv2.png"
          , "This is a test description for the meme",
          "https://static01.nyt.com/images/2021/04/30/multimedia/30xp-meme/29xp-meme-mediumSquareAt3X-v5.jpg",
          "This is a changed test description for the meme")]

        public void PostEditShouldRedirectToActionForAdmins(string imageURL, string description, string newImageURL, string newDescription)
          => MyController<MemesController>
              .Instance(controller => controller.WithUser(u => u.InRole("Administrator"))
                 .WithData(new Post
                 {
                     Id = 12,
                     ImageUrl = imageURL,
                     Description = description,
                     isPublic = true,
                     Date = "12-12-2022",
                     Likes = 0,
                     MemerId = 2
                 }))
              .Calling(c => c.Edit(12, new EditMemeFormModel
              {
                  ImageUrl = newImageURL,
                  Description = newDescription,
                  MemerId = 2
              }))
              .ShouldHave()
              .ActionAttributes(attributes => attributes.RestrictingForHttpMethod(HttpMethod.Post).RestrictingForAuthorizedRequests())
              .ValidModelState()
              .Data(data => data
                  .WithSet<Post>(meme => meme
                      .Any(m => m.ImageUrl == newImageURL && m.Description == newDescription)))
              .TempData(tempData => tempData
                  .ContainingEntryWithKey(GlobalMessageKey))
              .AndAlso()
              .ShouldReturn()
              .RedirectToAction("All");

        [Fact]
        public void GetDeleteShouldRedirectToAction()
            => MyController<MemesController>
                .Instance(controller => controller.WithUser(u => u.WithIdentifier("123456789"))
                    .WithData(new Memer
                    {
                        Id = 2,
                        UserId = "123456789",
                        Name = "Stivan Uzunov",
                        PhoneNumber = "+359123456789"
                    }).WithData(new Post
                    {
                        Id = 12,
                        ImageUrl =
                        "https://static.wixstatic.com/media/bb1bd6_5798c09022ba43249a38bfea9be1db34~mv2.png/v1/fill/w_640,h_366,al_c,q_85,usm_0.66_1.00_0.01,enc_auto/bb1bd6_5798c09022ba43249a38bfea9be1db34~mv2.png",
                        Description = "This is a test description for the meme",
                        isPublic = true,
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
                    => MyController<MemesController>
                        .Instance(controller => controller.WithUser()
                           .WithData(new Post
                           {
                               Id = 12,
                               ImageUrl =
                                "https://static.wixstatic.com/media/bb1bd6_5798c09022ba43249a38bfea9be1db34~mv2.png/v1/fill/w_640,h_366,al_c,q_85,usm_0.66_1.00_0.01,enc_auto/bb1bd6_5798c09022ba43249a38bfea9be1db34~mv2.png",
                               Description = "This is a test description for the meme",
                               isPublic = true,
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
            => MyController<MemesController>
                .Instance(controller => controller.WithUser(u => u.InRole("Administrator"))
                    .WithData(new Post
                    {
                        Id = 12,
                        ImageUrl =
                            "https://static.wixstatic.com/media/bb1bd6_5798c09022ba43249a38bfea9be1db34~mv2.png/v1/fill/w_640,h_366,al_c,q_85,usm_0.66_1.00_0.01,enc_auto/bb1bd6_5798c09022ba43249a38bfea9be1db34~mv2.png",
                        Description = "This is a test description for the meme",
                        isPublic = true,
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
            => MyController<MemesController>
                .Instance(controller => controller.WithUser(u => u.WithIdentifier("123456789"))
                    .WithData(new Memer
                    {
                        Id = 2,
                        UserId = "123456789",
                        Name = "Stivan Uzunov",
                        PhoneNumber = "+359123456789"
                    }).WithData(new Post
                    {
                        Id = 12,
                        ImageUrl =
                            "https://static.wixstatic.com/media/bb1bd6_5798c09022ba43249a38bfea9be1db34~mv2.png/v1/fill/w_640,h_366,al_c,q_85,usm_0.66_1.00_0.01,enc_auto/bb1bd6_5798c09022ba43249a38bfea9be1db34~mv2.png",
                        Description = "This is a test description for the meme",
                        isPublic = true,
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
                .RedirectToAction("All", "Memes");

        [Fact]
        public void GetLikeShouldRedirectToActionForMemers()
            => MyController<MemesController>
                .Instance(controller => controller.WithUser()
                    .WithData(new Post
                    {
                        Id = 12,
                        ImageUrl =
                            "https://static.wixstatic.com/media/bb1bd6_5798c09022ba43249a38bfea9be1db34~mv2.png/v1/fill/w_640,h_366,al_c,q_85,usm_0.66_1.00_0.01,enc_auto/bb1bd6_5798c09022ba43249a38bfea9be1db34~mv2.png",
                        Description = "This is a test description for the meme",
                        isPublic = true,
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
            => MyController<Areas.Admin.Controllers.MemesController>
                .Instance(controller => controller.WithUser(u => u.InRole("Administrator")))
                .Calling(c => c.All())
                .ShouldReturn()
                .View();

        [Fact]
        public void GetAdminAllShouldReturnBadRequest()
            => MyController<Areas.Admin.Controllers.MemesController>
                .Instance()
                .Calling(c => c.All())
                .ShouldReturn()
                .BadRequest();

        [Fact]
        public void GetMakePublicShouldReturnBadRequest()
            => MyController<Areas.Admin.Controllers.MemesController>
                .Instance()
                .Calling(c => c.MakePublic(12))
                .ShouldReturn()
                .BadRequest();

        [Fact]
        public void GetMakePublicShouldRedirectToAction()
            => MyController<Areas.Admin.Controllers.MemesController>
                .Instance(controller => controller.WithUser(u => u.InRole("Administrator"))
                    .WithData(new Post
                    {
                        Id = 12,
                        ImageUrl =
                            "https://static.wixstatic.com/media/bb1bd6_5798c09022ba43249a38bfea9be1db34~mv2.png/v1/fill/w_640,h_366,al_c,q_85,usm_0.66_1.00_0.01,enc_auto/bb1bd6_5798c09022ba43249a38bfea9be1db34~mv2.png",
                        Description = "This is a test description for the meme",
                        isPublic = true,
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
