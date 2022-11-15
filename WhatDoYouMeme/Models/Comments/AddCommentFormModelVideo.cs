using System.ComponentModel.DataAnnotations;
using WhatDoYouMeme.Data.Models;
using static WhatDoYouMeme.Data.DataConstants;

namespace WhatDoYouMeme.Models.Comments
{
    public class AddCommentFormModelVideo
    {
        [Required]
        [MaxLength(CommentMaxLength)]
        public string CommentText { get; set; }

        public int MemerId { get; set; }

        public Memer Memer { get; set; }

        public int VideoId { get; set; }

        public Video Video { get; set; }
    }
}
