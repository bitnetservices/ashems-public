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
    public class HazardIncidentsController : BaseController
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
        public HazardIncidentsController(ITenantRepository tenantRepository, ICatalogRepository catalogRepository, IStringLocalizer<BaseController> baseLocalizer, ILogger<HazardIncidentsController> logger,
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

        [Route("{tenant}/hazardincidents")]
        public async Task<ActionResult> Index(string tenant)
        {
            try
            {
                if (!string.IsNullOrEmpty(tenant))
                {
                    var tenantDetails = await _catalogRepository.GetTenant(tenant);

                    var model = await _tenantRepository.GetAllHazardIncident(tenantDetails.TenantId);
                    var summaryModel = new HazardIncidentSummaryViewModel()
                    {
                        User = GetUser,
                        HazardIncidents = model ?? new List<HazardIncidentModel>(),
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
            return View("Error");
        }

        [Route("{tenant}/hazardincidents/investigations")]
        public async Task<ActionResult> Investigation(string tenant)
        {
            try
            {
                if (!string.IsNullOrEmpty(tenant))
                {
                    var tenantDetails = await _catalogRepository.GetTenant(tenant);

                    var model = await _tenantRepository.GetAllHazardIncident(tenantDetails.TenantId, 4);
                    var summaryModel = new HazardIncidentSummaryViewModel()
                    {
                        User = GetUser,
                        HazardIncidents = model ?? new List<HazardIncidentModel>(),
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
            return View("Error");
        }

        [Route("{tenant}/hazardincidents/add")]
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
                                        new HazardIncidentModel()
                                        {
                                            ReportingDate = DateTime.Today,
                                            IncidentDate = Convert.ToDateTime(DateTime.Now.ToString("h:mm:ss tt")),
                                            HazardIncidentFormat = Guid.NewGuid().ToString()
                                        } :
                                        await _tenantRepository.GetHazardIncident(new Guid(id), tenantDetails.TenantId);

                        var model = _mapper.Map<HazardIncidentViewModel>(hazardIncident);
                        if (!string.IsNullOrEmpty(id))
                        {
                            model.IncidentBodyPartId = model.IncidentBodyParts.Select(id => id.BodyPartId).ToArray();
                            model.IncidentFuturePreventionId = model.IncidentFuturePreventions.Select(id => id.FuturePreventionId).ToArray();
                            model.IncidentTaskCarriedId = model.IncidentTaskCarrieds.Select(id => id.TaskCarriedId).ToArray();
                            model.IncidentInjuryTypeId = model.IncidentInjuryTypes.Select(id => id.InjuryTypeId).ToArray();
                        }

                        var employees = await _tenantRepository.GetAllEmployee(tenantDetails.TenantId);
                        if (employees != null)
                        {
                            model.EmployeeList = employees.Select(h => new SelectListItem
                            {
                                Text = h.Name,
                                Value = h.EmployeeId.ToString()
                            });
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

                        if (model.HazardIncidentId != 0)
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

                        var hazardCategoryList = await _tenantRepository.GetAllHazardCategory(tenantDetails.TenantId);
                        if (hazardCategoryList != null)
                        {
                            model.HazardCategoryList = hazardCategoryList.Select(h => new SelectListItem
                            {
                                Text = h.Name,
                                Value = h.HazardCategoryId.ToString()
                            });
                        }

                        var hazardTypeList = await _tenantRepository.GetAllHazardType(tenantDetails.TenantId);
                        if (hazardTypeList != null)
                        {
                            model.HazardTypeList = hazardTypeList.Select(h => new SelectListItem
                            {
                                Text = h.Name,
                                Value = h.HazardTypeId.ToString()
                            });
                        }

                        var hazardStatusList = await _tenantRepository.GetAllHazardStatus(tenantDetails.TenantId);
                        if (hazardStatusList != null)
                        {
                            model.HazardStatusList = hazardStatusList.Select(h => new SelectListItem
                            {
                                Text = h.Name,
                                Value = h.HazardStatusId.ToString()
                            });
                        }

                        var tasksCarriedList = await _tenantRepository.GetAllTaskCarried(tenantDetails.TenantId);
                        if (tasksCarriedList != null)
                        {
                            model.TasksCarriedList = tasksCarriedList.Select(h => new SelectListItem
                            {
                                Text = h.Name,
                                Value = h.TaskCarriedId.ToString(),
                                Selected = model.IncidentTaskCarriedId.Contains(h.TaskCarriedId)
                            });
                        }


                        var employmentTypeList = await _tenantRepository.GetAllEmploymentType(tenantDetails.TenantId);
                        if (employmentTypeList != null)
                        {
                            model.EmploymentTypeList = employmentTypeList.Select(h => new SelectListItem
                            {
                                Text = h.Name,
                                Value = h.EmploymentTypeId.ToString()
                            });
                        }


                        var bodyPartList = await _tenantRepository.GetAllBodyPart(tenantDetails.TenantId);
                        if (bodyPartList != null)
                        {
                            model.BodyPartList = bodyPartList.Select(h => new SelectListItem
                            {
                                Text = h.Name,
                                Value = h.BodyPartId.ToString(),
                                Selected = model.IncidentBodyPartId.Contains(h.BodyPartId)
                            }); ;
                        }

                        var consequenceList = await _tenantRepository.GetAllConsequence(tenantDetails.TenantId);
                        if (consequenceList != null)
                        {
                            model.ConsequenceList = consequenceList.Select(h => new SelectListItem
                            {
                                Text = h.Name,
                                Value = h.ConsequenceId.ToString()
                            });
                        }


                        var contractorList = await _tenantRepository.GetAllContractor(tenantDetails.TenantId);
                        if (contractorList != null)
                        {
                            model.ContractorList = contractorList.Select(h => new SelectListItem
                            {
                                Text = h.Name,
                                Value = h.ContractorId.ToString()
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

                        var futurePreventionList = await _tenantRepository.GetAllFuturePrevention(tenantDetails.TenantId);
                        if (futurePreventionList != null)
                        {
                            model.FuturePreventionList = futurePreventionList.Select(h => new SelectListItem
                            {
                                Text = h.Name,
                                Value = h.FuturePreventionId.ToString(),
                                Selected = model.IncidentFuturePreventionId.Contains(h.FuturePreventionId)
                            });
                        }

                        var injuredPersonCategoryList = await _tenantRepository.GetAllInjuredPersonCategory(tenantDetails.TenantId);
                        if (injuredPersonCategoryList != null)
                        {
                            model.InjuredPersonCategoryList = injuredPersonCategoryList.Select(h => new SelectListItem
                            {
                                Text = h.Name,
                                Value = h.InjuredPersonCategoryId.ToString()
                            });
                        }

                        var injuryTypeList = await _tenantRepository.GetAllInjuryType(tenantDetails.TenantId);
                        if (injuryTypeList != null)
                        {
                            model.InjuryTypeList = injuryTypeList.Select(h => new SelectListItem
                            {
                                Text = h.Name,
                                Value = h.InjuryTypeId.ToString(),
                                Selected = model.IncidentInjuryTypeId.Contains(h.InjuryTypeId)

                            });
                        }

                        var anticipatedAbsencesList = await _tenantRepository.GetAllAnticipatedAbsences(tenantDetails.TenantId);
                        if (anticipatedAbsencesList != null)
                        {
                            model.AnticipatedAbsenceList = anticipatedAbsencesList.Select(h => new SelectListItem
                            {
                                Text = h.Name,
                                Value = h.AnticipatedAbsenceId.ToString()
                            });
                        }

                        var incidentResultList = await _tenantRepository.GetAllIncidentResult(tenantDetails.TenantId);
                        if (incidentResultList != null)
                        {
                            model.IncidentResultList = incidentResultList.Select(h => new SelectListItem
                            {
                                Text = h.Name,
                                Value = h.IncidentResultId.ToString()
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
            catch (Microsoft.Azure.SqlDatabase.ElasticScale.ShardManagement.ShardManagementException ex)
            {
                if (ex.ErrorCode == Microsoft.Azure.SqlDatabase.ElasticScale.ShardManagement.ShardManagementErrorCode.MappingIsOffline)
                {
                    var tenantModel = await _catalogRepository.GetTenant(tenant);
                    _logger.LogInformation(0, ex, "Tenant is offline: {tenant}", tenantModel.TenantName);
                    return View("TenantOffline", tenantModel.TenantName);
                }
                else if (ex.ErrorCode == Microsoft.Azure.SqlDatabase.ElasticScale.ShardManagement.ShardManagementErrorCode.MappingDoesNotExist)
                {
                    var tenantModel = await _catalogRepository.GetTenant(tenant);
                    //Fix mapping irregularities
                    _utilities.ResolveMappingDifferences(tenantModel.TenantId);

                    //Get venue details
                    String tenantStatus = _utilities.GetTenantStatus(tenantModel.TenantId);
                    
                        return View("TenantOffline", tenantModel.TenantName);
                    
                }
                else
                {
                    _logger.LogError(0, ex, "Tenant shard was unavailable for tenant: {tenant}", tenant);
                    return View("Error", ex.Message);
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
        [Route("{tenant}/hazardincidents/add")]
        public async Task<ActionResult> Create(HazardIncidentViewModel model, string tenant, string submit)
        {
            try
            {
                if (!string.IsNullOrEmpty(tenant))
                {
                    var tenantDetails = await _catalogRepository.GetTenant(tenant);
                    if (tenantDetails != null)
                    {
                        if (model.HazardIncidentId == 0)
                        {
                            model.CreatedBy = GetUser.Employee.RowId;                            
                            model.CreatedOn = DateTime.Now;
                            await _tenantRepository.AddHazardIncident(model, tenantDetails.TenantId, submit);
                        }
                        else
                        {
                            model.ModifiedBy = GetUser.Employee.RowId;
                            model.ModifiedOn = DateTime.Now;
                            await _tenantRepository.UpdateHazardIncident(model, tenantDetails.TenantId, submit);

                        }
                    }
                }
                return RedirectToAction("Index");
            }
            catch (Microsoft.Azure.SqlDatabase.ElasticScale.ShardManagement.ShardManagementException ex)
            {
                if (ex.ErrorCode == Microsoft.Azure.SqlDatabase.ElasticScale.ShardManagement.ShardManagementErrorCode.MappingIsOffline)
                {
                    var tenantModel = await _catalogRepository.GetTenant(tenant);
                    _logger.LogInformation(0, ex, "Tenant is offline: {tenant}", tenantModel.TenantName);
                    return View("TenantOffline", tenantModel.TenantName);
                }
                else if (ex.ErrorCode == Microsoft.Azure.SqlDatabase.ElasticScale.ShardManagement.ShardManagementErrorCode.MappingDoesNotExist)
                {
                    var tenantModel = await _catalogRepository.GetTenant(tenant);
                    //Fix mapping irregularities
                    _utilities.ResolveMappingDifferences(tenantModel.TenantId);

                    //Get venue details
                    String tenantStatus = _utilities.GetTenantStatus(tenantModel.TenantId);
                    
                        return View("TenantOffline", tenantModel.TenantName);
                    
                }
                else
                {
                    _logger.LogError(0, ex, "Tenant shard was unavailable for tenant: {tenant}", tenant);
                    return View("Error", ex.Message);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(0, ex, "Get events failed for tenant {tenant}", tenant);
                return View("Error", ex.Message);
            }
            return View("Error");
        }

        [Route("{tenant}/hazardincidents/assign")]
        public async Task<ActionResult> Assign(string id, string tenant)
        {
            try
            {
                if (!string.IsNullOrEmpty(tenant))
                {
                    var tenantDetails = await _catalogRepository.GetTenant(tenant);
                    if (tenantDetails != null)
                    {

                        var hazardIncident = await _tenantRepository.GetHazardIncident(new Guid(id), tenantDetails.TenantId);

                        HazardIncidentViewModel model = _mapper.Map<HazardIncidentViewModel>(hazardIncident); ;

                        var employees = await _tenantRepository.GetAllEmployee(tenantDetails.TenantId);
                        var departmentId = model.CategoryOfHazardId >= 1 && model.CategoryOfHazardId <= 3 ? model.ResponsibleDepartmentId : model.ResponsibleIncidentDepartmentId;
                        employees = employees.Where(where => where.DepartmentId == departmentId).ToList();

                        //if (employees != null)
                        //{
                        //    model.EmployeeList = employees.Select(h => new SelectListItem
                        //    {
                        //        Text = h.Name,
                        //        Value = h.EmployeeId.ToString()
                        //    });
                        //}


                        //var locationOfHazard = await _tenantRepository.GetAllLocation(tenantDetails.TenantId);
                        //if (locationOfHazard != null)
                        //{
                        //    model.LocationList = locationOfHazard.Select(h => new SelectListItem
                        //    {
                        //        Text = h.Name,
                        //        Value = h.LocationId.ToString()
                        //    });
                        //}

                        //var areaOfBuilding = await _tenantRepository.GetAllAreaOrBuilding(tenantDetails.TenantId);
                        //if (areaOfBuilding != null)
                        //{
                        //    model.AreaOrBuildingList = areaOfBuilding.Select(h => new SelectListItem
                        //    {
                        //        Text = h.Name,
                        //        Value = h.AreaOrBuildingId.ToString()
                        //    });
                        //}
                        //var hazardCategoryList = await _tenantRepository.GetAllHazardCategory(tenantDetails.TenantId);
                        //if (hazardCategoryList != null)
                        //{
                        //    model.HazardCategoryList = hazardCategoryList.Select(h => new SelectListItem
                        //    {
                        //        Text = h.Name,
                        //        Value = h.HazardCategoryId.ToString()
                        //    });
                        //}

                        //var hazardTypeList = await _tenantRepository.GetAllHazardType(tenantDetails.TenantId);
                        //if (hazardTypeList != null)
                        //{
                        //    model.HazardTypeList = hazardTypeList.Select(h => new SelectListItem
                        //    {
                        //        Text = h.Name,
                        //        Value = h.HazardTypeId.ToString()
                        //    });
                        //}

                        //var hazardStatusList = await _tenantRepository.GetAllHazardStatus(tenantDetails.TenantId);
                        //if (hazardStatusList != null)
                        //{
                        //    model.HazardStatusList = hazardStatusList.Select(h => new SelectListItem
                        //    {
                        //        Text = h.Name,
                        //        Value = h.HazardStatusId.ToString()
                        //    });
                        //}

                        var tasksCarriedList = await _tenantRepository.GetAllTaskCarried(tenantDetails.TenantId);
                        if (tasksCarriedList != null)
                        {
                            model.TasksCarriedList = tasksCarriedList.Select(h => new SelectListItem
                            {
                                Text = h.Name,
                                Value = h.TaskCarriedId.ToString()
                            });
                        }


                        //var employmentTypeList = await _tenantRepository.GetAllEmploymentType(tenantDetails.TenantId);
                        //if (employmentTypeList != null)
                        //{
                        //    model.EmploymentTypeList = employmentTypeList.Select(h => new SelectListItem
                        //    {
                        //        Text = h.Name,
                        //        Value = h.EmploymentTypeId.ToString()
                        //    });
                        //}


                        var bodyPartList = await _tenantRepository.GetAllBodyPart(tenantDetails.TenantId);
                        if (bodyPartList != null)
                        {
                            model.BodyPartList = bodyPartList.Select(h => new SelectListItem
                            {
                                Text = h.Name,
                                Value = h.BodyPartId.ToString()
                            });
                        }

                        //var consequenceList = await _tenantRepository.GetAllConsequence(tenantDetails.TenantId);
                        //if (consequenceList != null)
                        //{
                        //    model.ConsequenceList = consequenceList.Select(h => new SelectListItem
                        //    {
                        //        Text = h.Name,
                        //        Value = h.ConsequenceId.ToString()
                        //    });
                        //}


                        //var contractorList = await _tenantRepository.GetAllContractor(tenantDetails.TenantId);
                        //if (contractorList != null)
                        //{
                        //    model.ContractorList = contractorList.Select(h => new SelectListItem
                        //    {
                        //        Text = h.Name,
                        //        Value = h.ContractorId.ToString()
                        //    });
                        //}

                        //var departmentList = await _tenantRepository.GetAllDepartment(tenantDetails.TenantId);
                        //if (departmentList != null)
                        //{
                        //    model.DepartmentList = departmentList.Select(h => new SelectListItem
                        //    {
                        //        Text = h.Name,
                        //        Value = h.DepartmentId.ToString()
                        //    });
                        //}

                        var futurePreventionList = await _tenantRepository.GetAllFuturePrevention(tenantDetails.TenantId);
                        if (futurePreventionList != null)
                        {
                            model.FuturePreventionList = futurePreventionList.Select(h => new SelectListItem
                            {
                                Text = h.Name,
                                Value = h.FuturePreventionId.ToString()
                            });
                        }

                        //var injuredPersonCategoryList = await _tenantRepository.GetAllInjuredPersonCategory(tenantDetails.TenantId);
                        //if (injuredPersonCategoryList != null)
                        //{
                        //    model.InjuredPersonCategoryList = injuredPersonCategoryList.Select(h => new SelectListItem
                        //    {
                        //        Text = h.Name,
                        //        Value = h.InjuredPersonCategoryId.ToString()
                        //    });
                        //}

                        var injuryTypeList = await _tenantRepository.GetAllInjuryType(tenantDetails.TenantId);
                        if (injuryTypeList != null)
                        {
                            model.InjuryTypeList = injuryTypeList.Select(h => new SelectListItem
                            {
                                Text = h.Name,
                                Value = h.InjuryTypeId.ToString()
                            });
                        }

                        var anticipatedAbsencesList = await _tenantRepository.GetAllAnticipatedAbsences(tenantDetails.TenantId);
                        if (anticipatedAbsencesList != null)
                        {
                            model.AnticipatedAbsenceList = anticipatedAbsencesList.Select(h => new SelectListItem
                            {
                                Text = h.Name,
                                Value = h.AnticipatedAbsenceId.ToString()
                            });
                        }

                        //var incidentResultList = await _tenantRepository.GetAllIncidentResult(tenantDetails.TenantId);
                        //if (incidentResultList != null)
                        //{
                        //    model.IncidentResultList = incidentResultList.Select(h => new SelectListItem
                        //    {
                        //        Text = h.Name,
                        //        Value = h.IncidentResultId.ToString()
                        //    });
                        //}

                        
                        model.IncidentBodyPartId = model.IncidentBodyParts?.Select(id => id.IncidentBodyPartId).ToArray();
                        model.IncidentFuturePreventionId = model.IncidentFuturePreventions?.Select(id => id.FuturePreventionId).ToArray();
                        model.IncidentTaskCarriedId = model.IncidentTaskCarrieds?.Select(id => id.TaskCarriedId).ToArray();
                        model.IncidentInjuryTypeId = model.IncidentInjuryTypes?.Select(id => id.InjuryTypeId).ToArray();
                        
                        return View(model);

                    }
                    else
                    {
                        return View("TenantError", tenant);
                    }
                }
            }
            catch (Microsoft.Azure.SqlDatabase.ElasticScale.ShardManagement.ShardManagementException ex)
            {
                if (ex.ErrorCode == Microsoft.Azure.SqlDatabase.ElasticScale.ShardManagement.ShardManagementErrorCode.MappingIsOffline)
                {
                    var tenantModel = await _catalogRepository.GetTenant(tenant);
                    _logger.LogInformation(0, ex, "Tenant is offline: {tenant}", tenantModel.TenantName);
                    return View("TenantOffline", tenantModel.TenantName);
                }
                else if (ex.ErrorCode == Microsoft.Azure.SqlDatabase.ElasticScale.ShardManagement.ShardManagementErrorCode.MappingDoesNotExist)
                {
                    var tenantModel = await _catalogRepository.GetTenant(tenant);
                    //Fix mapping irregularities
                    _utilities.ResolveMappingDifferences(tenantModel.TenantId);

                    //Get venue details
                    String tenantStatus = _utilities.GetTenantStatus(tenantModel.TenantId);                    
                    return View("TenantOffline", tenantModel.TenantName);
                }
                else
                {
                    _logger.LogError(0, ex, "Tenant shard was unavailable for tenant: {tenant}", tenant);
                    return View("Error", ex.Message);
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
        [Route("{tenant}/hazardincidents/assign")]
        public async Task<ActionResult> Assign(HazardIncidentViewModel model, string tenant)
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

                        
                        await _tenantRepository.AssignHazardIncident(model, tenantDetails.TenantId);
                    }
                }
                return RedirectToAction("Index");
            }
            catch (Microsoft.Azure.SqlDatabase.ElasticScale.ShardManagement.ShardManagementException ex)
            {
                if (ex.ErrorCode == Microsoft.Azure.SqlDatabase.ElasticScale.ShardManagement.ShardManagementErrorCode.MappingIsOffline)
                {
                    var tenantModel = await _catalogRepository.GetTenant(tenant);
                    _logger.LogInformation(0, ex, "Tenant is offline: {tenant}", tenantModel.TenantName);
                    return View("TenantOffline", tenantModel.TenantName);
                }
                else if (ex.ErrorCode == Microsoft.Azure.SqlDatabase.ElasticScale.ShardManagement.ShardManagementErrorCode.MappingDoesNotExist)
                {
                    var tenantModel = await _catalogRepository.GetTenant(tenant);
                    //Fix mapping irregularities
                    _utilities.ResolveMappingDifferences(tenantModel.TenantId);

                    //Get venue details
                    String tenantStatus = _utilities.GetTenantStatus(tenantModel.TenantId);                    
                    return View("TenantOffline", tenantModel.TenantName);
                }
                else
                {
                    _logger.LogError(0, ex, "Tenant shard was unavailable for tenant: {tenant}", tenant);
                    return View("Error", ex.Message);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(0, ex, "Get events failed for tenant {tenant}", tenant);
                return View("Error", ex.Message);
            }
            return View("Error");
        }

        [HttpGet]
        [Route("{tenant}/hazardincidents/pdf")]
        public async Task<ActionResult> HazardPdf(string id, string tenant)
        {

            try
            {
                if (!string.IsNullOrEmpty(tenant))
                {
                    var tenantDetails = await _catalogRepository.GetTenant(tenant);
                    if (tenantDetails != null)
                    {

                        var hazardIncident = await _tenantRepository.GetHazardIncident(new Guid(id), tenantDetails.TenantId);

                        HazardIncidentViewModel model = _mapper.Map<HazardIncidentViewModel>(hazardIncident); ;
                        var customSwitches = "--footer-right \"Generated on A-SHEMS by Alpa Groups\" --footer-left \"Date: [date] [time]\" " + "--footer-center \"Page: [page] of [toPage]\" --footer-line --footer-font-size \"9\" --footer-spacing 5 --footer-font-name \"calibri light\"";

                        return new ViewAsPdf(model)
                        {

                            CustomSwitches = customSwitches,
                        };
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

        [HttpGet]
        [Route("{tenant}/hazardincidents/investigation/pdf")]
        public async Task<ActionResult> InvestigationPdf(string id, string tenant)
        {

            try
            {
                if (!string.IsNullOrEmpty(tenant))
                {
                    var tenantDetails = await _catalogRepository.GetTenant(tenant);
                    if (tenantDetails != null)
                    {

                        var hazardIncident = await _tenantRepository.GetHazardIncident(new Guid(id), tenantDetails.TenantId);

                        HazardIncidentViewModel model = _mapper.Map<HazardIncidentViewModel>(hazardIncident);

                        var personnelList = await _tenantRepository.GetAllPersonnel(tenantDetails.TenantId);
                        if (personnelList != null)
                        {
                            model.PersonnelList = personnelList.Select(h => new SelectListItem
                            {
                                Text = h.Name,
                                Value = h.PersonnelId.ToString()
                            });
                        }

                        var rootCauseList = await _tenantRepository.GetAllRootCause(tenantDetails.TenantId);
                        if (rootCauseList != null)
                        {
                            model.RootCauseList = rootCauseList.Select(h => new SelectListItem
                            {
                                Text = h.Name,
                                Value = h.RootCauseId.ToString()
                            });
                        }


                        var correctMeasureList = await _tenantRepository.GetAllCorrectiveMeasure(tenantDetails.TenantId);
                        if (correctMeasureList != null)
                        {
                            model.CorrectiveMeasureList = correctMeasureList.Select(h => new SelectListItem
                            {
                                Text = h.Name,
                                Value = h.CorrectiveMeasureId.ToString()
                            });
                        }

                        var processEnvironmentList = await _tenantRepository.GetAllProcessEnvironment(tenantDetails.TenantId);
                        if (processEnvironmentList != null)
                        {
                            model.ProcessEnvironmentList = processEnvironmentList.Select(h => new SelectListItem
                            {
                                Text = h.Name,
                                Value = h.ProcessEnvironmentId.ToString()
                            });
                        }

                        var customSwitches = "--footer-right \"Generated on A-SHEMS by Alpa Groups\" --footer-left \"Date: [date] [time]\" " + "--footer-center \"Page: [page] of [toPage]\" --footer-line --footer-font-size \"9\" --footer-spacing 5 --footer-font-name \"calibri light\"";

                        return new ViewAsPdf(model)
                        {

                            CustomSwitches = customSwitches,
                        };
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

        [Route("{tenant}/hazardincidents/update")]
        public async Task<ActionResult> Update(string id, string tenant)
        {
            try
            {
                if (!string.IsNullOrEmpty(tenant))
                {
                    var tenantDetails = await _catalogRepository.GetTenant(tenant);
                    if (tenantDetails != null)
                    {

                        var hazardIncident = await _tenantRepository.GetHazardIncident(new Guid(id), tenantDetails.TenantId);
                        HazardIncidentViewModel model = _mapper.Map<HazardIncidentViewModel>(hazardIncident);

                        //var employees = await _tenantRepository.GetAllEmployee(tenantDetails.TenantId);
                        ////var departmentId = model.CategoryOfHazardId >= 1 && model.CategoryOfHazardId <= 3 ? model.ResponsibleDepartmentId : model.ResponsibleIncidentDepartmentId;
                        ////employees = employees.Where(where => where.DepartmentId == departmentId).ToList();

                        //if (employees != null)
                        //{
                        //    model.EmployeeList = employees.Select(h => new SelectListItem
                        //    {
                        //        Text = h.Name,
                        //        Value = h.EmployeeId.ToString()
                        //    });
                        //}


                        //var locationOfHazard = await _tenantRepository.GetAllLocation(tenantDetails.TenantId);
                        //if (locationOfHazard != null)
                        //{
                        //    model.LocationList = locationOfHazard.Select(h => new SelectListItem
                        //    {
                        //        Text = h.Name,
                        //        Value = h.LocationId.ToString()
                        //    });
                        //}

                        //var areaOfBuilding = await _tenantRepository.GetAllAreaOrBuilding(tenantDetails.TenantId);
                        //if (areaOfBuilding != null)
                        //{
                        //    model.AreaOrBuildingList = areaOfBuilding.Select(h => new SelectListItem
                        //    {
                        //        Text = h.Name,
                        //        Value = h.AreaOrBuildingId.ToString()
                        //    });
                        //}


                        //var hazardCategoryList = await _tenantRepository.GetAllHazardCategory(tenantDetails.TenantId);
                        //if (hazardCategoryList != null)
                        //{
                        //    model.HazardCategoryList = hazardCategoryList.Select(h => new SelectListItem
                        //    {
                        //        Text = h.Name,
                        //        Value = h.HazardCategoryId.ToString()
                        //    });
                        //}

                        //var hazardTypeList = await _tenantRepository.GetAllHazardType(tenantDetails.TenantId);
                        //if (hazardTypeList != null)
                        //{
                        //    model.HazardTypeList = hazardTypeList.Select(h => new SelectListItem
                        //    {
                        //        Text = h.Name,
                        //        Value = h.HazardTypeId.ToString()
                        //    });
                        //}

                        var hazardStatusList = await _tenantRepository.GetAllHazardStatus(tenantDetails.TenantId);
                        if (hazardStatusList != null)
                        {
                            model.HazardStatusList = hazardStatusList.Select(h => new SelectListItem
                            {
                                Text = h.Name,
                                Value = h.HazardStatusId.ToString()
                            });
                        }

                        var tasksCarriedList = await _tenantRepository.GetAllTaskCarried(tenantDetails.TenantId);
                        if (tasksCarriedList != null)
                        {
                            model.TasksCarriedList = tasksCarriedList.Select(h => new SelectListItem
                            {
                                Text = h.Name,
                                Value = h.TaskCarriedId.ToString()
                            });
                        }


                        //var employmentTypeList = await _tenantRepository.GetAllEmploymentType(tenantDetails.TenantId);
                        //if (employmentTypeList != null)
                        //{
                        //    model.EmploymentTypeList = employmentTypeList.Select(h => new SelectListItem
                        //    {
                        //        Text = h.Name,
                        //        Value = h.EmploymentTypeId.ToString()
                        //    });
                        //}


                        var bodyPartList = await _tenantRepository.GetAllBodyPart(tenantDetails.TenantId);
                        if (bodyPartList != null)
                        {
                            model.BodyPartList = bodyPartList.Select(h => new SelectListItem
                            {
                                Text = h.Name,
                                Value = h.BodyPartId.ToString()
                            });
                        }

                        //var consequenceList = await _tenantRepository.GetAllConsequence(tenantDetails.TenantId);
                        //if (consequenceList != null)
                        //{
                        //    model.ConsequenceList = consequenceList.Select(h => new SelectListItem
                        //    {
                        //        Text = h.Name,
                        //        Value = h.ConsequenceId.ToString()
                        //    });
                        //}


                        //var contractorList = await _tenantRepository.GetAllContractor(tenantDetails.TenantId);
                        //if (contractorList != null)
                        //{
                        //    model.ContractorList = contractorList.Select(h => new SelectListItem
                        //    {
                        //        Text = h.Name,
                        //        Value = h.ContractorId.ToString()
                        //    });
                        //}

                        //var departmentList = await _tenantRepository.GetAllDepartment(tenantDetails.TenantId);
                        //if (departmentList != null)
                        //{
                        //    model.DepartmentList = departmentList.Select(h => new SelectListItem
                        //    {
                        //        Text = h.Name,
                        //        Value = h.DepartmentId.ToString()
                        //    });
                        //}

                        var futurePreventionList = await _tenantRepository.GetAllFuturePrevention(tenantDetails.TenantId);
                        if (futurePreventionList != null)
                        {
                            model.FuturePreventionList = futurePreventionList.Select(h => new SelectListItem
                            {
                                Text = h.Name,
                                Value = h.FuturePreventionId.ToString()
                            });
                        }

                        //var injuredPersonCategoryList = await _tenantRepository.GetAllInjuredPersonCategory(tenantDetails.TenantId);
                        //if (injuredPersonCategoryList != null)
                        //{
                        //    model.InjuredPersonCategoryList = injuredPersonCategoryList.Select(h => new SelectListItem
                        //    {
                        //        Text = h.Name,
                        //        Value = h.InjuredPersonCategoryId.ToString()
                        //    });
                        //}

                        var injuryTypeList = await _tenantRepository.GetAllInjuryType(tenantDetails.TenantId);
                        if (injuryTypeList != null)
                        {
                            model.InjuryTypeList = injuryTypeList.Select(h => new SelectListItem
                            {
                                Text = h.Name,
                                Value = h.InjuryTypeId.ToString()
                            });
                        }

                        //var anticipatedAbsencesList = await _tenantRepository.GetAllAnticipatedAbsences(tenantDetails.TenantId);
                        //if (anticipatedAbsencesList != null)
                        //{
                        //    model.AnticipatedAbsenceList = anticipatedAbsencesList.Select(h => new SelectListItem
                        //    {
                        //        Text = h.Name,
                        //        Value = h.AnticipatedAbsenceId.ToString()
                        //    });
                        //}

                        //var incidentResultList = await _tenantRepository.GetAllIncidentResult(tenantDetails.TenantId);
                        //if (incidentResultList != null)
                        //{
                        //    model.IncidentResultList = incidentResultList.Select(h => new SelectListItem
                        //    {
                        //        Text = h.Name,
                        //        Value = h.IncidentResultId.ToString()
                        //    });
                        //}


                        model.IncidentBodyPartId = model.IncidentBodyParts?.Select(id => id.BodyPartId).ToArray();
                        model.IncidentFuturePreventionId = model.IncidentFuturePreventions?.Select(id => id.FuturePreventionId).ToArray();
                        model.IncidentTaskCarriedId = model.IncidentTaskCarrieds?.Select(id => id.TaskCarriedId).ToArray();
                        model.IncidentInjuryTypeId = model.IncidentInjuryTypes?.Select(id => id.InjuryTypeId).ToArray();


                        return View(model);
                    }
                    else
                    {
                        return View("TenantError", tenant);
                    }
                }
            }
            catch (Microsoft.Azure.SqlDatabase.ElasticScale.ShardManagement.ShardManagementException ex)
            {
                if (ex.ErrorCode == Microsoft.Azure.SqlDatabase.ElasticScale.ShardManagement.ShardManagementErrorCode.MappingIsOffline)
                {
                    var tenantModel = await _catalogRepository.GetTenant(tenant);
                    _logger.LogInformation(0, ex, "Tenant is offline: {tenant}", tenantModel.TenantName);
                    return View("TenantOffline", tenantModel.TenantName);
                }
                else if (ex.ErrorCode == Microsoft.Azure.SqlDatabase.ElasticScale.ShardManagement.ShardManagementErrorCode.MappingDoesNotExist)
                {
                    var tenantModel = await _catalogRepository.GetTenant(tenant);
                    //Fix mapping irregularities
                    _utilities.ResolveMappingDifferences(tenantModel.TenantId);

                    //Get venue details
                    String tenantStatus = _utilities.GetTenantStatus(tenantModel.TenantId);
                   
                        return View("TenantOffline", tenantModel.TenantName);
                    
                }
                else
                {
                    _logger.LogError(0, ex, "Tenant shard was unavailable for tenant: {tenant}", tenant);
                    return View("Error", ex.Message);
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
        [Route("{tenant}/hazardincidents/update")]
        public async Task<ActionResult> Update(HazardIncidentViewModel model, string tenant, string submit)
        {
            try
            {
                if (!string.IsNullOrEmpty(tenant))
                {
                    var tenantDetails = await _catalogRepository.GetTenant(tenant);
                    if (tenantDetails != null)
                    {
                        if (model.StatusOfHazardId == 2 || model.IncidentStatusOfHazardId == 2)
                        {
                            model.DateOfCompletion = null;
                            model.IncidentDateOfCompletion = null;
                        }

                        model.ModifiedBy = GetUser.Employee.RowId;
                        model.ModifiedOn = DateTime.Now;
                        await _tenantRepository.UpdateHazardIncident(model, tenantDetails.TenantId, submit);
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


        [Route("{tenant}/hazardincidents/investigation/update")]
        public async Task<ActionResult> InvestigationUpdate(string id, string tenant)
        {
            try
            {
                if (!string.IsNullOrEmpty(tenant))
                {
                    var tenantDetails = await _catalogRepository.GetTenant(tenant);
                    if (tenantDetails != null)
                    {

                        var hazardIncident = await _tenantRepository.GetHazardIncident(new Guid(id), tenantDetails.TenantId);
                        HazardIncidentViewModel model = _mapper.Map<HazardIncidentViewModel>(hazardIncident);

                       

                        var tasksCarriedList = await _tenantRepository.GetAllTaskCarried(tenantDetails.TenantId);
                        if (tasksCarriedList != null)
                        {
                            model.TasksCarriedList = tasksCarriedList.Select(h => new SelectListItem
                            {
                                Text = h.Name,
                                Value = h.TaskCarriedId.ToString()
                            });
                        }




                        var bodyPartList = await _tenantRepository.GetAllBodyPart(tenantDetails.TenantId);
                        if (bodyPartList != null)
                        {
                            model.BodyPartList = bodyPartList.Select(h => new SelectListItem
                            {
                                Text = h.Name,
                                Value = h.BodyPartId.ToString()
                            });
                        }


                       
                        var futurePreventionList = await _tenantRepository.GetAllFuturePrevention(tenantDetails.TenantId);
                        if (futurePreventionList != null)
                        {
                            model.FuturePreventionList = futurePreventionList.Select(h => new SelectListItem
                            {
                                Text = h.Name,
                                Value = h.FuturePreventionId.ToString()
                            });
                        }

                        

                        var injuryTypeList = await _tenantRepository.GetAllInjuryType(tenantDetails.TenantId);
                        if (injuryTypeList != null)
                        {
                            model.InjuryTypeList = injuryTypeList.Select(h => new SelectListItem
                            {
                                Text = h.Name,
                                Value = h.InjuryTypeId.ToString()
                            });
                        }




                        var personnelList = await _tenantRepository.GetAllPersonnel(tenantDetails.TenantId);
                        if (personnelList != null)
                        {
                            model.PersonnelList = personnelList.Select(h => new SelectListItem
                            {
                                Text = h.Name,
                                Value = h.PersonnelId.ToString()
                            });
                        }

                        var rootCauseList = await _tenantRepository.GetAllRootCause(tenantDetails.TenantId);
                        if (rootCauseList != null)
                        {
                            model.RootCauseList = rootCauseList.Select(h => new SelectListItem
                            {
                                Text = h.Name,
                                Value = h.RootCauseId.ToString()
                            });
                        }


                        var correctMeasureList = await _tenantRepository.GetAllCorrectiveMeasure(tenantDetails.TenantId);
                        if (correctMeasureList != null)
                        {
                            model.CorrectiveMeasureList = correctMeasureList.Select(h => new SelectListItem
                            {
                                Text = h.Name,
                                Value = h.CorrectiveMeasureId.ToString()
                            });
                        }

                        var processEnvironmentList = await _tenantRepository.GetAllProcessEnvironment(tenantDetails.TenantId);
                        if (processEnvironmentList != null)
                        {
                            model.ProcessEnvironmentList = processEnvironmentList.Select(h => new SelectListItem
                            {
                                Text = h.Name,
                                Value = h.ProcessEnvironmentId.ToString()
                            });
                        }


                        var medicalEvaluationList = await _tenantRepository.GetAllMedicalEvaluation(tenantDetails.TenantId);
                        if (medicalEvaluationList != null)
                        {
                            model.MedicalEvaluationList = medicalEvaluationList.Select(h => new SelectListItem
                            {
                                Text = h.Name,
                                Value = h.MedicalEvaluationId.ToString()
                            });
                        }

                        model.Investigation = model.Investigation ?? new HazardIncidentInvestigationModel() { HazardIncidentId = model.HazardIncidentId};
                        model.Investigation.EmployeeId= GetUser.Employee.EmployeeId;
                        model.Investigation.EmployeeName = GetUser.Employee.Name;
                        model.Investigation.EmployeeDesignation = GetUser.Employee?.Designation;
                        model.Investigation.EmployeeManager = GetUser.Employee?.Manager;

                        model.IncidentBodyPartId = model.IncidentBodyParts?.Select(id => id.BodyPartId).ToArray();
                        model.IncidentFuturePreventionId = model.IncidentFuturePreventions?.Select(id => id.FuturePreventionId).ToArray();
                        model.IncidentTaskCarriedId = model.IncidentTaskCarrieds?.Select(id => id.TaskCarriedId).ToArray();
                        model.IncidentInjuryTypeId = model.IncidentInjuryTypes?.Select(id => id.InjuryTypeId).ToArray();

                        if (model.Investigation?.HazardIncidentInvestigationId > 0)
                        {
                            model.Investigation.CorrectiveMeasureId = model.Investigation.CorrectiveMeasures.Select(id => id.CorrectiveMeasureId).ToArray();
                            model.Investigation.RootCausesId = model.Investigation.RootCauses.Select(id => id.RootCauseId).ToArray();
                            model.Investigation.ProcessEnvironmentId = model.Investigation.ProcessEnvironment.Select(id => id.ProcessEnvironmentId).ToArray();
                            model.Investigation.PersonnelId = model.Investigation.Personnel.Select(id => id.PersonnelId).ToArray();
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
        [Route("{tenant}/hazardincidents/investigation/update")]
        public async Task<ActionResult> InvestigationUpdate(HazardIncidentViewModel model, string tenant, string submit)
        {
            try
            {
                if (!string.IsNullOrEmpty(tenant))
                {
                    var tenantDetails = await _catalogRepository.GetTenant(tenant);
                    if (tenantDetails != null)
                    {
                        model.Investigation.ModifiedBy = GetUser.Employee.RowId;
                        model.Investigation.ModifiedOn = DateTime.Now;
                        model.Investigation.CreatedBy = GetUser.Employee.RowId;
                        model.Investigation.CreatedOn = DateTime.Now;
                        await _tenantRepository.UpdateInvestigation(model.Investigation, tenantDetails.TenantId, submit);
                    }
                }
                return RedirectToAction("Investigation");
            }            
            catch (Exception ex)
            {
                _logger.LogError(0, ex, "Get events failed for tenant {tenant}", tenant);
                return View("Error", ex.Message);
            }
            return View("Error");
        }

        private UserModel GetUser
        {
            get { return _httpContextAccessor.HttpContext.Session.GetObjectFromJson<List<UserModel>>("SessionUsers").Last(); }
        }

    }
}

