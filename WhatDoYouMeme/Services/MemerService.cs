using System.Linq;
using WhatDoYouMeme.Data;

namespace WhatDoYouMeme.Services
{
    public class MemerService:IMemerService
    {
        private readonly ApplicationDbContext data;

        public MemerService(ApplicationDbContext data)
            => this.data = data;

        public bool IsMemer(string userId)
            => this.data
                .Memers
                .Any(m => m.UserId == userId);
    }
}
