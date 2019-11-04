using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using IdentityAPI.Infrastructure;
using Swashbuckle.AspNetCore.Swagger;

namespace IdentityAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<IdentityDbContext>(config =>
            {
                config.UseSqlServer(Configuration.GetConnectionString("identityConnection"));
            });
            services.AddCors(c =>
            {
                c.AddDefaultPolicy(X => X.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

                //c.AddPolicy("Allowpartners", x =>
                // {
                //     x.WithOrigins("http://mic5rosoft.com", "http://synergetics.com")
                //     .WithMethods("GET", "POST")
                //     .AllowAnyHeader();
                // });
            });
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Swashbuckle.AspNetCore.Swagger.Info
                {
                    Title = "Identity API",
                    Description = "Identity management API methods for ESHOP Application",
                    Version = "1.0",
                    Contact = new Contact
                    {
                        Name = "Kedar",
                        Email = "kedar@abc.com"
                    }
                });
            });
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseCors();
            app.UseSwagger();
            if (env.IsDevelopment())
            {
                app.UseSwaggerUI(config => //swaggerui for testing
                {
                    config.SwaggerEndpoint("/swagger/v1/swagger.json", "Identity API");
                    config.RoutePrefix = "";
                });
            }
            app.UseMvc();
        }
    }
}
