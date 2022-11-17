namespace WhatDoYouMeme.Areas.Admin.Models
{
    public class IssueListingViewModel
    {
        public int Id { get; init; }
        public string Date { get; init; }
        public string Title { get; init; }
        public string Description { get; init; }
        public int MemerId { get; set; }
        public string MemerName { get; set; }
        public string UserEmail { get; set; }
    }
}
