using Com.BudgetMetal.DataRepository.Company;
using Com.BudgetMetal.DataRepository.RFQ;
using Com.BudgetMetal.DataRepository.Users;
using Com.BudgetMetal.DB;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Com.EzTender.SchedulerJob
{
    class Program
    {


        static void Main(string[] args)
        {
            
            try
            {
                Console.WriteLine("Hello World!");
                string sqlConnectionString = "server=35.229.71.137;userid=platform_user;password=password;database=platform_db;SslMode=none;";
                var services = new ServiceCollection()
                .AddDbContext<DataContext>(options =>
                      options.UseMySQL(
                      sqlConnectionString,
                      b => b.MigrationsAssembly("Com.BudgetMetal.Services.Gallery") // main project name = Application Specific
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
            // add app
            services.AddTransient<App>();
        }


    }

    
}
