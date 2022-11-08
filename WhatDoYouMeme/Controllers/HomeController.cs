using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Linq;
using Microsoft.CodeAnalysis.Operations;
using Microsoft.Extensions.Caching.Memory;
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
        private readonly IMemoryCache cache;

        public HomeController(ApplicationDbContext data, IMemoryCache cache)
        {
            this.data = data;
            this.cache = cache;
        } 
 
        public IActionResult Index()
        {
            const string latestMemesCacheKey = "latestMemesCacheKey";

            var latestMemes = this.cache.Get<List<MemeListingViewModel>>(latestMemesCacheKey);
            if (latestMemes == null)
            {
                 latestMemes = this.data
                    .Posts
                    .Where(m=>m.isPublic)
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

                 var cacheOptions = new MemoryCacheEntryOptions()
                     .SetAbsoluteExpiration(TimeSpan.FromMinutes(15));

                 this.cache.Set(latestMemesCacheKey, latestMemes, cacheOptions);
            }
            var totalMemes = this.data.Posts.Count(m=>m.isPublic);
            var totalUsers = this.data.Users.Count();
            

            return View(new IndexViewModel
            {
                TotalMemes = totalMemes,
                TotalUsers = totalUsers,
                Posts = latestMemes
            });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() => View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
