using Com.BudgetMetal.DataRepository.Company;
using Com.BudgetMetal.DataRepository.RFQ;
using Com.BudgetMetal.DataRepository.Users;
using Com.BudgetMetal.DB;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration.FileExtensions;
using Microsoft.Extensions.Configuration.Json;
using System;
using System.IO;
using Com.BudgetMetal.DataRepository.Forex;
using Com.BudgetMetal.Services.Facebook;
using Com.BudgetMetal.DataRepository.Code_Table;

namespace Com.EzTender.SchedulerJob
{
    class Program
    {


        static void Main(string[] args)
        {
            
            try
            {
                var builder = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

                IConfigurationRoot configuration = builder.Build();

                //Console.WriteLine(configuration.GetConnectionString("DefaultConnection"));


                //Console.WriteLine("Hello World!");
                string sqlConnectionString = configuration.GetConnectionString("DefaultConnection");
                var services = new ServiceCollection()
                .AddDbContext<DataContext>(options =>
                      options.UseMySQL(
                      sqlConnectionString,
                      b => b.MigrationsAssembly("Com.EzyTender.Scheduler") // main project name = Application Specific
                  ));
                //services.AddScoped<ICompanyRepository, CompanyRepository>();
                ConfigureServices(services);


                // create service provider
                var serviceProvider = services.BuildServiceProvider();
                // entry to run app
                serviceProvider.GetService<App>().Run();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            // add services
            services.AddScoped<ICompanyRepository, CompanyRepository>();
            services.AddScoped<IRfqRepository, RfqRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IForexRepository, ForexRepository>();
            services.AddScoped<ICodeTableRepository, CodeTableRepository>();
            services.AddScoped<IFacebookService, FacebookService>();
            // add app
            services.AddTransient<App>();
        }


    }

    
}
