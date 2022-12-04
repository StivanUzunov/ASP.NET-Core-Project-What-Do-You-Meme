using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WhatDoYouMeme.Infrastructure;
using WhatDoYouMeme.Models.Memers;
using WhatDoYouMeme.Services.Memers;
using static WhatDoYouMeme.WebConstants;

namespace WhatDoYouMeme.Controllers
{
    public class MemersController:Controller
    {
        private readonly IMemerService memers;

        public MemersController(IMemerService memers) 
            => this.memers = memers;

        [Authorize]
        public IActionResult Create() => View();

        [Authorize]
        [HttpPost]
        public IActionResult Create(BecomeMemerFormModel memer)
        {
            var userId = User.GetUserId();

            if (memers.UserIsAlreadyAMemer(userId))
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return View(memer);
            }

            memers.CreateMemer(memer,userId);

            TempData[GlobalMessageKey] = "Thank you for becoming a Memer!";

            return RedirectToAction("All", "Memes");
        }
    }
}
