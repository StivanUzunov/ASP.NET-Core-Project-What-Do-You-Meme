using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.IIS.Core;
using WhatDoYouMeme.Areas.Admin.Models;
using WhatDoYouMeme.Data;
using static WhatDoYouMeme.WebConstants;

namespace WhatDoYouMeme.Areas.Admin.Controllers
{
    public class IssuesController : AdminController
    {
        private readonly ApplicationDbContext data;

        public IssuesController(ApplicationDbContext data)
         => this.data = data;

        [Authorize]
        public IActionResult All()
        {
            var reviewedIssues = this.data
                .Issues
                .Count(i => i.IsSolved);

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

            return View(new IssuesViewModel
            {
                Issues = issues,
                IssuesReviewed = reviewedIssues
            });
        }
        [Authorize]
        public IActionResult Delete()
        {
            var issuesForDeleting = this.data
                .Issues
                .Where(i => i.IsSolved).ToList();

            this.data.Issues.RemoveRange(issuesForDeleting);
            this.data.SaveChanges();

            TempData[GlobalMessageKey] = "All reviewed issues have been deleted!";

            return RedirectToAction(nameof(All));
        }

    }
}
