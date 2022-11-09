using System.Linq;
using Microsoft.AspNetCore.Mvc;
using WhatDoYouMeme.Data;
using WhatDoYouMeme.Models.Memes;

namespace WhatDoYouMeme.Areas.Admin.Controllers
{
    public class MemesController : AdminController
    {
        private readonly ApplicationDbContext data;

        public MemesController(ApplicationDbContext data)
        => this.data = data;



        public IActionResult All()
        {
            var memes = this.data
                .Posts
                .Where(m=>m.isPublic==false)
                .OrderBy(m=>m.Id)
                .Select(m => new MemeListingViewModel
                {
                    Id = m.Id,
                    ImageUrl = m.ImageUrl,
                    Description = m.Description,
                    Date = m.Date,
                    Likes = m.Likes,
                    MemerId = m.MemerId,
                    MemerName = m.Memer.Name,
                    Comments = m.Comments.ToList()
                })
                .ToList();

            return View(memes);
        }
    }
}
