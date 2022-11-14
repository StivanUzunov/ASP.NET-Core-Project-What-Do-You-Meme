using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static WhatDoYouMeme.Data.DataConstants;

namespace WhatDoYouMeme.Data.Models
{
    public class Memer
    {

        public int Id { get; init; }

        [Required]
        [MaxLength(MemerNameMaxLength)]
        public string Name { get; set; }

        [Required]
        [MaxLength(MemerPhoneNumberMaxLength)]
        public string PhoneNumber { get; set; }

        [Required]
        public string UserId { get; set; }

        public IEnumerable<Post> Posts { get; init; } = new List<Post>();

        public IEnumerable<Comment> Comments { get; init; } = new List<Comment>();
        public IEnumerable<Issues> Issues { get; init; } = new List<Issues>();
    }
}
