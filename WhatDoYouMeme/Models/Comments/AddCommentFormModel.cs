using System.ComponentModel.DataAnnotations;
using WhatDoYouMeme.Data.Models;
using static WhatDoYouMeme.Data.DataConstants;

namespace WhatDoYouMeme.Models.Comments
{
    public class AddCommentFormModel
    {
        [Required]
        [MaxLength(CommentMaxLength)]
        public string CommentText { get; set; }
        public Post Post { get; set; }
        public int MemerId { get; set; }
        public Memer Memer { get; set; }

    }
}
