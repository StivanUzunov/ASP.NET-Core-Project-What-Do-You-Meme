using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WhatDoYouMeme.Infrastructure;
using WhatDoYouMeme.Models.Memes;
using WhatDoYouMeme.Services.Memers;
using WhatDoYouMeme.Services.Memes;
using static WhatDoYouMeme.WebConstants;

namespace WhatDoYouMeme.Controllers
{
    public class MemesController : Controller
    {
        private readonly IMemerService memers;
        private readonly IMemeService memes;
        public MemesController(IMemerService memers, IMemeService memes)
        {
            this.memers = memers;
            this.memes = memes;
        }
        public IActionResult All()
        {
            var allMemes = this.memes.All();

            return View(allMemes);
        }

        [Authorize]
        public IActionResult Mine()
        {
            var userId = User.GetUserId();

            var myMemes = memes.Mine(userId);

            return View(myMemes);
        }

        public IActionResult Details(int id)
        {
            var memeData = memes.Details(id);

            return View(memeData);
        }
        
        [Authorize]
        public IActionResult Add()
        {
            var userId = User.GetUserId();

            if (!memers.IsMemer(userId))
            {
                return RedirectToAction(nameof(MemersController.Create), "Memers");
            };

            return View(new AddMemeFormModel());
        }

        [HttpPost]
        [Authorize]
        public IActionResult Add(AddMemeFormModel meme)
        {
            var userId = User.GetUserId();
            var memerId = memers.GetMemerId(userId);

            if (memerId == 0)
            {
                return RedirectToAction(nameof(MemersController.Create), "Memers");
            }

            if (!ModelState.IsValid)
            {
                return View(meme);
            }

            memes.Add(meme,memerId);

            TempData[GlobalMessageKey] = "Your Meme was added successfully and it is waiting for approval!";

            return RedirectToAction(nameof(All));
        }

        [Authorize]
        public IActionResult Edit(int id)
        {
            var userId = User.GetUserId();
            var memerId = memers.GetMemerId(userId);

            if (!memers.IsMemer(userId) && !User.IsAdmin())
            {
                return RedirectToAction(nameof(MemersController.Create), "Memers");
            }

            var memeData = memes.GetMemeData(id);

            if (memeData == null)
            {
                return BadRequest();
            }

            if (memeData.MemerId != memerId && !User.IsAdmin())
            {
                return Unauthorized();
            }

           var meme = memes.Edit(id);

            return View(new EditMemeFormModel
            {
                ImageUrl = meme.ImageUrl,
                Description = meme.Description,
                MemerId = meme.MemerId
            });
        }

        [HttpPost]
        [Authorize]
        public IActionResult Edit(int id, EditMemeFormModel meme)
        {
            var userId = User.GetUserId();
            var memerId = memers.GetMemerId(userId);

            var memeData = memes.GetMemeData(id);

            if (memerId == 0 && !User.IsAdmin())
            {
                return RedirectToAction(nameof(MemersController.Create), "Memers");
            }

            if (!ModelState.IsValid)
            {
                return View(meme);
            }

            if (memeData.MemerId != memerId && !User.IsAdmin())
            {
                return BadRequest();
            }

            memes.EditMemeData(memeData,meme);

            TempData[GlobalMessageKey] = "Your Meme was edited successfully and it is waiting for approval!";

            return RedirectToAction(nameof(All));

        }

        [Authorize]
        public IActionResult Delete(int id)
        { 
            var userId = User.GetUserId();
            var memerId = memers.GetMemerId(userId);

            var memeData = memes.GetMemeData(id);

            if (memerId != memeData.MemerId&& !User.IsAdmin())
            {
                return BadRequest();
            }

            memes.Delete(id);

            return RedirectToAction(nameof(All));
        }

        [Authorize]
        public IActionResult Like(int id)
        {
            var userId = User.GetUserId();

            if (!memers.IsMemer(userId))
            {

                return RedirectToAction(nameof(MemersController.Create), "Memers");
            };

            memes.Like(id);

            return RedirectToAction(nameof(All), "Memes", $"{id}");
        }
    }
}
