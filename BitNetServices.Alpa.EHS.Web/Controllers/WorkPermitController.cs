using System;
using System.Threading.Tasks;
using BitNetServices.Alpa.EHS.Common.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using BitNetServices.Alpa.EHS.Web.Tenant.Models;
using AutoMapper;
using Rotativa.AspNetCore;
using BitNetServices.Alpa.EHS.Common.Model.Catalog;
using System.Collections.Generic;
using BitNetServices.Alpa.EHS.Web.Filter;
using BitNetServices.Alpa.EHS.Common.Model.Tenant;
using Microsoft.AspNetCore.Http;
using Rotativa.Core;
using Rotativa.AspNetCore.Options;

namespace BitNetServices.Alpa.EHS.Web.Controllers
{
    [SessionExpire]
    [AuthorizedTenantOnly]
    public class WorkPermitController : BaseController
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
        public WorkPermitController(ITenantRepository tenantRepository, ICatalogRepository catalogRepository, IStringLocalizer<BaseController> baseLocalizer, ILogger<HazardIncidentsController> logger,
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

        [Route("{tenant}/workpermit")]
        public async Task<ActionResult> Index(string tenant)
        {
            try
            {
                if (!string.IsNullOrEmpty(tenant))
                {
                    var tenantDetails = await _catalogRepository.GetTenant(tenant);

                    var model = await _tenantRepository.GetAllWorkPermit(tenantDetails.TenantId);
                    var summaryModel = new WorkPermitSummaryViewModel()
                    {
                        User = GetUser,
                        WorkPermits = model ?? new List<WorkPermitModel>(),
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


        [Route("{tenant}/workpermit/add")]
        public async Task<ActionResult> Create(string id, string tenant)
        {
            try
            {
                if (!string.IsNullOrEmpty(tenant))
                {
                    var tenantDetails = await _catalogRepository.GetTenant(tenant);
                    if (tenantDetails != null)
                    {

                        var hazardIncident = string.IsNullOrEmpty(id) ?
                                        new WorkPermitModel() { } :
                                        await _tenantRepository.GetWorkPermit(new Guid(id), tenantDetails.TenantId);

                        var model = _mapper.Map<WorkPermitViewModel>(hazardIncident);
                        if (string.IsNullOrEmpty(id))
                        {
                            model.WorkerWorkPermitId = model.WorkerWorkPermits.Select(id => id.WorkerId).ToArray();
                        }
                       

                        var locationOfHazard = await _tenantRepository.GetAllLocation(tenantDetails.TenantId);
                        if (locationOfHazard != null)
                        {
                            model.LocationList = locationOfHazard.Select(h => new SelectListItem
                            {
                                Text = h.Name,
                                Value = h.LocationId.ToString()
                            });
                        }

                        if (model.WorkPermitId != 0)
                        {
                            var areaOrBuilding = await _tenantRepository.GetAllAreaOrBuilding(tenantDetails.TenantId);
                            if (areaOrBuilding != null)
                            {
                                model.AreaOrBuildingList = areaOrBuilding.Select(h => new SelectListItem
                                {
                                    Text = h.Name,
                                    Value = h.AreaOrBuildingId.ToString()
                                });
                            }
                        }

                        var workPermitType = await _tenantRepository.GetAllWorkerPermitTypes(tenantDetails.TenantId);
                        if (workPermitType != null)
                        {
                            model.WorkPermitTypeList = workPermitType.Select(h => new SelectListItem
                            {
                                Text = h.Name,
                                Value = h.WorkPermitTypeId.ToString()
                            });
                        }

                        var departmentList = await _tenantRepository.GetAllDepartment(tenantDetails.TenantId);
                        if (departmentList != null)
                        {
                            model.DepartmentList = departmentList.Select(h => new SelectListItem
                            {
                                Text = h.Name,
                                Value = h.DepartmentId.ToString()
                            });
                        }

                        return View(model);

                    }
                    else
                    {
                        return View("TenantError", tenant);
                    }
                }
            }            
            catch (Exception ex)
            {
                _logger.LogError(0, ex, "Get events failed for tenant {tenant}", tenant);
                return View("Error", ex.Message);
            }
            return View("Error");
        }

        [HttpPost]
        [Route("{tenant}/workpermit/add")]
        public async Task<ActionResult> Create(WorkPermitViewModel model, string tenant, string submit)
        {
            try
            {
                if (!string.IsNullOrEmpty(tenant))
                {
                    var tenantDetails = await _catalogRepository.GetTenant(tenant);
                    if (tenantDetails != null)
                    {
                        if (model.WorkPermitId == 0)
                        {
                            model.CreatedBy = GetUser.Employee.RowId;
                            model.CreatedOn = DateTime.Now;
                            await _tenantRepository.AddWorkPermit(model, tenantDetails.TenantId, submit);
                        }
                        else
                        {
                            model.ModifiedBy = GetUser.Employee.RowId;
                            model.ModifiedOn = DateTime.Now;
                            await _tenantRepository.UpdateWorkPermit(model, tenantDetails.TenantId, submit);

                        }
                    }
                }
                return RedirectToAction("Index");
            }            
            catch (Exception ex)
            {
                _logger.LogError(0, ex, "Get events failed for tenant {tenant}", tenant);
                return View("Error", ex.Message);
            }
        }

        [Route("{tenant}/workpermit/issue")]
        public async Task<ActionResult> Issue(string id, string tenant)
        {
            try
            {
                if (!string.IsNullOrEmpty(tenant))
                {
                    var tenantDetails = await _catalogRepository.GetTenant(tenant);
                    if (tenantDetails != null)
                    {
                        var hazardIncident = await _tenantRepository.GetWorkPermit(new Guid(id), tenantDetails.TenantId);
                        WorkPermitViewModel model = _mapper.Map<WorkPermitViewModel>(hazardIncident);
                        return View(model);
                    }
                    else
                    {
                        return View("TenantError", tenant);
                    }
                }
            }            
            catch (Exception ex)
            {
                _logger.LogError(0, ex, "Get events failed for tenant {tenant}", tenant);
                return View("Error", ex.Message);
            }
            return View("Error");
        }

        [HttpPost]
        [Route("{tenant}/workpermit/issue")]
        public async Task<ActionResult> Issue(WorkPermitViewModel model, string tenant)
        {
            try
            {
                if (!string.IsNullOrEmpty(tenant))
                {
                    var tenantDetails = await _catalogRepository.GetTenant(tenant);
                    if (tenantDetails != null)
                    {
                        model.ModifiedBy = GetUser.Employee.RowId;
                        model.ModifiedOn = DateTime.Now;


                        await _tenantRepository.AssignWorkPermit(model, tenantDetails.TenantId);
                    }
                }
                return RedirectToAction("Index");
            }            
            catch (Exception ex)
            {
                _logger.LogError(0, ex, "Get events failed for tenant {tenant}", tenant);
                return View("Error", ex.Message);
            }            
        }

        [Route("{tenant}/workpermit/update")]
        public async Task<ActionResult> Update(string id, string tenant)
        {
            try
            {
                if (!string.IsNullOrEmpty(tenant))
                {
                    var tenantDetails = await _catalogRepository.GetTenant(tenant);
                    if (tenantDetails != null)
                    {
                        var hazardIncident = await _tenantRepository.GetWorkPermit(new Guid(id), tenantDetails.TenantId);
                        WorkPermitViewModel model = _mapper.Map<WorkPermitViewModel>(hazardIncident);
                        return View(model);
                    }
                    else
                    {
                        return View("TenantError", tenant);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(0, ex, "Get events failed for tenant {tenant}", tenant);
                return View("Error", ex.Message);
            }
            return View("Error");
        }

        [HttpPost]
        [Route("{tenant}/workpermit/update")]
        public async Task<ActionResult> Update(WorkPermitViewModel model, string tenant, string submit)
        {
            try
            {
                if (!string.IsNullOrEmpty(tenant))
                {
                    var tenantDetails = await _catalogRepository.GetTenant(tenant);
                    if (tenantDetails != null)
                    {                        
                        model.ModifiedBy = GetUser.Employee.RowId;
                        model.ModifiedOn = DateTime.Now;
                        await _tenantRepository.UpdateWorkPermit(model, tenantDetails.TenantId, submit);
                    }
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(0, ex, "Get events failed for tenant {tenant}", tenant);
                return View("Error", ex.Message);
            }
        }

        private UserModel GetUser
        {
            get { return _httpContextAccessor.HttpContext.Session.GetObjectFromJson<List<UserModel>>("SessionUsers").Last(); }
        }

    }
}

