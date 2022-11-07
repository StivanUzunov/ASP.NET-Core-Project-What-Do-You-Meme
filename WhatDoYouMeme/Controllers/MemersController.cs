using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using WhatDoYouMeme.Data;
using WhatDoYouMeme.Data.Models;
using WhatDoYouMeme.Models.Memers;
using static WhatDoYouMeme.WebConstants;
namespace WhatDoYouMeme.Controllers
{
    public class MemersController:Controller
    {
        private readonly ApplicationDbContext data;

        public MemersController(ApplicationDbContext data) => this.data = data;

        [Authorize]
        public IActionResult Create() => View();

        [Authorize]
        [HttpPost]
        public IActionResult Create(BecomeMemerFormModel memer)
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var userIsAlreadyMemer = this.data
                .Memers
                .Any(m => m.UserId == userId);
            if (userIsAlreadyMemer)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return View(memer);
            }

            var memerData = new Memer
            {
                Name = memer.Name,
                PhoneNumber = memer.PhoneNumber,
                UserId = userId
            };

            this.data.Memers.Add(memerData);
            this.data.SaveChanges();

            TempData[GlobalMessageKey] = "Thank you for becoming a Memer!";

            return RedirectToAction("All", "Memes");
        }
    }
}
