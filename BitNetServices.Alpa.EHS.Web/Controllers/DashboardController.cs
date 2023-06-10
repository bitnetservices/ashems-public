using System;
using System.Threading.Tasks;
using BitNetServices.Alpa.EHS.Common.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System.Linq;
using BitNetServices.Alpa.EHS.Web.Tenant.Models;
using AutoMapper;
using BitNetServices.Alpa.EHS.Common.Model.Catalog;
using System.Collections.Generic;
using BitNetServices.Alpa.EHS.Web.Filter;
using BitNetServices.Alpa.EHS.Common.Model.Tenant;
using Microsoft.AspNetCore.Http;

namespace BitNetServices.Alpa.EHS.Web.Controllers
{
    [SessionExpire]
    [AuthorizedTenantOnly]
    public class DashboardController : BaseController
    {
        #region Fields
        private readonly ITenantRepository _tenantRepository;
        private readonly ICatalogRepository _catalogRepository;
        private readonly ILogger _logger;
        private readonly DnsClient.ILookupClient _client;
        private readonly IConfiguration _configuration;
        private readonly IUtilities _utilities;
        private String _appRegion;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        #endregion

        #region Constructors
        public DashboardController(ITenantRepository tenantRepository, ICatalogRepository catalogRepository, IStringLocalizer<BaseController> baseLocalizer, ILogger<HazardIncidentsController> logger,
                                IConfiguration configuration, DnsClient.ILookupClient client, IUtilities utilities, IMapper mapper
            , IHttpContextAccessor httpContextAccessor) : base(baseLocalizer, tenantRepository, configuration, client)
        {
            _logger = logger;
            _tenantRepository = tenantRepository;
            _catalogRepository = catalogRepository;
            _client = client;
            _utilities = utilities;
            _configuration = configuration;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _appRegion = configuration["APP_REGION"];
        }

        #endregion

        [Route("{tenant}/dashboard")]
        public async Task<ActionResult> Index(string tenant)
        {
            try
            {
                if (!string.IsNullOrEmpty(tenant))
                {
                    var tenantDetails = await _catalogRepository.GetTenant(tenant);

                    var model = await _tenantRepository.GetAllHazardIncidentForDashboard(tenantDetails.TenantId);
                    var summaryModel = new DashboardViewModel()
                    {
                        User = GetUser,
                        ChartHazardIncidentByStage = this.GetHazardIncidentStageSummary(model) ?? new List<ChartData>(),
                        ChartHazardIncidentByType = this.GetHazardIncidentTypeSummary(model) ?? new List<ChartData>(),
                        HazardIncidentCount = model.Count,
                        HazardIncidentAverageClosingTime = this.GetHazardIncidentAverageClosingTime(model),
                        NeedAttention = GetNeedAttentionCount(model),
                        InvestigationFinished = GetInvestigationFinishedCount(model),
                        UnderInvestigation = GetUnderInvestigationCount(model),
                        YourDepartment = GetYourAttentionCount(model),
                    };
                    return View(summaryModel);

                }
                else
                {
                    return View("TenantError", tenant);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(0, ex, "Get events failed for tenant {tenant}", tenant);
                return View("Error", ex.Message);
            }
        }

        private IEnumerable<ChartData> GetHazardIncidentStageSummary(List<HazardIncidentModel> model)
        {
            var summary = model.GroupBy(x => x.HazardIncidentStage.Name)
                    .Select(y => new ChartData
                    {
                        labels = y.Key,
                        data = y.Count()
                    });

            return summary;
        }

        private IEnumerable<ChartData> GetHazardIncidentTypeSummary(List<HazardIncidentModel> model)
        {
            var summary = model.GroupBy(x => x.CategoryOfHazard.Name)
                    .Select(y => new ChartData
                    {
                        labels = y.Key,
                        data = y.Count()
                    });

            return summary;
        }

        private string GetHazardIncidentAverageClosingTime(List<HazardIncidentModel> model)
        {
            var summary = model.Where(where => where.HazardIncidentStageId >= 4);
            if (summary.Count() > 0) {
                var averageClosing = summary.Select(s => (s.ModifiedOn - s.CreatedOn).TotalMilliseconds).Average();
                TimeSpan averageTime = new TimeSpan(TimeSpan.TicksPerMillisecond * Convert.ToInt64(averageClosing));
                return averageTime.TotalHours.ToString("00");
            }
            return "0";
        }

        private int GetYourAttentionCount(List<HazardIncidentModel> model)
        {
            return model.Where(where => where.HazardIncidentStageId < 4 &&
                                     (where.ResponsibleDepartmentId == GetUser.Employee.DepartmentId || where.ResponsibleIncidentDepartmentId == GetUser.Employee.DepartmentId))
                          .Count();
        }

        private int GetNeedAttentionCount(List<HazardIncidentModel> model)
        {
            return model.Where(where => where.HazardIncidentStageId < 4 && 
                                     (where.EmployeeAssignedId == GetUser.Employee.EmployeeId || where.IncidentEmployeeAssignedId == GetUser.Employee.EmployeeId))
                          .Count();
        }
        private int GetUnderInvestigationCount(List<HazardIncidentModel> model)
        {
            return model.Where(where => where.HazardIncidentStageId == 5 &&
                                     (where.ResponsibleDepartmentId == GetUser.Employee.DepartmentId || where.ResponsibleIncidentDepartmentId == GetUser.Employee.DepartmentId))
                          .Count();
        }
        private int GetInvestigationFinishedCount(List<HazardIncidentModel> model)
        {
            return model.Where(where => where.HazardIncidentStageId == 6 &&
                                     (where.ResponsibleDepartmentId == GetUser.Employee.DepartmentId || where.ResponsibleIncidentDepartmentId == GetUser.Employee.DepartmentId))
                          .Count();
        }
        private UserModel GetUser
        {
            get { return _httpContextAccessor.HttpContext.Session.GetObjectFromJson<List<UserModel>>("SessionUsers").Last(); }
        }


        [Route("{tenant}/comingsoon")]
        public async Task<ActionResult> ComingSoon(string tenant)
        {
            return View();
        }
    }
}

