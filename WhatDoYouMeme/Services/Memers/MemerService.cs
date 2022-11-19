using System.Linq;
using WhatDoYouMeme.Data;
using WhatDoYouMeme.Data.Models;
using WhatDoYouMeme.Models.Memers;
using WhatDoYouMeme.Services.Memers;

namespace WhatDoYouMeme.Services.Memers
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

        public int GetMemerId(string userId)
        => this.data
            .Memers
            .Where(m => m.UserId == userId)
            .Select(m => m.Id)
            .FirstOrDefault();

        public string MemerName(string userId)
        => this.data.Memers.Where(m => m.UserId == userId).Select(n => n.Name).FirstOrDefault();

        public bool UserIsAlreadyAMemer(string userId)
        => this.data
            .Memers
            .Any(m => m.UserId == userId);

        public void CreateMemer(BecomeMemerFormModel memer,string userId)
        {

            var memerData = new Memer
            {
                Name = memer.Name,
                PhoneNumber = memer.PhoneNumber,
                UserId = userId
            };

            this.data.Memers.Add(memerData);
            this.data.SaveChanges();
        }
    }
}
