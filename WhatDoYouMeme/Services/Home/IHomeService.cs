using System.Collections.Generic;
using WhatDoYouMeme.Models.Memes;

namespace WhatDoYouMeme.Services.Home
{
   public interface IHomeService
    {
        public List<MemeListingViewModel> GetLatestMemes();
        public int GetTotalMemes();
        public int GetTotalUsers();
        public int GetTotalVideos();
    }
}
