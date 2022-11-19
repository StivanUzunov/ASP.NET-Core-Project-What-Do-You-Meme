using System.Collections.Generic;
using WhatDoYouMeme.Areas.Admin.Models;
using WhatDoYouMeme.Models.Issues;

namespace WhatDoYouMeme.Services.Issues
{
    public interface IIssueService
    {
        public int ReviewedIssues();
        public void DeleteIssues();
        public void IsReviewed(int id);
        public void Log(AddIssueFormModel issue, int memerId, string email);
        public List<IssueListingViewModel> AllIssues();
    }
}
