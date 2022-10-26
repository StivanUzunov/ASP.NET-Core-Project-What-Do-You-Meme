using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WhatDoYouMeme.Data;
using WhatDoYouMeme.Data.Models;
using WhatDoYouMeme.Models.Memes;

namespace WhatDoYouMeme.Controllers
{
    public class MemesController : Controller
    {
        private readonly ApplicationDbContext data;
        public MemesController(ApplicationDbContext data) => this.data = data;

        [Authorize]
        public IActionResult Mine()
        {

            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var myMemes = this.data
                            .Posts
                            .Where(m=>m.Memer.UserId==userId)
                            .OrderByDescending(m => m.Id)
                            .Select(m => new MemeListingViewModel
                            {
                                Id = m.Id,
                                ImageUrl = m.ImageUrl,
                                Description = m.Description,
                                Date = m.Date,
                                Likes = m.Likes,
                                MemerId = m.MemerId
                    // Comments = m.Comments
                })
                          .ToList();

            return View(myMemes);

        }

        [Authorize]
        public IActionResult Add()
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var userIsMemer = this.data.Memers.Any(m => m.UserId == userId);

            if (!userIsMemer)
            {

                return RedirectToAction(nameof(MemersController.Create), "Memers");
            };


            return View(new AddMemeFormModel());
        }

        public IActionResult All()
        {

            var memes = this.data
                .Posts
                .OrderByDescending(m => m.Id)
                .Select(m => new MemeListingViewModel
                {
                    Id = m.Id,
                    ImageUrl = m.ImageUrl,
                    Description = m.Description,
                    Date = m.Date,
                    Likes = m.Likes,
                    MemerId = m.MemerId
                    // Comments = m.Comments
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
                MemerId = memerId
            };

            this.data.Posts.Add(memeData);
            this.data.SaveChanges();

            return RedirectToAction("Index", "Home");
        }
    }
}
