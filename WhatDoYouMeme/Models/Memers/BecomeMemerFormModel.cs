using System.ComponentModel.DataAnnotations;
using static WhatDoYouMeme.Data.DataConstants;
namespace WhatDoYouMeme.Models.Memers
{
    public class BecomeMemerFormModel
    {
        [Required]
        [StringLength(MemerNameMaxLength,MinimumLength = MemerNameMinLength)]
        public string Name { get; set; }
        [Required]
        [StringLength(MemerPhoneNumberMaxLength,MinimumLength = MemerPhoneNumberMinLength)]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
    }
}
