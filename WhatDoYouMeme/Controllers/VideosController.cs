using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WhatDoYouMeme.Data;
using WhatDoYouMeme.Data.Models;
using WhatDoYouMeme.Models.Videos;
using WhatDoYouMeme.Services;
using static WhatDoYouMeme.WebConstants;
using WhatDoYouMeme.Infrastructure;

namespace WhatDoYouMeme.Controllers
{
    public class VideosController : Controller
    {
        private readonly ApplicationDbContext data;
        private readonly IMemerService memers;

        public VideosController(ApplicationDbContext data, IMemerService memers)
        {
            this.data = data;
            this.memers = memers;
        }


        [Authorize]
        public IActionResult Add()
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;


            if (!this.memers.IsMemer(userId))
            {

                return RedirectToAction(nameof(MemersController.Create), "Memers");
            };


            return View(new AddVideoFormModel());
        }

        [HttpPost]
        [Authorize]
        public IActionResult Add(AddVideoFormModel video)
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var memerId = this.data.Memers.Where(m => m.UserId == userId).Select(m => m.Id).FirstOrDefault();

            if (memerId == 0)
            {

                return RedirectToAction(nameof(MemersController.Create), "Memers");
            }

            if (!ModelState.IsValid)
            {
                return View(video);
            }

            var videoData = new Video()
            {
                VideoUrl = video.VideoUrl,
                Title = video.Title,
                Description = video.Description,
                Likes = 0,
                Date = DateTime.Now.ToString(CultureInfo.CurrentCulture),
                Comments = new List<Comment>(),
                MemerId = memerId,
                IsPublic = false,
            };

            this.data.Videos.Add(videoData);
            this.data.SaveChanges();

            TempData[GlobalMessageKey] = "Your video was added successfully and it is waiting for approval!";

            return RedirectToAction(nameof(All));
        }

        public IActionResult Details(int id)
        {
            var videoData = this.data.
                Videos.
                Where(v => v.Id == id)
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
                    Comments = v.Comments.OrderByDescending(l => l.Likes).ToList()
                }).FirstOrDefault();


            return View(videoData);
        }

        public IActionResult All()
        {

            var videos = this.data
                .Videos
                .Where(v => v.IsPublic)
                .OrderByDescending(m => m.Id)
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
                    Comments = v.Comments.OrderByDescending(c => c.Likes).Take(3).ToList(),
                })
                .ToList();

            return View(videos);
        }

        [Authorize]
        public IActionResult Like(int id)
        {
            var video = this.data.Videos.Where(m => m.Id == id).First();

            video.Likes++;


            this.data.SaveChanges();

            return RedirectToAction(nameof(All), "Videos", $"{id}");

        }

        [Authorize]
        public IActionResult Delete(int id)
        {
            var video = this.data.Videos.Where(m => m.Id == id).First();

            this.data.Remove(video);

            this.data.SaveChanges();

            return RedirectToAction(nameof(All));
        }

        [Authorize]
        public IActionResult Edit(int id)
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var memerId = this.data.Memers.Where(m => m.UserId == userId).Select(m => m.Id).FirstOrDefault();
            if (!memers.IsMemer(userId) && !User.IsAdmin())
            {
                return RedirectToAction(nameof(MemersController.Create), "Memers");
            }

            var video = this.data
                .Videos
                .Where(v => v.Id == id)
                .Select(v => new EditVideoFormModel
                {
                    VideoUrl = v.VideoUrl,
                    Title = v.Title,
                    Description = v.Description,
                    MemerId = v.MemerId
                })
                .FirstOrDefault();

            if (video.MemerId != memerId && !User.IsAdmin())
            {
                return Unauthorized();
            }

            return View(new EditVideoFormModel
            {
                VideoUrl = video.VideoUrl,
                Title = video.Title,
                Description = video.Description,
                MemerId = video.MemerId
            });
        }

        [HttpPost]
        [Authorize]
        public IActionResult Edit(int id, EditVideoFormModel video)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var memerId = this.data.Memers.Where(m => m.UserId == userId).Select(m => m.Id).FirstOrDefault();
            var videoData = this.data.Videos.Where(m => m.Id == id).First();

            if (memerId == 0 && !User.IsAdmin())
            {

                return RedirectToAction(nameof(MemersController.Create), "Memers");
            }

            if (!ModelState.IsValid)
            {
                return View(video);
            }

            if (videoData.MemerId != memerId && !User.IsAdmin())
            {
                return BadRequest();
            }

            videoData.VideoUrl = video.VideoUrl;
            videoData.Title = video.Title;
            videoData.Description = video.Description;
            videoData.IsPublic = false;

            this.data.SaveChanges();

            TempData[GlobalMessageKey] = "Your video was edited successfully and it is waiting for approval!";

            return RedirectToAction(nameof(All));

        }

        [Authorize]
        public IActionResult Mine()
        {

            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;


            var myVideos = this.data
                .Videos
                .Where(v => v.Memer.UserId == userId)
                .OrderByDescending(v => v.Id)
                .Select(video => new VideoListingViewModel
                {
                    Id = video.Id,
                    VideoUrl = video.VideoUrl,
                    Title = video.Title,
                    Description = video.Description,
                    Date = video.Date,
                    Likes = video.Likes,
                    MemerId = video.MemerId,
                    MemerName = video.Memer.Name,
                    Comments = video.Comments.ToList()
                })
                .ToList();

            return View(myVideos);

        }

        public IActionResult MakePublic(int id)
        {
            var memeData = this.data.Videos.Where(m => m.Id == id).First();

            memeData.IsPublic = true;

            this.data.SaveChanges();

            TempData[GlobalMessageKey] = "This video was approved successfully!";

            var url = Url.RouteUrl("areas", new { controller = "Videos", action = "All", area = "Admin" });
            return Redirect(url);
        }
    }
}
