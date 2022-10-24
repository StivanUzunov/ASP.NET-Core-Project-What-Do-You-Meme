using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static WhatDoYouMeme.Data.DataConstants;
namespace WhatDoYouMeme.Data.Models
{
    public class Post
    {
        public int Id { get; init; }

        [Required]
        [MaxLength(ImgURLMaxLength)]
        public string ImageUrl { get; set; }
        [Required]
        public string Date { get; set; }
        [Required]
        [MaxLength(PostDescriptionMaxLength)]
        [MinLength(PostDescriptionMinLength)]
        public string Description { get; set; }
        [Required]
        public int Likes { get; set; }
        public int MemerId { get; init; }
        public Memer Memer { get; init; }
        public IEnumerable<Comment> Comments { get; set; } = new List<Comment>();
    }
}
