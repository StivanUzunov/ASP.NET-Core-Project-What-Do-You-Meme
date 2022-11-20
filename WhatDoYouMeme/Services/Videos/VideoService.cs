using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using WhatDoYouMeme.Data;
using WhatDoYouMeme.Data.Models;
using WhatDoYouMeme.Models.Videos;

namespace WhatDoYouMeme.Services.Videos
{
    public class VideoService:IVideoService
    {
        private readonly ApplicationDbContext data;

        public VideoService(ApplicationDbContext data)
            => this.data = data;

        public void Add(AddVideoFormModel video, int memerId)
        {
            var videoData = new Video
            {
                VideoUrl = video.VideoUrl,
                Title = video.Title,
                Description = video.Description,
                Likes = 0,
                Date = DateTime.Now.ToString(CultureInfo.CurrentCulture),
                Comments = new List<Comment>(),
                MemerId = memerId,
                IsPublic = false,
            };

            data.Videos.Add(videoData);

            data.SaveChanges();
        }

        public VideoListingViewModel Details(int id)
        => data.
            Videos.
            Where(v => v.Id == id)
            .Select(v => new VideoListingViewModel
            {
                Id = v.Id,
                VideoUrl = v.VideoUrl,
                Title = v.Title,
                Description = v.Description,
                Date = v.Date,
                Likes = v.Likes,
                MemerId = v.MemerId,
                MemerName = v.Memer.Name,
                Comments = v.Comments.OrderByDescending(l => l.Likes).ToList()
            }).FirstOrDefault();

        public List<VideoListingViewModel> All()
        => data
            .Videos
            .Where(v => v.IsPublic)
            .OrderByDescending(m => m.Id)
            .Select(v => new VideoListingViewModel
            {
                Id = v.Id,
                VideoUrl = v.VideoUrl,
                Title = v.Title,
                Description = v.Description,
                Date = v.Date,
                Likes = v.Likes,
                MemerId = v.MemerId,
                MemerName = v.Memer.Name,
                Comments = v.Comments.OrderByDescending(c => c.Likes).Take(3).ToList(),
            })
            .ToList();

        public List<VideoListingViewModel> AdminsAll()
        => data
            .Videos
            .Where(v => v.IsPublic == false)
        .OrderBy(v => v.Id)
            .Select(v => new VideoListingViewModel
        {
            Id = v.Id,
            VideoUrl = v.VideoUrl,
            Title = v.Title,
            Description = v.Description,
            Date = v.Date,
            Likes = v.Likes,
            MemerId = v.MemerId,
            MemerName = v.Memer.Name,
            Comments = v.Comments.ToList()
        })
        .ToList();

        public void Like(int id)
        {
            var video = data
                .Videos
                .First(m => m.Id == id);

            video.Likes++;

            data.SaveChanges();
        }

        public void Delete(int id)
        {
            var video = data
                .Videos
                .First(m => m.Id == id);

            data.Remove(video);

            data.SaveChanges();
        }

        public EditVideoFormModel Edit(int id)
        => data
            .Videos
            .Where(v => v.Id == id)
            .Select(v => new EditVideoFormModel
            {
                VideoUrl = v.VideoUrl,
                Title = v.Title,
                Description = v.Description,
                MemerId = v.MemerId
            })
            .FirstOrDefault();

        public void EditVideo(Video videoData, EditVideoFormModel video)
        {
            videoData.VideoUrl = video.VideoUrl;
            videoData.Title = video.Title;
            videoData.Description = video.Description;
            videoData.IsPublic = false;

            data.SaveChanges();
        }

        public Video GetVideo(int id)
        => data.Videos
            .First(m => m.Id == id);

        public List<VideoListingViewModel> Mine(string userId)
       => data
           .Videos
           .Where(v => v.Memer.UserId == userId)
           .OrderByDescending(v => v.Id)
           .Select(video => new VideoListingViewModel
           {
               Id = video.Id,
               VideoUrl = video.VideoUrl,
               Title = video.Title,
               Description = video.Description,
               Date = video.Date,
               Likes = video.Likes,
               MemerId = video.MemerId,
               MemerName = video.Memer.Name,
               Comments = video.Comments.ToList()
           })
           .ToList();

        public void MakePublic(int id)
        {
            var memeData = data.Videos
                .First(m => m.Id == id);

            memeData.IsPublic = true;

            data.SaveChanges();
        }
    }
}
