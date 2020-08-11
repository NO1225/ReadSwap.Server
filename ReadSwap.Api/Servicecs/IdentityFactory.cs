using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using ReadSwap.Core.Models;
using ReadSwap.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReadSwap.Api.Servicecs
{
    public static class IdentityFactory
    {
        public static IServiceCollection AddIdentity(this IServiceCollection services)
        {

            services.AddIdentity<AppUser, IdentityRole>()
                .AddEntityFrameworkStores<DataAccess>();

            return services;
        }

        public static IServiceCollection ConfigureIdentityOptions(this IServiceCollection services)
        {

            services.Configure<IdentityOptions>(options =>
            {
               // No dublicate emails
                options.User.RequireUniqueEmail = true;
            });

            return services;
        }


        public static IServiceCollection ConfigureWeakPassward(this IServiceCollection services)
        {

            services.Configure<IdentityOptions>(options =>
            {
                // Very weak passward
                options.Password.RequiredLength = 8;
                options.Password.RequireDigit = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
            });

            return services;
        }
    }
}
