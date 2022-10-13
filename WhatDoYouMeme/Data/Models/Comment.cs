using System.ComponentModel.DataAnnotations;
using static WhatDoYouMeme.Data.DataConstants;
namespace WhatDoYouMeme.Data.Models
{
    public class Comment
    {

        public int Id { get; init; }

        [Required]
        [MaxLength(CommentMaxLength)]
        public string CommentText { get; set; }

        [Required]
        public int Likes { get; set; }

    }
}
