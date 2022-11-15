using System.ComponentModel.DataAnnotations;
using static WhatDoYouMeme.Data.DataConstants;
namespace WhatDoYouMeme.Data.Models
{
    public class Comment
    {

        public int Id { get; init; }

        [Required]
        [MaxLength(CommentMaxLength)]
        public string CommentText { get; set; }
        [Required]
        public string Date { get; set; }
        [Required]
        public int Likes { get; set; }
        public int? PostId { get; set; }
        public Post Post { get; set; }
        public int? MemerId { get; init; }
        public Memer Memer { get; init; }
        public string MemerName { get; set; }
        public int? VideoId { get; init; }
        public Video Video { get; set; }
       
    }
}
