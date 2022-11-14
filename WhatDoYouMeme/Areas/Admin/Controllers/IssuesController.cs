using System.Linq;
using Microsoft.AspNetCore.Mvc;
using WhatDoYouMeme.Areas.Admin.Models;
using WhatDoYouMeme.Data;
using static WhatDoYouMeme.WebConstants;

namespace WhatDoYouMeme.Areas.Admin.Controllers
{
    public class IssuesController : AdminController
    {
        private readonly ApplicationDbContext data;

        public IssuesController(ApplicationDbContext data)
         =>   this.data = data;
      
           
        public IActionResult All()
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
                    MemerName = i.Memer.Name
                })
                .ToList();

            return View(issues);
        }

        
    }
}
