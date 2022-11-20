using System.Linq;
using WhatDoYouMeme.Data;
using WhatDoYouMeme.Data.Models;
using WhatDoYouMeme.Models.Memers;

namespace WhatDoYouMeme.Services.Memers
{
    public class MemerService:IMemerService
    {
        private readonly ApplicationDbContext data;

        public MemerService(ApplicationDbContext data)
            => this.data = data;

        public bool IsMemer(string userId)
            => data
                .Memers
                .Any(m => m.UserId == userId);

        public int GetMemerId(string userId)
        => data
            .Memers
            .Where(m => m.UserId == userId)
            .Select(m => m.Id)
            .FirstOrDefault();

        public string MemerName(string userId)
        => data.Memers.Where(m => m.UserId == userId)
            .Select(n => n.Name)
            .FirstOrDefault();

        public bool UserIsAlreadyAMemer(string userId)
        => data
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

            data.Memers.Add(memerData);
            data.SaveChanges();
        }
    }
}
