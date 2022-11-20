using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using WhatDoYouMeme.Models;
using WhatDoYouMeme.Models.Home;
using WhatDoYouMeme.Models.Memes;
using WhatDoYouMeme.Services.Home;

namespace WhatDoYouMeme.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMemoryCache cache;
        private readonly IHomeService home;

        public HomeController(IMemoryCache cache,IHomeService home)
        {
            this.cache = cache;
            this.home = home;
        } 
 
        public IActionResult Index()
        {
            const string latestMemesCacheKey = "latestMemesCacheKey";

            var latestMemes = cache.Get<List<MemeListingViewModel>>(latestMemesCacheKey);
            if (latestMemes == null)
            {
                latestMemes = home.GetLatestMemes();

                 var cacheOptions = new MemoryCacheEntryOptions()
                     .SetAbsoluteExpiration(TimeSpan.FromMinutes(15));

                 cache.Set(latestMemesCacheKey, latestMemes, cacheOptions);
            }

            var totalMemes = home.GetTotalMemes();
            var totalVideos = home.GetTotalVideos();
            var totalUsers = home.GetTotalUsers();
            

            return View(new IndexViewModel
            {
                TotalMemes = totalMemes,
                TotalVideos = totalVideos,
                TotalUsers = totalUsers,
                Posts = latestMemes
            });
        }

        public IActionResult About() => View();

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() => View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
