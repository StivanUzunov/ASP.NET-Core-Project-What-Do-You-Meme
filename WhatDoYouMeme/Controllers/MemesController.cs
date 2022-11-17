using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WhatDoYouMeme.Data;
using WhatDoYouMeme.Services;
using WhatDoYouMeme.Data.Models;
using WhatDoYouMeme.Infrastructure;
using WhatDoYouMeme.Models.Memes;
using static WhatDoYouMeme.WebConstants;

namespace WhatDoYouMeme.Controllers
{
    public class MemesController : Controller
    {
        private readonly ApplicationDbContext data;
        private readonly IMemerService memers;

        public MemesController(ApplicationDbContext data, IMemerService memers)
        {
            this.data = data;
            this.memers = memers;
        }

        [Authorize]
        public IActionResult Mine()
        {

            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;


            var myMemes = this.data
                .Posts
                .Where(m => m.Memer.UserId == userId)
                .OrderByDescending(m => m.Id)
                .Select(m => new MemeListingViewModel
                {
                    Id = m.Id,
                    ImageUrl = m.ImageUrl,
                    Description = m.Description,
                    Date = m.Date,
                    Likes = m.Likes,
                    MemerId = m.MemerId,
                    MemerName = m.Memer.Name,
                    Comments = m.Comments.ToList()
                })
                .ToList();

            return View(myMemes);

        }

        public IActionResult Details(int id)
        {
            var memeData = this.data.
                Posts.
                Where(m => m.Id == id)
                .Select(m => new MemeListingViewModel
                {
                    Id = m.Id,
                    ImageUrl = m.ImageUrl,
                    Description = m.Description,
                    Date = m.Date,
                    Likes = m.Likes,
                    MemerId = m.MemerId,
                    MemerName = m.Memer.Name,
                    Comments = m.Comments.OrderByDescending(l => l.Likes).ToList()
                }).FirstOrDefault();


            return View(memeData);
        }

        [Authorize]
        public IActionResult Add()
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;


            if (!this.memers.IsMemer(userId))
            {

                return RedirectToAction(nameof(MemersController.Create), "Memers");
            };


            return View(new AddMemeFormModel());
        }

        public IActionResult All()
        {

            var memes = this.data
                .Posts
                .Where(m => m.isPublic)
                .OrderByDescending(m => m.Id)
                .Select(m => new MemeListingViewModel
                {
                    Id = m.Id,
                    ImageUrl = m.ImageUrl,
                    Description = m.Description,
                    Date = m.Date,
                    Likes = m.Likes,
                    MemerId = m.MemerId,
                    MemerName = m.Memer.Name,
                    Comments = m.Comments.OrderByDescending(c => c.Likes).Take(3).ToList(),
                })
                .ToList();

            return View(memes);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Add(AddMemeFormModel meme)
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var memerId = this.data.Memers.Where(m => m.UserId == userId).Select(m => m.Id).FirstOrDefault();

            if (memerId == 0)
            {

                return RedirectToAction(nameof(MemersController.Create), "Memers");
            }

            if (!ModelState.IsValid)
            {
                return View(meme);
            }

            var memeData = new Post
            {
                ImageUrl = meme.ImageUrl,
                Description = meme.Description,
                Likes = 0,
                Date = DateTime.Now.ToString(CultureInfo.CurrentCulture),
                Comments = new List<Comment>(),
                MemerId = memerId,
                isPublic = false,

            };

            this.data.Posts.Add(memeData);
            this.data.SaveChanges();

            TempData[GlobalMessageKey] = "Your Meme was added successfully and it is waiting for approval!";

            return RedirectToAction(nameof(Details), memeData.Id);
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

            var meme = this.data
                .Posts
                .Where(m => m.Id == id)
                .Select(m => new EditMemeFormModel
                {
                    ImageUrl = m.ImageUrl,
                    Description = m.Description,
                    MemerId = m.MemerId
                })
                .FirstOrDefault();

            if (meme.MemerId != memerId && !User.IsAdmin())
            {
                return Unauthorized();
            }

            return View(new EditMemeFormModel
            {
                ImageUrl = meme.ImageUrl,
                Description = meme.Description,
                MemerId = meme.MemerId
            });
        }

        [HttpPost]
        [Authorize]
        public IActionResult Edit(int id, EditMemeFormModel meme)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var memerId = this.data.Memers.Where(m => m.UserId == userId).Select(m => m.Id).FirstOrDefault();

            var memeData = this.data.Posts.Where(m => m.Id == id).First();
            if (memerId == 0 && !User.IsAdmin())
            {

                return RedirectToAction(nameof(MemersController.Create), "Memers");
            }

            if (!ModelState.IsValid)
            {
                return View(meme);
            }

            if (memeData.MemerId != memerId && !User.IsAdmin())
            {
                return BadRequest();
            }

            memeData.ImageUrl = meme.ImageUrl;
            memeData.Description = meme.Description;
            memeData.isPublic = false;

            this.data.SaveChanges();

            TempData[GlobalMessageKey] = "Your Meme was edited successfully and it is waiting for approval!";

            return RedirectToAction(nameof(All));

        }

        public IActionResult MakePublic(int id)
        {
            var memeData = this.data.Posts.Where(m => m.Id == id).First();

            memeData.isPublic = true;

            this.data.SaveChanges();

            TempData[GlobalMessageKey] = "This meme was approved successfully!";

            var url = Url.RouteUrl("areas", new { controller = "Memes", action = "All", area = "Admin" });
            return Redirect(url);
        }

        [Authorize]
        public IActionResult Delete(int id)
        {
            var meme = this.data.Posts.Where(m => m.Id == id).First();

            this.data.Remove(meme);

            this.data.SaveChanges();

            return RedirectToAction(nameof(All));
        }

        [Authorize]
        public IActionResult Like(int id)
        {
            var meme = this.data.Posts.Where(m => m.Id == id).First();

            meme.Likes++;


            this.data.SaveChanges();

            return RedirectToAction(nameof(All), "Memes", $"{id}");

        }
    }
}
