using System.Collections.Generic;
using System.Linq;
using WhatDoYouMeme.Data.Models;

namespace WhatDoYouMeme.Test.Data
{
    public  class Memes
    {
        public static IEnumerable<Post> TenPublicMemes()
            => Enumerable.Range(0, 10).Select(i => new Post
            {
                isPublic = true
            });
    }
}
