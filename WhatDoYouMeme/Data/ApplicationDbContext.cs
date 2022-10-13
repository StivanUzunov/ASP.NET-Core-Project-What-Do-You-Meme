using WhatDoYouMeme.Data.Models;

namespace WhatDoYouMeme.Data
{
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    public class ApplicationDbContext : IdentityDbContext
    {
       
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        public DbSet<Post> Posts { get; init; }
        public DbSet<Comment> Comments { get; init; }
    }
}
