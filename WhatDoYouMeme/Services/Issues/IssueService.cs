using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using WhatDoYouMeme.Areas.Admin.Models;
using WhatDoYouMeme.Data;
using WhatDoYouMeme.Models.Issues;

namespace WhatDoYouMeme.Services.Issues
{
    public class IssueService:IIssueService
    {
        private readonly ApplicationDbContext data;

        public IssueService(ApplicationDbContext data)
            => this.data = data;

        public int ReviewedIssues()
        => data
            .Issues
            .Count(i => i.IsSolved);

        public void DeleteIssues()
        {
            var issuesForDeleting = data
                .Issues
                .Where(i => i.IsSolved).ToList();

            data.Issues.RemoveRange(issuesForDeleting);
            data.SaveChanges();
        }

        public void IsReviewed(int id)
        {
            var issueData = data.Issues
                .First(i => i.Id == id);

            issueData.IsSolved = true;

            data.SaveChanges();
        }

        public void Log(AddIssueFormModel issue,int memerId,string email)
        {
            var issueData = new Data.Models.Issues
            {
                Title = issue.Title,
                Date = DateTime.Now.ToString(CultureInfo.CurrentCulture),
                Description = issue.Description,
                IsSolved = false,
                MemerId = memerId,
                UserEmail = email
            };

            data.Issues.Add(issueData);
            data.SaveChanges();

        }

        public List<IssueListingViewModel> AllIssues()
        {
            var issues = data
                .Issues
                .Where(i => i.IsSolved == false)
                .OrderBy(i => i.Id)
                .Select(i => new IssueListingViewModel
                {
                    Id = i.Id,
                    Title = i.Title,
                    Description = i.Description,
                    Date = i.Date,
                    MemerId = i.MemerId,
                    MemerName = i.Memer.Name,
                    UserEmail = i.UserEmail,
                })
                .ToList();

            return (issues);
        }
    }
}
