using System;
using System.Globalization;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WhatDoYouMeme.Data;
using WhatDoYouMeme.Data.Models;
using WhatDoYouMeme.Infrastructure;
using WhatDoYouMeme.Models.Comments;
using WhatDoYouMeme.Services.Memers;
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
            var userId = this.User.GerUserId();


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
            var userId = this.User.GerUserId();
            var memerId = this.memers.GetMemerId(userId);
            var memerName = this.memers.MemerName(userId);

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
        [Authorize]
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
            var userId = this.User.GerUserId();


            if (!this.memers.IsMemer(userId))
            {

                return RedirectToAction(nameof(MemersController.Create), "Memers");
            };

            var comment = this.data.Comments.Where(m => m.Id == id).First();

            var postId = this.data.Posts.Where(p => p.Comments.Contains(comment)).Select(i => i.Id).First();

            comment.Likes++;

            this.data.SaveChanges();

            return RedirectToAction(nameof(MemesController.Details), "Memes", new { id = postId });
        }


        [Authorize]
        public IActionResult AddToVideo()
        {
            var userId = this.User.GerUserId();


            if (!this.memers.IsMemer(userId))
            {

                return RedirectToAction(nameof(MemersController.Create), "Memers");
            };


            return View(new AddCommentFormModelVideo());
        }

        [HttpPost]
        [Authorize]
        public IActionResult AddToVideo(int id, AddCommentFormModelVideo comment)
        {
            var userId = this.User.GerUserId();
            var memerId = this.memers.GetMemerId(userId);
            var memerName = this.memers.MemerName(userId);

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
                VideoId = id,
                MemerName = memerName
            };

            this.data.Comments.Add(commentData);
            this.data.SaveChanges();

            TempData[GlobalMessageKey] = "Your comment was added successfully!";

            return RedirectToAction(nameof(VideosController.Details), "Videos", new { id });
        }

        public IActionResult DeleteOfVideo(int id)
        {
            var comment = this.data.Comments.Where(m => m.Id == id).First();

            var videoId = this.data.Videos.Where(p => p.Comments.Contains(comment)).Select(i => i.Id).First();

            this.data.Remove(comment);

            this.data.SaveChanges();

            TempData[GlobalMessageKey] = "The comment was deleted successfully!";

            return RedirectToAction(nameof(VideosController.Details), "Videos", new { id = videoId });
        }

        [Authorize]
        public IActionResult LikeofVideo(int id)
        {
            var userId = this.User.GerUserId();


            if (!this.memers.IsMemer(userId))
            {

                return RedirectToAction(nameof(MemersController.Create), "Memers");
            };

            var comment = this.data.Comments.Where(m => m.Id == id).First();

            var videoId = this.data.Videos.Where(p => p.Comments.Contains(comment)).Select(i => i.Id).First();

            comment.Likes++;

            this.data.SaveChanges();

            return RedirectToAction(nameof(VideosController.Details), "Videos", new { id = videoId });
        }
    }
}
