using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WhatDoYouMeme.Infrastructure;
using WhatDoYouMeme.Models.Issues;
using WhatDoYouMeme.Services.Issues;
using WhatDoYouMeme.Services.Memers;
using static WhatDoYouMeme.WebConstants;

namespace WhatDoYouMeme.Controllers
{
    public class IssuesController : Controller
    {
        private readonly IMemerService memers;
        private readonly IIssueService issues;
        public IssuesController(IMemerService memers, IIssueService issues)
        {
            this.memers = memers;
            this.issues = issues;
        }

        public IActionResult Log()
        {
            var userId = User.GerUserId();


            if (!memers.IsMemer(userId))
            {

                return RedirectToAction(nameof(MemersController.Create), "Memers");
            };


            return View(new AddIssueFormModel());
        }


        [HttpPost]
        [Authorize]
        public IActionResult Log(AddIssueFormModel issue)
        {
            var userId = User.GerUserId();
            var memerId = memers.GetMemerId(userId);
            var email = User.GerUserEmail();

            if (memerId == 0)
            {
                return RedirectToAction(nameof(MemersController.Create), "Memers");
            }

            if (!ModelState.IsValid)
            {
                return View(issue);
            }

            issues.Log(issue,memerId,email);

            TempData[GlobalMessageKey] = "Your issue was logged successfully and our admins will review it and you write back to you by email! Thank you! :)";

            return RedirectToAction(nameof(MemesController.All), "Memes");
        }

        public IActionResult IsReviewed(int id)
        {
           issues.IsReviewed(id);

            TempData[GlobalMessageKey] = "This issue was reviewed successfully!";

            var url = Url.RouteUrl("areas", new { controller = "Issues", action = "All", area = "Admin" });
            return Redirect(url);
        }

    }
}
