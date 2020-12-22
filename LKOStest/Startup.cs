using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LKOStest.Controllers;
using LKOStest.Interfaces;
using LKOStest.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace LKOStest
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
            services.AddControllers();
            services.AddScoped<ISearchService, SearchService>();
            services.AddScoped<ITripService, TripService>();
            services.AddScoped<IDistanceMatrixService, DistanceMatrixService>();
            services.AddScoped<IOrganisationService, OrganisationService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IReviewService, ReviewService>();
            services.AddDbContext<TripContext>(item => item.UseSqlServer(Configuration.GetConnectionString("TripContext")));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
