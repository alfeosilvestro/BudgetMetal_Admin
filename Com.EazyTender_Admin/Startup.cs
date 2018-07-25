using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Com.BudgetMetal.DataRepository.Code_Category;
using Com.BudgetMetal.DataRepository.Gallery;
using Com.BudgetMetal.DataRepository.Single_Sign_On;
using Com.BudgetMetal.DataRepository.Users;
using Com.BudgetMetal.DB;
using Com.BudgetMetal.Services.Code_Category;
using Com.BudgetMetal.Services.Gallery;
using Com.BudgetMetal.Services.Role;
using Com.BudgetMetal.Services.User;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
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
            services.AddCors(options => options.AddPolicy("AllowAll", p => p.AllowAnyOrigin()
                                                                   .AllowAnyMethod()
                                                                    .AllowAnyHeader()));

            //services.AddDbContext<AppDbContext>(options => options.UseMySql("Server=localhost;Database=mrthintan;UserID=mrthintan_user;Password=Qwer@123;"));
            // Use a PostgreSQL database
            var sqlConnectionString = Configuration.GetConnectionString("DefaultConnection");
            var sqlConnectionStringBM = Configuration.GetConnectionString("BMConnection");
            // TODO: Update in each application
            services.AddDbContext<DataContext>(options =>
                    options.UseMySQL(
                    sqlConnectionString,
                    b => b.MigrationsAssembly("Com.BudgetMetal.Services.Gallery") // main project name = Application Specific
                )
            );

            services.AddDbContext<DataContext_BM>(options =>
                    options.UseMySQL(
                    sqlConnectionStringBM,
                    b => b.MigrationsAssembly("Com.BudgetMetal.Services.Gallery") // main project name = Application Specific
                )
            );

            services.Configure<Configurations.AppSettings>(Configuration.GetSection("AppSettings"));

            services.AddMvc();

            RegisterForDependencyInjection(services);
        }

        private void RegisterForDependencyInjection(IServiceCollection services)
        {
            //// Register for repository classes
            services.AddScoped<IRoleRepository, RoleRepository>();

            //// Register for repository classes
            services.AddScoped<ISingle_Sign_OnRepository, Single_Sign_OnRepository>();

            //// Register for logic classes
            services.AddScoped<IGalleryService, GalleryService>();

            services.AddScoped<Com.BudgetMetal.DataRepository.Roles.IRoleRepository, Com.BudgetMetal.DataRepository.Roles.RoleRepository>();
            services.AddScoped<IRoleService, RoleService>();

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserService, UserService>();

            services.AddScoped<ICodeCategoryRepository, CodeCategoryRepository>();
            services.AddScoped<ICodeCategoryService, CodeCategoryService>();

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
