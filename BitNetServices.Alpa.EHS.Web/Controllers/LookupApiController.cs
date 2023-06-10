using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BitNetServices.Alpa.EHS.Common.Interfaces;
using BitNetServices.Alpa.EHS.Web.Filter;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace BitNetServices.Alpa.EHS.Web.Controllers
{
    [SessionExpire]
    [AuthorizedTenantOnly]

    public class LookupApiController : BaseController
    {
        #region Fields

        private readonly ITenantRepository _tenantRepository;
        private readonly IStringLocalizer<AccountController> _localizer;
        private readonly ICatalogRepository _catalogRepository;
        private readonly ILogger _logger;
        private readonly DnsClient.ILookupClient _client;

        #endregion

        #region Constructors

        public LookupApiController(IStringLocalizer<AccountController> localizer, IStringLocalizer<BaseController> baseLocalizer, ITenantRepository tenantRepository,
            ICatalogRepository catalogRepository, ILogger<LookupApiController> logger,
            IConfiguration configuration, DnsClient.ILookupClient client)
            : base(baseLocalizer, tenantRepository, configuration, client)
        {
            _localizer = localizer;
            _tenantRepository = tenantRepository;
            _catalogRepository = catalogRepository;
            _logger = logger;
            _client = client;
        }

        #endregion

        [Produces("application/json")]
        [HttpGet("{tenant}/api/lookup/departments")]
        public async Task<JsonResult> Departments(int tenantId)
        {
            try
            {
                var departments = await _tenantRepository.GetAllDepartment(tenantId);
                return new JsonResult(departments);
            }
            catch
            {
                return new JsonResult(new object());
            }
        }

        [Produces("application/json")]
        [HttpGet("{tenant}/api/lookup/employee")]
        public async Task<JsonResult> Employee(string term, int? departmentId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(term)) return new JsonResult(new object());
                var urlPath = Request.Path.ToString();
                string[] urlPieces = urlPath.Split(new[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
                var tenantName = urlPieces[0];

                var tenantDetails = await _catalogRepository.GetTenant(tenantName);

                var employees = await _tenantRepository.LookupEmployee(tenantDetails.TenantId, term, departmentId);
                return new JsonResult(employees);
            }
            catch
            {
                return new JsonResult(new object());
            }
        }

        [Produces("application/json")]
        [HttpGet("{tenant}/api/lookup/location")]

        public async Task<JsonResult> Location(string term)
        {
            if (string.IsNullOrWhiteSpace(term)) return Json(new object());
            var urlPath = Request.Path.ToString();
            string[] urlPieces = urlPath.Split(new[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
            var tenantName = urlPieces[0];

            var tenantDetails = await _catalogRepository.GetTenant(tenantName);

            var areaOrBuilding = await _tenantRepository.LookupLocation(tenantDetails.TenantId, term);
            return Json(areaOrBuilding);
        }

        [Produces("application/json")]
        [HttpGet("{tenant}/api/lookup/sublocation")]

        public async Task<JsonResult> AreaOrBuilding(string term, int locationId)
        {
            if (string.IsNullOrWhiteSpace(term) && locationId == 0) return Json(new object());
            var urlPath = Request.Path.ToString();
            string[] urlPieces = urlPath.Split(new[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
            var tenantName = urlPieces[0];

            var tenantDetails = await _catalogRepository.GetTenant(tenantName);
            term = term ?? String.Empty;
            var areaOrBuilding = await _tenantRepository.LookupAreaOrBuilding(tenantDetails.TenantId, term, locationId);
            return Json(areaOrBuilding);
        }
    }
}
