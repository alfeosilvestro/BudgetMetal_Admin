using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Com.BudgetMetal.DataRepository.Attachment;
using Com.BudgetMetal.DataRepository.Blogs;
using Com.BudgetMetal.DataRepository.Clarification;
using Com.BudgetMetal.DataRepository.Code_Category;
using Com.BudgetMetal.DataRepository.Code_Table;
using Com.BudgetMetal.DataRepository.Company;
using Com.BudgetMetal.DataRepository.CompanySupplier;
using Com.BudgetMetal.DataRepository.Document;
using Com.BudgetMetal.DataRepository.DocumentActivity;
using Com.BudgetMetal.DataRepository.DocumentUser;
using Com.BudgetMetal.DataRepository.EmailLog;
using Com.BudgetMetal.DataRepository.Industries;
using Com.BudgetMetal.DataRepository.InvitedSupplier;
using Com.BudgetMetal.DataRepository.Penalty;
using Com.BudgetMetal.DataRepository.Quotation;
using Com.BudgetMetal.DataRepository.QuotationPriceSchedule;
using Com.BudgetMetal.DataRepository.QuotationRequirement;
using Com.BudgetMetal.DataRepository.Rating;
using Com.BudgetMetal.DataRepository.Requirement;
using Com.BudgetMetal.DataRepository.RFQ;
using Com.BudgetMetal.DataRepository.RfqInvites;
using Com.BudgetMetal.DataRepository.RfqPriceSchedule;
using Com.BudgetMetal.DataRepository.Roles;
using Com.BudgetMetal.DataRepository.ServiceTags;
using Com.BudgetMetal.DataRepository.Single_Sign_On;
using Com.BudgetMetal.DataRepository.Sla;
using Com.BudgetMetal.DataRepository.SupplierServiceTags;
using Com.BudgetMetal.DataRepository.TimeLine;
using Com.BudgetMetal.DataRepository.UserRoles;
using Com.BudgetMetal.DataRepository.Users;
using Com.BudgetMetal.DB;
using Com.BudgetMetal.Services.Attachment;
using Com.BudgetMetal.Services.Blogs;
using Com.BudgetMetal.Services.Code_Category;
using Com.BudgetMetal.Services.Code_Table;
using Com.BudgetMetal.Services.Company;
using Com.BudgetMetal.Services.EmailLog;
using Com.BudgetMetal.Services.Facebook;
using Com.BudgetMetal.Services.Gallery;
using Com.BudgetMetal.Services.Industries;
using Com.BudgetMetal.Services.Quotation;
using Com.BudgetMetal.Services.RFQ;
using Com.BudgetMetal.Services.Roles;
using Com.BudgetMetal.Services.ServiceTags;
using Com.BudgetMetal.Services.TimeLine;
using Com.BudgetMetal.Services.Users;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Com.EzTender.WebApp
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

            services.Configure<EazyTender.WebApp.Configurations.AppSettings>(Configuration.GetSection("AppSettings"));


            services.AddDistributedMemoryCache();
            services.AddMvc().AddSessionStateTempDataProvider();
            services.AddSession(options =>
            {
                // Set a short timeout for easy testing.
                options.IdleTimeout = TimeSpan.FromHours(10);
                options.Cookie.HttpOnly = true;
            });

            //services.AddMvc();
            services.AddMvc(options => options.MaxModelValidationErrors = 50)
                 .AddViewLocalization(opts => { opts.ResourcesPath = "Resources"; })
                .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
                .AddDataAnnotationsLocalization(); 
            RegisterForDependencyInjection(services);

            //Localization
            services.AddLocalization(options => {
                options.ResourcesPath = "Resources";
            });

            services.AddLocalization(opts =>
            {
                opts.ResourcesPath = "Resources";
            });



            services.Configure<RequestLocalizationOptions>(opts =>
            {
                var supportedCultures = new List<CultureInfo>
                {
                    new CultureInfo("en-US"),
                    new CultureInfo("my-mm"),
                };
                opts.DefaultRequestCulture = new RequestCulture("en-US");
                opts.SupportedCultures = supportedCultures;
                opts.SupportedUICultures = supportedCultures;
            });
        }

        private void RegisterForDependencyInjection(IServiceCollection services)
        {
           

            //// Register for repository classes
            services.AddScoped<ISingle_Sign_OnRepository, Single_Sign_OnRepository>();

            //// Register for logic classes
            services.AddScoped<IGalleryService, GalleryService>();

            services.AddScoped<IDocumentRepository, DocumentRepository>();
            services.AddScoped<IRequirementRepository, RequirementRepository>();
            services.AddScoped<ISlaRepository, SlaRepository>();
            services.AddScoped<IPenaltyRepository, PenaltyRepository>();
            services.AddScoped<IInvitedSupplierRepository, InvitedSupplierRepository>();
            services.AddScoped<IRfqPriceScheduleRepository, RfqPriceScheduleRepository>();
            services.AddScoped<IDocumentUserRepository, DocumentUserRepository>();
            services.AddScoped<IDocumentActivityRepository, DocumentActivityRepository>();
            //Rating
            services.AddScoped<IRatingRepository, RatingRepository>();
            //CompanySupplier
            services.AddScoped<ICompanySupplierRepository, CompanySupplierRepository>();
            services.AddScoped<IRfqInvitesRepository, RfqInvitesRepository>();

            services.AddScoped<IRFQService, RFQService>();
            services.AddScoped<IRfqRepository, RfqRepository>();

            services.AddScoped<IQuotationService, QuotationService>();
            services.AddScoped<IQuotationRepository, QuotationRepository>();

            services.AddScoped<IQuotationPriceScheduleRepository, QuotationPriceScheduleRepository>();
            services.AddScoped<IQuotationRequirementRepository, QuotationRequiremetRepository>();

            services.AddScoped<IAttachmentRepository, AttachmentRepository>();
            services.AddScoped<IAttachmentService, AttachmentService>();

            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IRoleService, RoleService>();

            services.AddScoped<ICompanyRepository, CompanyRepository>();
            services.AddScoped<ICompanyService, CompanyService>();

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserService, UserService>();

            services.AddScoped<IUserRolesRepository, UserRolesRepository>();

            services.AddScoped<ICodeCategoryRepository, CodeCategoryRepository>();
            services.AddScoped<ICodeCategoryService, CodeCategoryService>();

            services.AddScoped<IIndustryRepository, IndustryRepository>();
            services.AddScoped<IIndustryService, IndustryService>();

            services.AddScoped<IServiceTagsRepository, ServiceTagsRepository>();
            services.AddScoped<IServiceTagsService, ServiceTagsService>();

            services.AddScoped<ITimeLineRepository, TimeLineRepository>();
            services.AddScoped<ITimeLineService, TimeLineService>();

            services.AddScoped<IClarificationRepository, ClarificationRepository>();
            services.AddScoped<ISupplierServiceTagsRepository, SupplierServiceTagsRepository>();

            //CodeTable
            services.AddScoped<ICodeTableRepository, CodeTableRepository>();
            services.AddScoped<ICodeTableService, CodeTableService>();

            //EmailLog
            services.AddScoped<IEmailLogRepository, EmailLogRepository>();
            services.AddScoped<IEmailLogService, EmailLogService>();

            //// Register for repository classes
            services.AddScoped<IBlogService, BlogService>();
            services.AddScoped<IBlogRepository, BlogRepository>();

            //// Register for repository classes
            services.AddScoped<IFacebookService, FacebookService>();
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

            //Localization
            var options = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
            app.UseRequestLocalization(options.Value);

            app.UseCookiePolicy();

            app.UseStaticFiles();
            app.UseSession();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Public}/{action=Index}/{id?}");
            });
        }
    }
}
