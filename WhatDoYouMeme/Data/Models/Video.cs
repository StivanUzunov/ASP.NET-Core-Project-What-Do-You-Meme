using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static WhatDoYouMeme.Data.DataConstants;
namespace WhatDoYouMeme.Data.Models
{
    public class Video
    {
        public int Id { get; init; }
        [Required]
        [Url]
        [MaxLength(VideoURLMaxLength)]
        public string VideoUrl { get; set; }
        [Required]
        public string Date { get; set; }
        [Required]
        [MaxLength(VideoTitleMaxLength)]
        [MinLength(VideoTitleMinLength)]
        public string Title { get; set; }
        [Required]
        [MaxLength(VideoDescriptionMaxLength)]
        [MinLength(VideoDescriptionMinLength)]
        public string Description { get; set; }
        [Required]
        public int Likes { get; set; }
        public bool IsPublic { get; set; }
        public int MemerId { get; init; }
        public Memer Memer { get; init; }
        public IEnumerable<Comment> Comments { get; set; } = new List<Comment>();
    }
}
