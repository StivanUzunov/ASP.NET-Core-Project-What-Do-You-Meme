using WhatDoYouMeme.Models.Memers;

namespace WhatDoYouMeme.Services.Memers
{
    public interface IMemerService
    {
        public bool IsMemer(string userId);
        public int GetMemerId(string userId);
        public string MemerName(string userId);
        public bool UserIsAlreadyAMemer(string userId);
        public void CreateMemer(BecomeMemerFormModel memer, string userId);
    }
}
