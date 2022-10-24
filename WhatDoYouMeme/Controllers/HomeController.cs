using System;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Linq;
using Microsoft.CodeAnalysis.Operations;
using WhatDoYouMeme.Data;
using WhatDoYouMeme.Data.Models;
using WhatDoYouMeme.Models;
using WhatDoYouMeme.Models.Home;
using WhatDoYouMeme.Models.Memes;

namespace WhatDoYouMeme.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext data;
        public HomeController(ApplicationDbContext data) => this.data = data;
 
        public IActionResult Index()
        {
            var totalMemes = this.data.Posts.Count();
            var totalUsers = this.data.Users.Count();
            var memes = this.data
                .Posts
                .OrderByDescending(m => m.Id)
                .Select(m => new MemeListingViewModel
                {
                    Id = m.Id,
                    ImageUrl = m.ImageUrl,
                    Description = m.Description,
                    Date = m.Date,
                    Likes = m.Likes
                })
                .Take(3)
                .ToList();

            return View(new IndexViewModel
            {
                TotalMemes = totalMemes,
                TotalUsers = totalUsers,
                Posts = memes
            });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() => View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
