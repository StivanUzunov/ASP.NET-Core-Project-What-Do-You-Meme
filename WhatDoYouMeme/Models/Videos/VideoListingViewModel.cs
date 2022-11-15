using System.Collections.Generic;
using WhatDoYouMeme.Data.Models;

namespace WhatDoYouMeme.Models.Videos
{
    public class VideoListingViewModel
    {
        public int Id { get; init; }
        public string Date { get; init; }
        public string VideoUrl { get; init; }
        public string Title { get; init; }
        public string Description { get; init; }
        public int Likes { get; init; }
        public int MemerId { get; set; }
        public string MemerName { get; set; }
        public List<Comment> Comments { get; init; }
    }
}
