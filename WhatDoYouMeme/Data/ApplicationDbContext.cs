using Microsoft.AspNetCore.Identity;
using WhatDoYouMeme.Data.Models;

namespace WhatDoYouMeme.Data
{
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<Post> Posts { get; init; }
        public DbSet<Comment> Comments { get; init; }
        public DbSet<Memer> Memers { get; init; }
        public DbSet<Issues> Issues { get; init; }
        public DbSet<Video> Videos { get; init; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Memer>()
                .HasOne<IdentityUser>()
                .WithOne()
                .HasForeignKey<Memer>(m=>m.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Post>()
                .HasOne(p => p.Memer)
                .WithMany(m => m.Posts)
                .HasForeignKey(m => m.MemerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Memer>()
                .HasMany(i => i.Issues)
                .WithOne(m => m.Memer)
                .HasForeignKey(m => m.MemerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Video>()
                .HasOne(p => p.Memer)
                .WithMany(m => m.Videos)
                .HasForeignKey(m => m.MemerId)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(builder);
        }
    }
}
