using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static WhatDoYouMeme.Data.DataConstants;

namespace WhatDoYouMeme.Data.Models
{
    public class Issues
    {
        public int Id { get; init; }
        [Required]
        [MaxLength(IssueTitleMaxLength)]
        [MinLength(IssueTitleMinLength)]
        public string Title { get; set; }
        [Required]
        public string Date { get; set; }
        [Required]
        [MaxLength(IssueDescriptionMaxLength)]
        [MinLength(IssueDescriptionMinLength)]
        public string Description { get; set; }
        public bool IsSolved { get; set; }
        public int MemerId { get; init; }
        public Memer Memer { get; init; }
        public string UserEmail { get; set; }
    }
}
