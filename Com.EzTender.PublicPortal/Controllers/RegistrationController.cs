using Microsoft.AspNetCore.Mvc;
using Com.BudgetMetal.Services.RFQ;
using Com.BudgetMetal.Services.Industries;
using Configurations;
using Com.BudgetMetal.Services.Company;
using Com.BudgetMetal.Services.ServiceTags;
using Com.BudgetMetal.Services.Users;
using Com.BudgetMetal.Services.Attachment;
using Microsoft.Extensions.Options;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Com.EzTender.PublicPortal.Controllers
{
    public class RegistrationController : Controller
    {
        private readonly AppSettings _appSettings;
        private readonly IIndustryService industryService;
        private readonly IRFQService rfqService;
        private readonly ICompanyService companyService;
        private readonly IServiceTagsService serviceTagsService;
        private readonly IUserService userService;
        private readonly IAttachmentService attachmentService;

        public RegistrationController(IIndustryService industryService, 
                                      ICompanyService companyService, 
                                      IServiceTagsService serviceTagsService, 
                                      IUserService userService, 
                                      IOptions<AppSettings> appSettings)
        {
            this.industryService = industryService;
            this.companyService = companyService;
            this.serviceTagsService = serviceTagsService;
            this.userService = userService;
            this._appSettings = appSettings.Value;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }
    }
}
