using Microsoft.AspNetCore.Mvc;

namespace WhatDoYouMeme.Areas.Admin.Controllers
{
    public class MemesController : AdminController
    {
        public IActionResult Index() => View();
    }
}
