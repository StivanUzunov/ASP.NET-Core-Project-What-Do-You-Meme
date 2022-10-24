using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WhatDoYouMeme.Data.Models;
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
        public int Likes { get; init; }
     
    }
}
