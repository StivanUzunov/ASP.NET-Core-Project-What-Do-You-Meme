using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static WhatDoYouMeme.Data.DataConstants;
namespace WhatDoYouMeme.Data.Models
{
    public class Post
    {
        public int Id { get; init; }

        [Required]
        public string ImageUrl { get; set; }
        [Required]
        public int Date { get; set; }
        [Required]
        [MaxLength(PostDescriptionMaxLength)]
        public string Description { get; set; }
        [Required]
        public int Likes { get; set; }

        public IEnumerable<Comment> Comments { get; set; } = new List<Comment>();
    }
}
