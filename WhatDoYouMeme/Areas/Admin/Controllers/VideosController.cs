using Microsoft.AspNetCore.Mvc;
using WhatDoYouMeme.Services.Videos;

namespace WhatDoYouMeme.Areas.Admin.Controllers
{
    public class VideosController:AdminController
    {
        private readonly IVideoService videos;

        public VideosController(IVideoService videos)
            => this.videos = videos;

        public IActionResult All()
        {
            var allVideos = videos.AdminsAll();

            return View(allVideos);
        }
    }
}
