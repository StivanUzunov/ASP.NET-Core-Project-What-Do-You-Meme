using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WhatDoYouMeme.Data;

namespace WhatDoYouMeme.Infrastructure
{
    public static class ApplicationBuilderExtensions
    {

        public static IApplicationBuilder PrepareDatabase(this IApplicationBuilder app)
        {
            using var scopedServices = app.ApplicationServices.CreateScope();
           var data = scopedServices.ServiceProvider.GetService<ApplicationDbContext>();
            data.Database.Migrate();
            return app;
        }
    }


}
