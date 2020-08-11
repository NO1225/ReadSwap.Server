using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace ReadSwap.Data
{
    public static class DataAccessFactory
    {
        public static IServiceCollection AddDataAccess(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DataAccess>(options => options.UseSqlServer(configuration.GetConnectionString("Default")));

            return services;
        }
    }
}
