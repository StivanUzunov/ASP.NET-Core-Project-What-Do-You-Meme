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
        => this.data
            .Issues
            .Count(i => i.IsSolved);

        public void DeleteIssues()
        {
            var issuesForDeleting = this.data
                .Issues
                .Where(i => i.IsSolved).ToList();

            this.data.Issues.RemoveRange(issuesForDeleting);
            this.data.SaveChanges();
        }

        public void IsReviewed(int id)
        {
            var issueData = this.data.Issues.Where(i => i.Id == id).First();

            issueData.IsSolved = true;

            this.data.SaveChanges();
        }

        public void Log(AddIssueFormModel issue,int memerId,string email)
        {
            var issueData = new Data.Models.Issues()
            {
                Title = issue.Title,
                Date = DateTime.Now.ToString(CultureInfo.CurrentCulture),
                Description = issue.Description,
                IsSolved = false,
                MemerId = memerId,
                UserEmail = email
            };

            this.data.Issues.Add(issueData);
            this.data.SaveChanges();

        }

        public List<IssueListingViewModel> AllIssues()
        {
            var issues = this.data
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
