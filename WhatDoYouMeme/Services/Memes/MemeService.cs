using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using WhatDoYouMeme.Data;
using WhatDoYouMeme.Data.Models;
using WhatDoYouMeme.Models.Memes;

namespace WhatDoYouMeme.Services.Memes
{
    public class MemeService:IMemeService
    {
        private readonly ApplicationDbContext data;

        public MemeService(ApplicationDbContext data)
            => this.data = data;

        public List<MemeListingViewModel> Mine(string userId)
       => data
           .Posts
           .Where(m => m.Memer.UserId == userId)
           .OrderByDescending(m => m.Id)
           .Select(m => new MemeListingViewModel
           {
               Id = m.Id,
               ImageUrl = m.ImageUrl,
               Description = m.Description,
               Date = m.Date,
               Likes = m.Likes,
               MemerId = m.MemerId,
               MemerName = m.Memer.Name,
               Comments = m.Comments.ToList()
           })
           .ToList();

        public List<MemeListingViewModel> All()
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
                Likes = m.Likes,
                MemerId = m.MemerId,
                MemerName = m.Memer.Name,
                Comments = m.Comments.OrderByDescending(c => c.Likes).Take(3).ToList(),
            })
            .ToList();

        public List<MemeListingViewModel> AdminsAll()
       => data
           .Posts
           .Where(m => m.isPublic == false)
           .OrderBy(m => m.Id)
           .Select(m => new MemeListingViewModel
           {
               Id = m.Id,
               ImageUrl = m.ImageUrl,
               Description = m.Description,
               Date = m.Date,
               Likes = m.Likes,
               MemerId = m.MemerId,
               MemerName = m.Memer.Name,
               Comments = m.Comments.ToList()
           })
           .ToList();

        public MemeListingViewModel Details(int id)
        => data.
            Posts.
            Where(m => m.Id == id)
            .Select(m => new MemeListingViewModel
            {
                Id = m.Id,
                ImageUrl = m.ImageUrl,
                Description = m.Description,
                Date = m.Date,
                Likes = m.Likes,
                MemerId = m.MemerId,
                MemerName = m.Memer.Name,
                Comments = m.Comments.OrderByDescending(l => l.Likes).ToList()
            }).FirstOrDefault();

        public void Add(AddMemeFormModel meme, int memerId)
        {
            var memeData = new Post
            {
                ImageUrl = meme.ImageUrl,
                Description = meme.Description,
                Likes = 0,
                Date = DateTime.Now.ToString(CultureInfo.CurrentCulture),
                Comments = new List<Comment>(),
                MemerId = memerId,
                isPublic = false,

            };

            data.Posts.Add(memeData);

            data.SaveChanges();
        }

        public EditMemeFormModel Edit(int id)
        => data
            .Posts
            .Where(m => m.Id == id)
            .Select(m => new EditMemeFormModel
            {
                ImageUrl = m.ImageUrl,
                Description = m.Description,
                MemerId = m.MemerId
            })
            .FirstOrDefault();

        public Post GetMemeData(int id)
        => data
            .Posts
            .FirstOrDefault(m => m.Id == id);

        public void EditMemeData(Post memeData, EditMemeFormModel meme)
        {
            memeData.ImageUrl = meme.ImageUrl;

            memeData.Description = meme.Description;

            memeData.isPublic = false;

            data.SaveChanges();
        }

        public void MakePublic(int id)
        {
            var memeData = data
                .Posts
                .FirstOrDefault(m => m.Id == id);

            memeData.isPublic = true;

            data.SaveChanges();
        }

        public void Delete(int id)
        {
            var meme = data
                .Posts
                .FirstOrDefault(m => m.Id == id);

            data.Remove(meme);

            data.SaveChanges();
        }

        public void Like(int id)
        {
            var meme = data
                .Posts
                .FirstOrDefault(m => m.Id == id);

            meme.Likes++;

            data.SaveChanges();
        }
    }
}
