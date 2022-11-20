using System.Collections.Generic;
using System.Linq;
using WhatDoYouMeme.Data;
using WhatDoYouMeme.Models.Memes;

namespace WhatDoYouMeme.Services.Home
{
    public class HomeService:IHomeService
    {
        private readonly ApplicationDbContext data;

        public HomeService(ApplicationDbContext data)
            => this.data = data;
        
        public List<MemeListingViewModel> GetLatestMemes()
       => data
           .Posts
           .Where(m => m.isPublic)
           .OrderByDescending(m => m.Id)
           .Select(m => new MemeListingViewModel
           {
               Id = m.Id,
               ImageUrl = m.ImageUrl,
               Description = m.Description,
               Date = m.Date,
               Likes = m.Likes
           })
           .Take(3)
           .ToList();

        public int GetTotalMemes()
        => data
            .Posts
            .Count(m => m.isPublic);

        public int GetTotalUsers()
        => data
            .Users
            .Count();

        public int GetTotalVideos()
        => data
            .Videos
            .Count(m => m.IsPublic);
    }
}
