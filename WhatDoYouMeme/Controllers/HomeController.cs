using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Linq;
using WhatDoYouMeme.Data;
using WhatDoYouMeme.Models;
using WhatDoYouMeme.Models.Memes;

namespace WhatDoYouMeme.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext data;
        public HomeController(ApplicationDbContext data) => this.data = data;
        public IActionResult Index()
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
                .Take(3)
                .ToList();

            return View(memes);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() => View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
