using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WhatDoYouMeme.Areas.Admin.Models;
using WhatDoYouMeme.Services.Issues;
using static WhatDoYouMeme.WebConstants;

namespace WhatDoYouMeme.Areas.Admin.Controllers
{
    public class IssuesController : AdminController
    {
        private readonly IIssueService issues;

        public IssuesController(IIssueService issues)
        =>  this.issues = issues;
        

        [Authorize]
        public IActionResult All()
        {
            var reviewedIssues = this.issues.ReviewedIssues();

            var issues = this.issues.AllIssues();

            return View(new IssuesViewModel
            {
                Issues = issues,
                IssuesReviewed = reviewedIssues
            });
        }
        [Authorize]
        public IActionResult Delete()
        {
            this.issues.DeleteIssues();

            TempData[GlobalMessageKey] = "All reviewed issues have been deleted!";

            return RedirectToAction(nameof(All));
        }

    }
}
