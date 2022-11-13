using System;
using System.Globalization;
using System.Security.Claims;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WhatDoYouMeme.Data;
using WhatDoYouMeme.Data.Models;
using WhatDoYouMeme.Models.Comments;
using WhatDoYouMeme.Services;
using static WhatDoYouMeme.WebConstants;

namespace WhatDoYouMeme.Controllers
{
    public class CommentsController:Controller
    {
        private readonly ApplicationDbContext data;
        private readonly IMemerService memers;

        public CommentsController(ApplicationDbContext data, IMemerService memers)
        {
            this.data = data;
            this.memers = memers;
        }


        [Authorize]
        public IActionResult Add()
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;


            if (!this.memers.IsMemer(userId))
            {

                return RedirectToAction(nameof(MemersController.Create), "Memers");
            };


            return View(new AddCommentFormModel());
        }


        [HttpPost]
        [Authorize]
        public IActionResult Add(int id,AddCommentFormModel comment)
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var memerId = this.data.Memers.Where(m => m.UserId == userId).Select(m => m.Id).FirstOrDefault();
            var memerName = this.data.Memers.Where(m => m.UserId == userId).Select(n => n.Name).FirstOrDefault();

            if (memerId == 0)
            {

                return RedirectToAction(nameof(MemersController.Create), "Memers");
            }

            if (!ModelState.IsValid)
            {
                return View(comment);
            }

            var commentData = new Comment
            {
                CommentText = comment.CommentText,
                Likes = 0,
                Date = DateTime.Now.ToString(CultureInfo.CurrentCulture),
                MemerId = memerId,
                PostId = id,
                MemerName = memerName
            };

            this.data.Comments.Add(commentData);
            this.data.SaveChanges();

            TempData[GlobalMessageKey] = "Your comment was added successfully!";

            return RedirectToAction(nameof(MemesController.Details), "Memes", new {id});
        }

        public IActionResult Delete(int id)
        {
            var comment = this.data.Comments.Where(m => m.Id == id).First();

            var postId = this.data.Posts.Where(p => p.Comments.Contains(comment)).Select(i => i.Id).First();

            this.data.Remove(comment);

            this.data.SaveChanges();

            TempData[GlobalMessageKey] = "The comment was deleted successfully!";

            return RedirectToAction(nameof(MemesController.Details), "Memes", new {id=postId});
        }

        [Authorize]
        public IActionResult Like(int id)
        {
            var comment = this.data.Comments.Where(m => m.Id == id).First();

            var postId = this.data.Posts.Where(p => p.Comments.Contains(comment)).Select(i => i.Id).First();

            comment.Likes++;

            this.data.SaveChanges();

            return RedirectToAction(nameof(MemesController.Details), "Memes", new { id = postId });
        }
    }
}
