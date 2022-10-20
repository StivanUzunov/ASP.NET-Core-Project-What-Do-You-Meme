using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using WhatDoYouMeme.Data;
using WhatDoYouMeme.Data.Models;
using WhatDoYouMeme.Models.Memes;

namespace WhatDoYouMeme.Controllers
{
    public class MemesController:Controller
    {
        private readonly ApplicationDbContext data;
        public MemesController(ApplicationDbContext data) => this.data = data;
        public IActionResult Add() => View( new AddMemeFormModel());

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
                   // Comments = m.Comments
                })
                .ToList();

            return View(memes);
        }
        [HttpPost]
        public IActionResult Add(AddMemeFormModel meme)
        {
            if (!ModelState.IsValid)
            {
                return View(meme);
            }

            var memeData = new Post
            {
                ImageUrl = meme.ImageUrl,
                Description = meme.Description,
                Likes  = 0,
                Date = DateTime.Now.ToString(CultureInfo.CurrentCulture),
                Comments = new List<Comment>()
            };

            this.data.Posts.Add(memeData);
            this.data.SaveChanges();

            return RedirectToAction("Index", "Home");
        }
    }
}
