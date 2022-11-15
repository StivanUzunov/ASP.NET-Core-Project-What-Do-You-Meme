using System.Linq;
using Microsoft.AspNetCore.Mvc;
using WhatDoYouMeme.Data;
using WhatDoYouMeme.Data.Models;
using WhatDoYouMeme.Models.Memes;
using WhatDoYouMeme.Models.Videos;

namespace WhatDoYouMeme.Areas.Admin.Controllers
{
    public class VideosController:AdminController
    {
        private readonly ApplicationDbContext data;

        public VideosController(ApplicationDbContext data)
            => this.data = data;



        public IActionResult All()
        {
            var videos = this.data
                .Videos
                .Where(v => v.IsPublic == false)
                .OrderBy(v => v.Id)
                .Select(v => new VideoListingViewModel
                {
                    Id = v.Id,
                    VideoUrl = v.VideoUrl,
                    Title = v.Title,
                    Description = v.Description,
                    Date = v.Date,
                    Likes = v.Likes,
                    MemerId = v.MemerId,
                    MemerName = v.Memer.Name,
                    Comments = v.Comments.ToList()
                })
                .ToList();

            return View(videos);
        }
    }
}
