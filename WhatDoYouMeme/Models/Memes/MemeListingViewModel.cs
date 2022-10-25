using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Design;
using WhatDoYouMeme.Data.Models;

namespace WhatDoYouMeme.Models.Memes
{
    public class MemeListingViewModel
    {

        public int Id { get; init; }
        public string Date { get; init; }
        public string ImageUrl { get; init; }
        public string Description { get; init; }
        public int Likes { get; init; }
        public int MemerId { get; set; }
        public IEnumerable<Comment> Comments { get; init; }

    }
}
