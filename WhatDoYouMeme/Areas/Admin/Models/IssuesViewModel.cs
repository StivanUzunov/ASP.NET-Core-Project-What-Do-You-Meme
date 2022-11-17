using System.Collections.Generic;

namespace WhatDoYouMeme.Areas.Admin.Models
{
    public class IssuesViewModel
    {
        public int IssuesReviewed { get; set; }
        public List<IssueListingViewModel> Issues { get; set; }
    }
}
