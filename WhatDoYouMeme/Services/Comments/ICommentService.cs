using WhatDoYouMeme.Models.Comments;

namespace WhatDoYouMeme.Services.Comments
{
  public  interface ICommentService
  {
      public void AddComment(int id, AddCommentFormModel comment, int memerId, string memerName);
      public void AddCommentToVideo(int id, AddCommentFormModelVideo comment, int memerId, string memerName);
      public void DeleteComment(int id);
      public void DeleteCommentFromVideo(int id);
      public int GetPostId(int id);
      public int GetVideoId(int id);
      public void Like(int id);
      public void LikeVideo(int id);
    }
}
