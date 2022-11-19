using System.ComponentModel.DataAnnotations;
using static WhatDoYouMeme.Data.DataConstants;

namespace WhatDoYouMeme.Models.Memes
{
    public class AddMemeFormModel
    {
        [Required]
        [Url]
        [StringLength(ImgURLMaxLength)]
        public string ImageUrl { get; init; }
        public string Date { get; init; }
        [Required]
        [StringLength(PostDescriptionMaxLength, MinimumLength = PostDescriptionMinLength)]
        public string Description { get; init; }
        [Required]
        public int MemerId { get; init; }
    }
}
