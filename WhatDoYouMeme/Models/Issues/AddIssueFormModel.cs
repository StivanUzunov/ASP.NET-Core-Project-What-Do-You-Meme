using System.ComponentModel.DataAnnotations;
using static WhatDoYouMeme.Data.DataConstants;
namespace WhatDoYouMeme.Models.Issues
{
    public class AddIssueFormModel
    {
        [Required]
        [StringLength(IssueTitleMaxLength, MinimumLength = IssueTitleMinLength)]
        public string Title { get; init; }
        public string Date { get; init; }
        [Required]
        [StringLength(IssueDescriptionMaxLength, MinimumLength = IssueTitleMinLength)]
        public string Description { get; init; }
        [Required]
        public int MemerId { get; init; }
    }
}
