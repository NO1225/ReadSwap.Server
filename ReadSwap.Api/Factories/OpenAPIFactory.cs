using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReadSwap.Api.Factories
{
    public static class OpenAPIFactory
    {
        public static IServiceCollection AddOpenAPI(this IServiceCollection services)
        {

            services.AddSwaggerGen(options =>
            {
                // Adding swagger document
                options.SwaggerDoc("v1.0", new OpenApiInfo() { Title = "Main API v1.0", Version = "v1.0" });

                // Include the comments that we wrote in the documentation
                options.IncludeXmlComments("ReadSwap.Api.xml");
                options.IncludeXmlComments("ReadSwap.Core.xml");

                // To use unique names with the requests and responses
                options.CustomSchemaIds(x => x.FullName);

                // Defining the security schema
                var securitySchema = new OpenApiSecurityScheme()
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                };

                // Adding the bearer token authentaction option to the ui
                options.AddSecurityDefinition("Bearer", securitySchema);

                // use the token provided with the endpoints call
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    { securitySchema, new[] { "Bearer" } }
                });

            });

            return services;
        }

        public static IApplicationBuilder ConfigureOpenAPI(this IApplicationBuilder app)
        {
            app.UseSwagger();

            // Add swagger UI
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1.0/swagger.json", "Versioned API v1.0");

                c.DocExpansion(DocExpansion.None);
            });

            return app;
        }
    }
}
