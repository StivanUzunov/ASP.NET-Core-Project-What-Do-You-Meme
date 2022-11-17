using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WhatDoYouMeme.Data;
using WhatDoYouMeme.Data.Models;
using WhatDoYouMeme.Models.Issues;
using WhatDoYouMeme.Models.Memes;
using WhatDoYouMeme.Services;
using static WhatDoYouMeme.WebConstants;

namespace WhatDoYouMeme.Controllers
{
    public class IssuesController : Controller
    {
        private readonly ApplicationDbContext data;
        private readonly IMemerService memers;

        public IssuesController(ApplicationDbContext data, IMemerService memers)
        {
            this.data = data;
            this.memers = memers;
        }

        public IActionResult Log()
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;


            if (!this.memers.IsMemer(userId))
            {

                return RedirectToAction(nameof(MemersController.Create), "Memers");
            };


            return View(new AddIssueFormModel());
        }


        [HttpPost]
        [Authorize]
        public IActionResult Log(AddIssueFormModel issue)
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var memerId = this.data.Memers.Where(m => m.UserId == userId).Select(m => m.Id).FirstOrDefault();
            var email = this.User.FindFirst(ClaimTypes.Email).Value;
            if (memerId == 0)
            {

                return RedirectToAction(nameof(MemersController.Create), "Memers");
            }

            if (!ModelState.IsValid)
            {
                return View(issue);
            }

            var issueData = new Issues
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

            TempData[GlobalMessageKey] = "Your issue was logged successfully and our admins will review it and you write back to you by email! Thank you! :)";

            return RedirectToAction(nameof(MemesController.All), "Memes");
        }

        public IActionResult IsReviewed(int id)
        {
            var issueData = this.data.Issues.Where(i => i.Id == id).First();

            issueData.IsSolved = true;

            this.data.SaveChanges();

            TempData[GlobalMessageKey] = "This issue was reviewed successfully!";

            var url = Url.RouteUrl("areas", new { controller = "Issues", action = "All", area = "Admin" });
            return Redirect(url);
        }

    }
}
