using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WhatDoYouMeme.Infrastructure;
using WhatDoYouMeme.Services.Videos;
using static WhatDoYouMeme.WebConstants;

namespace WhatDoYouMeme.Areas.Admin.Controllers
{
    public class VideosController:AdminController
    {
        private readonly IVideoService videos;

        public VideosController(IVideoService videos)
            => this.videos = videos;
        [Authorize]
        public IActionResult All()
        {
            if (!User.IsAdmin())
            {
                return BadRequest();
            }

            var allVideos = videos.AdminsAll();

            return View(allVideos);
        }

        [Authorize]
        public IActionResult MakePublic(int id)
        {
            if (!User.IsAdmin())
            {
                return BadRequest();
            }

            videos.MakePublic(id);

            TempData[GlobalMessageKey] = "This video was approved successfully!";

            return RedirectToAction(nameof(All));
        }
    }
}
