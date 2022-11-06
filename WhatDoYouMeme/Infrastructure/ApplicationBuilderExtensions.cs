using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WhatDoYouMeme.Data;
using static WhatDoYouMeme.Areas.Admin.AdminConstants;

namespace WhatDoYouMeme.Infrastructure
{
    public static class ApplicationBuilderExtensions
    {

        public static IApplicationBuilder PrepareDatabase(this IApplicationBuilder app)
        {
            using var scopedServices = app.ApplicationServices.CreateScope();
            var serviceProvider = scopedServices.ServiceProvider;
           var data = serviceProvider.GetRequiredService<ApplicationDbContext>();
            data.Database.Migrate();
            SeedAdministrator(data, serviceProvider);
            return app;
        }

        private static void SeedAdministrator(ApplicationDbContext data, IServiceProvider services)
        {
            var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

           Task
               .Run( async () =>
           {
               if ( await roleManager.RoleExistsAsync(AdministratorRoleName))
               {
                   return;
               }

               var role = new IdentityRole {Name = AdministratorRoleName};
                await roleManager.CreateAsync(role);
                const string adminEmail = "admin@meme.com";
                const string adminPassword = "Su-123456";
                var user = new IdentityUser()
                {
                    Email = adminEmail,
                    UserName = adminEmail,
                    EmailConfirmed = true
                };

                await userManager.CreateAsync(user, adminPassword);

               await userManager.AddToRoleAsync(user, role.Name);
           })
               .GetAwaiter()
               .GetResult();
        }
    }


}
