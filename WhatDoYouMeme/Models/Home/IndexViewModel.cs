using System.Collections.Generic;
using WhatDoYouMeme.Models.Memes;

namespace WhatDoYouMeme.Models.Home
{
    public class IndexViewModel
    {
        public int TotalMemes { get; init; }

        public int TotalUsers { get; init; }

        public List<MemeListingViewModel> Posts { get; set; }

    }
}
