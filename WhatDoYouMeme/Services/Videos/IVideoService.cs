using System.Collections.Generic;
using WhatDoYouMeme.Data.Models;
using WhatDoYouMeme.Models.Videos;

namespace WhatDoYouMeme.Services.Videos
{
    public interface IVideoService
    {
        public void Add(AddVideoFormModel video,int memerId);
        public VideoListingViewModel Details(int id);
        public List<VideoListingViewModel> All();
        public List<VideoListingViewModel> AdminsAll();
        public void Like(int id);
        public void Delete(int id);
        public EditVideoFormModel Edit(int id);
        public void EditVideo(Video videoData, EditVideoFormModel video);
        public Video GetVideo(int id);
        public List<VideoListingViewModel> Mine(string userId);
        public void MakePublic(int id);
    }
}
