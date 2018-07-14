using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Com.BudgetMetal.DataRepository.Gallery;
using Com.BudgetMetal.DataRepository.Single_Sign_On;
using Com.BudgetMetal.Services.Gallery;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Com.EazyTender_Admin
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
            services.AddMvc();

            RegisterForDependencyInjection(services);
        }

        private void RegisterForDependencyInjection(IServiceCollection services)
        {
            //// Register for repository classes
            services.AddScoped<IGalleryRepository, GalleryRepository>();

            //// Register for repository classes
            services.AddScoped<ISingle_Sign_OnRepository, Single_Sign_OnRepository>();

            //// Register for logic classes
            services.AddScoped<IGalleryService, GalleryService>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
