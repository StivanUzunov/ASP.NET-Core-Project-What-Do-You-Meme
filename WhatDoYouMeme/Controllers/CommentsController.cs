using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WhatDoYouMeme.Infrastructure;
using WhatDoYouMeme.Models.Comments;
using WhatDoYouMeme.Services.Comments;
using WhatDoYouMeme.Services.Memers;
using static WhatDoYouMeme.WebConstants;

namespace WhatDoYouMeme.Controllers
{
    public class CommentsController:Controller
    {
        private readonly IMemerService memers;
        private readonly ICommentService comments; 
        public CommentsController(IMemerService memers,ICommentService comments)
        {
            this.memers = memers;
            this.comments = comments;
        }

        [Authorize]
        public IActionResult Add()
        {
            var userId = User.GetUserId();

            if (!memers.IsMemer(userId))
            {
                return RedirectToAction(nameof(MemersController.Create), "Memers");
            };

            return View(new AddCommentFormModel());
        }

        [HttpPost]
        [Authorize]
        public IActionResult Add(int id,AddCommentFormModel comment)
        {
            var userId = User.GetUserId();
            var memerId = memers.GetMemerId(userId);
            var memerName = memers.MemerName(userId);

            if (memerId == 0)
            {

                return RedirectToAction(nameof(MemersController.Create), "Memers");
            }

            if (!ModelState.IsValid)
            {
                return View(comment);
            }

            comments.AddComment(id,comment,memerId,memerName);

            TempData[GlobalMessageKey] = "Your comment was added successfully!";

            return RedirectToAction(nameof(MemesController.Details), "Memes", new {id});
        }
        [Authorize]
        public IActionResult Delete(int id)
        {
            var postId = comments.GetPostId(id);

            comments.DeleteComment(id);

            TempData[GlobalMessageKey] = "The comment was deleted successfully!";

            return RedirectToAction(nameof(MemesController.Details), "Memes", new {id=postId});
        }

        [Authorize]
        public IActionResult Like(int id)
        {
            var userId = User.GetUserId();

            if (!memers.IsMemer(userId))
            {
                return RedirectToAction(nameof(MemersController.Create), "Memers");
            };

            var postId = comments.GetPostId(id);

            comments.Like(id);

            return RedirectToAction(nameof(MemesController.Details), "Memes", new { id = postId });
        }


        [Authorize]
        public IActionResult AddToVideo()
        {
            var userId = User.GetUserId();

            if (!memers.IsMemer(userId))
            {
                return RedirectToAction(nameof(MemersController.Create), "Memers");
            };

            return View(new AddCommentFormModelVideo());
        }

        [HttpPost]
        [Authorize]
        public IActionResult AddToVideo(int id, AddCommentFormModelVideo comment)
        {
            var userId = User.GetUserId();
            var memerId = memers.GetMemerId(userId);
            var memerName = memers.MemerName(userId);

            if (memerId == 0)
            {
                return RedirectToAction(nameof(MemersController.Create), "Memers");
            }

            if (!ModelState.IsValid)
            {
                return View(comment);
            }

            comments.AddCommentToVideo(id,comment,memerId,memerName);

            TempData[GlobalMessageKey] = "Your comment was added successfully!";

            return RedirectToAction(nameof(VideosController.Details), "Videos", new { id });
        }

        public IActionResult DeleteOfVideo(int id)
        {
           var videoId=  comments.GetVideoId(id);

            comments.DeleteCommentFromVideo(id);

            TempData[GlobalMessageKey] = "The comment was deleted successfully!";

            return RedirectToAction(nameof(VideosController.Details), "Videos", new { id = videoId });
        }

        [Authorize]
        public IActionResult LikeofVideo(int id)
        {
            var userId = User.GetUserId();

            if (!memers.IsMemer(userId))
            {
                return RedirectToAction(nameof(MemersController.Create), "Memers");
            };
            comments.LikeVideo(id);

            var videoId = comments.GetVideoId(id);

            return RedirectToAction(nameof(VideosController.Details), "Videos", new { id = videoId });
        }
    }
}
