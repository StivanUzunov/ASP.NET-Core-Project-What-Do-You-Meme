using System.ComponentModel.DataAnnotations;
using static WhatDoYouMeme.Data.DataConstants;

namespace WhatDoYouMeme.Models.Videos
{
    public class EditVideoFormModel
    {
        [Required]
        [Url]
        [StringLength(VideoURLMaxLength)]
        public string VideoUrl { get; init; }
        [Required]
        [StringLength(VideoTitleMaxLength, MinimumLength = VideoTitleMinLength)]
        public string Title { get; init; }
        [Required]
        [StringLength(VideoDescriptionMaxLength, MinimumLength = VideoDescriptionMinLength)]
        public string Description { get; init; }
        [Required]
        public int MemerId { get; init; }
    }
}
