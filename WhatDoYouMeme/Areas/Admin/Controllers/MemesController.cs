using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WhatDoYouMeme.Infrastructure;
using WhatDoYouMeme.Services.Memes;
using static WhatDoYouMeme.WebConstants;

namespace WhatDoYouMeme.Areas.Admin.Controllers
{
    public class MemesController : AdminController
    {
        private readonly IMemeService memes;

        public MemesController(IMemeService memes)
        => this.memes = memes;

        [Authorize]
        public IActionResult All()
        {
            var allMemes = memes.AdminsAll();

            return View(allMemes);
        }

        [Authorize]
        public IActionResult MakePublic(int id)
        {
            if (!User.IsAdmin())
            {
                return BadRequest();
            }

            memes.MakePublic(id);

            TempData[GlobalMessageKey] = "This meme was approved successfully!";

            return RedirectToAction(nameof(All));
        }
    }
}
