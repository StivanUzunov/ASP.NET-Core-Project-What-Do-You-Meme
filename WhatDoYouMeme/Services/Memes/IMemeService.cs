using System.Collections.Generic;
using WhatDoYouMeme.Data.Models;
using WhatDoYouMeme.Models.Memes;

namespace WhatDoYouMeme.Services.Memes
{
    public interface IMemeService
    {
        public List<MemeListingViewModel> Mine(string userId);
        public List<MemeListingViewModel> All();
        public List<MemeListingViewModel> AdminsAll();
        public MemeListingViewModel Details(int id);
        public void Add(AddMemeFormModel meme,int memerId);
        public EditMemeFormModel Edit(int id);
        public Post GetMemeData(int id);
        public void EditMemeData(Post memeData, EditMemeFormModel meme);
        public void MakePublic(int id);
        public void Delete(int id);
        public void Like(int id);
    }
}
