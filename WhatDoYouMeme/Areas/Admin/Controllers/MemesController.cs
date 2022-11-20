using Microsoft.AspNetCore.Mvc;
using WhatDoYouMeme.Services.Memes;

namespace WhatDoYouMeme.Areas.Admin.Controllers
{
    public class MemesController : AdminController
    {
        private readonly IMemeService memes;

        public MemesController(IMemeService memes)
        => this.memes = memes;

        public IActionResult All()
        {
            var allMemes = memes.AdminsAll();

            return View(allMemes);
        }
    }
}
