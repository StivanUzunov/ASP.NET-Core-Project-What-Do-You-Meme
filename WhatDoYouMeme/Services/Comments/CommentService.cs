using System;
using System.Globalization;
using System.Linq;
using WhatDoYouMeme.Data;
using WhatDoYouMeme.Data.Models;
using WhatDoYouMeme.Models.Comments;

namespace WhatDoYouMeme.Services.Comments
{
    public class CommentService:ICommentService
    {
        private readonly ApplicationDbContext data;
        
        public CommentService(ApplicationDbContext data)
        => this.data = data;

        public void AddComment(int id, AddCommentFormModel comment, int memerId, string memerName)
        {
            var commentData = new Comment
            {
                CommentText = comment.CommentText,
                Likes = 0,
                Date = DateTime.Now.ToString(CultureInfo.CurrentCulture),
                MemerId = memerId,
                PostId = id,
                MemerName = memerName
            };

            data.Comments.Add(commentData);
            data.SaveChanges();
        }

        public void DeleteComment(int id)
        {
            var comment = data.Comments
                .First(m => m.Id == id);

            data.Remove(comment);

            data.SaveChanges();
        }

        public int GetPostId(int id)
        {
            var comment = data.Comments
                .First(m => m.Id == id);

            var postId = data.Posts.Where(p => p.Comments.Contains(comment)).Select(i => i.Id).First();

            return postId;
        }

        public void Like(int id)
        {
            var comment = data.Comments
                .First(m => m.Id == id);

            comment.Likes++;

            data.SaveChanges();
        }

        public void AddCommentToVideo(int id, AddCommentFormModelVideo comment, int memerId, string memerName)
        {
            var commentData = new Comment
            {
                CommentText = comment.CommentText,
                Likes = 0,
                Date = DateTime.Now.ToString(CultureInfo.CurrentCulture),
                MemerId = memerId,
                VideoId = id,
                MemerName = memerName
            };

            data.Comments.Add(commentData);

            data.SaveChanges();
        }

        public void DeleteCommentFromVideo(int id)
        {
            var comment = data.Comments
                .First(m => m.Id == id);

            data.Remove(comment);

            data.SaveChanges();
        }

        public int GetVideoId(int id)
        {
            var comment = data.Comments
                .First(m => m.Id == id);

            var videoId = data.Videos.Where(p => p.Comments
                    .Contains(comment))
                .Select(i => i.Id)
                .First();

            return videoId;
        }

        public void LikeVideo(int id)
        {
            var comment = data.Comments
                .First(m => m.Id == id);

            comment.Likes++;

            data.SaveChanges();
        }
    }
}
