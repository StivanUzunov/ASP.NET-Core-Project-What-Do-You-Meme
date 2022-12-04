using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WhatDoYouMeme.Infrastructure;
using WhatDoYouMeme.Models.Videos;
using WhatDoYouMeme.Services.Memers;
using WhatDoYouMeme.Services.Videos;
using static WhatDoYouMeme.WebConstants;

namespace WhatDoYouMeme.Controllers
{
    public class VideosController : Controller
    {
        private readonly IMemerService memers;
        private readonly IVideoService videos;

        public VideosController(IMemerService memers,IVideoService videos)
        {
            this.memers = memers;
            this.videos = videos;
        }

        [Authorize]
        public IActionResult Add()
        {
            var userId = User.GetUserId();

            if (!memers.IsMemer(userId))
            {
                return RedirectToAction(nameof(MemersController.Create), "Memers");
            };

            return View(new AddVideoFormModel());
        }

        [HttpPost]
        [Authorize]
        public IActionResult Add(AddVideoFormModel video)
        {
            var userId = User.GetUserId();
            var memerId = memers.GetMemerId(userId);

            if (memerId == 0)
            {
                return RedirectToAction(nameof(MemersController.Create), "Memers");
            }

            if (!ModelState.IsValid)
            {
                return View(video);
            }

            videos.Add(video,memerId);

            TempData[GlobalMessageKey] = "Your video was added successfully and it is waiting for approval!";

            return RedirectToAction(nameof(All));
        }

        public IActionResult Details(int id)
        {
            var videoData = videos.Details(id);

            return View(videoData);
        }

        public IActionResult All()
        {
            var videosData = videos.All();

            return View(videosData);
        }

        [Authorize]
        public IActionResult Like(int id)
        {
            var userId = User.GetUserId();

            if (!memers.IsMemer(userId))
            {
                return RedirectToAction(nameof(MemersController.Create), "Memers");
            };

            videos.Like(id);

            return RedirectToAction(nameof(All), "Videos", $"{id}");
        }

        [Authorize]
        public IActionResult Delete(int id)
        {
            videos.Delete(id);

            return RedirectToAction(nameof(All));
        }

        [Authorize]
        public IActionResult Edit(int id)
        {
            var userId = User.GetUserId();
            var memerId = memers.GetMemerId(userId);

            if (!memers.IsMemer(userId) && !User.IsAdmin())
            {
                return RedirectToAction(nameof(MemersController.Create), "Memers");
            }

            var video = videos.Edit(id);

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
            var userId = User.GetUserId();
            var memerId = memers.GetMemerId(userId);
            var videoData = videos.GetVideo(id);

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

            videos.EditVideo(videoData,video);

            TempData[GlobalMessageKey] = "Your video was edited successfully and it is waiting for approval!";

            return RedirectToAction(nameof(All));

        }

        [Authorize]
        public IActionResult Mine()
        {
            var userId = User.GetUserId();

            var myVideos = videos.Mine(userId);

            return View(myVideos);
        }
       
    }
}
