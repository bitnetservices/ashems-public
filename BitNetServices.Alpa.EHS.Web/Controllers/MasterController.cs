using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BitNetServices.Alpa.EHS.Common.Interfaces;
using BitNetServices.Alpa.EHS.Common.Model.Catalog;
using BitNetServices.Alpa.EHS.Common.Model.Tenant.Master;
using BitNetServices.Alpa.EHS.Web.Filter;
using BitNetServices.Alpa.EHS.Web.Tenant.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace BitNetServices.Alpa.EHS.Web.Controllers
{
    [SessionExpire]
    [AuthorizedTenantOnly]
    public class MasterController : BaseController
    {
        #region Fields

        private readonly ITenantRepository _tenantRepository;
        private readonly IStringLocalizer<AccountController> _localizer;
        private readonly ICatalogRepository _catalogRepository;
        private readonly ILogger _logger;
        private readonly DnsClient.ILookupClient _client;
        private readonly Microsoft.AspNetCore.Http.IHttpContextAccessor _httpContextAccessor;
        #endregion

        private UserModel GetUser
        {
            get { return _httpContextAccessor.HttpContext.Session.GetObjectFromJson<List<UserModel>>("SessionUsers").Last(); }
        }


        #region Constructors

        public MasterController(IStringLocalizer<AccountController> localizer, IStringLocalizer<BaseController> baseLocalizer, ITenantRepository tenantRepository, ICatalogRepository catalogRepository, ILogger<AccountController> logger, IConfiguration configuration,
            DnsClient.ILookupClient client, Microsoft.AspNetCore.Http.IHttpContextAccessor httpContextAccessor)
            : base(baseLocalizer, tenantRepository, configuration, client)
        {
            _localizer = localizer;
            _tenantRepository = tenantRepository;
            _catalogRepository = catalogRepository;
            _logger = logger;
            _client = client;
            _httpContextAccessor = httpContextAccessor;
        }

        #endregion

        [Route("{tenant}/master")]
        public async Task<ActionResult> Index(string tenant)
        {
            MasterViewModel model = new MasterViewModel();
            if (!string.IsNullOrEmpty(tenant))
            {
                var tenantDetails = await _catalogRepository.GetTenant(tenant);
                if (tenantDetails != null)
                {
                    model.HazardCateogries = await _tenantRepository.GetAllHazardCategory(tenantDetails.TenantId);
                    model.HazardTypes = await _tenantRepository.GetAllHazardType(tenantDetails.TenantId);
                    model.HazardStatuses = await _tenantRepository.GetAllHazardStatus(tenantDetails.TenantId);
                    model.Departments = await _tenantRepository.GetAllDepartment(tenantDetails.TenantId);
                    model.Employees = await _tenantRepository.GetAllEmployee(tenantDetails.TenantId);
                    model.Locations = await _tenantRepository.GetAllLocation(tenantDetails.TenantId);
                    model.AreaOrBuildings = await _tenantRepository.GetAllAreaOrBuilding(tenantDetails.TenantId);                    
                    if (tenant == "alpagroup")
                    {

                        model.Users = await _catalogRepository.GetAllUsers();
                    }
                    else
                    {
                        model.Users = await _catalogRepository.GetAllUsers(tenantDetails.TenantId);
                        foreach (UserModel user in model.Users )
                        {
                            user.SecurityRole = await _tenantRepository.GetSecurityRole(user.UserId, tenantDetails.TenantId);
                        }


                    }                    
                }
            }
            return View(model);
        }

        [Route("{tenant}/department/add")]
        public ActionResult CreateDepartment(string tenant)
        {
            return View("_Department");
        }

        [Route("{tenant}/hazardcategory/add")]
        public ActionResult CreateHazardCategory(string tenant)
        {
            return View("_HazardCategory");
        }

        [Route("{tenant}/hazardstatus/add")]
        public ActionResult CreateHazardStatus(string tenant)
        {
            return View("_HazardStatus");
        }

        [Route("{tenant}/employee/add")]
        public async Task<IActionResult> CreateEmployee(string tenant)
        {
            if (!string.IsNullOrEmpty(tenant))
            {
                var tenantDetails = await _catalogRepository.GetTenant(tenant);
                if (tenantDetails != null)
                {
                    var model = new EmployeeViewModel();
                    var employeeList = await _tenantRepository.GetAllEmployee(tenantDetails.TenantId);
                    var excludeIds = new HashSet<Guid>(employeeList.Select(x => x.UserId));

                    var departmentList = await _tenantRepository.GetAllDepartment(tenantDetails.TenantId);
                    if (departmentList != null)
                    {
                        model.DepartmentList = departmentList.Select(h => new SelectListItem
                        {
                            Text = h.Name,
                            Value = h.DepartmentId.ToString()
                        });
                    }

                    var employmenttype = await _tenantRepository.GetAllEmploymentType(tenantDetails.TenantId);
                    if (employmenttype != null)
                    {
                        model.EmploymentTypeList = employmenttype.Select(h => new SelectListItem
                        {
                            Text = h.Name,
                            Value = h.EmploymentTypeId.ToString()
                        }); ;
                    }
                    var userList = await _catalogRepository.GetAllUsers(tenantDetails.TenantId);
                    userList = userList.Where(where => !excludeIds.Contains(where.UserId)).ToList();
                    if (userList != null)
                    {
                        model.UserList = userList.Select(h => new SelectListItem
                        {
                            Text = h.FirstName + ' ' + h.LastName,
                            Value = h.UserId.ToString()
                        });
                    }
                    return View("_Employee", model);
                }
            }
            return View("_Employee");
        }

        [Route("{tenant}/hazardtype/add")]
        public ActionResult CreateHazardType(string tenant)
        {
            return View("_HazardType");
        }

        [Route("{tenant}/location/add")]
        public ActionResult CreateLocation(string tenant)
        {
            return View("_Location");
        }

        [Route("{tenant}/areaorbuilding/add")]
        public async Task<ActionResult> CreateAreaOrBuilding(string tenant)
        {
            if (!string.IsNullOrEmpty(tenant))
            {
                var tenantDetails = await _catalogRepository.GetTenant(tenant);
                if (tenantDetails != null)
                {
                    var model = new AreaOrBuildingViewModel();

                    var location = await _tenantRepository.GetAllLocation(tenantDetails.TenantId);
                    if (location != null)
                    {
                        model.LocationList = location.Select(h => new SelectListItem
                        {
                            Text = h.Name,
                            Value = h.LocationId.ToString()
                        }); ;
                    }

                    return View("_AreaOrBuilding", model);
                }
            }
            return View("_AreaOrBuilding");
        }

        [Route("{tenant}/user/add")]
        public async Task<ActionResult> CreateUser()
        {
            var model = new UserViewModel();
            var tenants = await _catalogRepository.GetAllTenants();
            if (tenants != null)
            {
                model.TenantList = tenants.Select(h => new SelectListItem
                {
                    Text = h.TenantName,
                    Value = h.TenantId.ToString()
                });
            }

            return View("_User", model);
        }
        [HttpPost]
        [Route("{tenant}/user/add")]
        public new async Task<ActionResult> User(UserViewModel model)
        {

            string url = "/";
            try
            {
                var tenantUsers = await _catalogRepository.GetAllUsers(model.TenantId);
                if (model.UserId == Guid.Empty)
                {
                    var user = tenantUsers.Where(where => where.Email == model.Email || where.LoginName == model.LoginName);
                    if (user.Count() == 0)
                    {
                        model.IsActive = true;
                        var guid = await _catalogRepository.AddUser(model, model.TenantId);

                        var securityRoles = await _tenantRepository.GetAllSecurityRoles(model.TenantId);
                        var adminRole = securityRoles.Where(where => where.Name == "Admin").FirstOrDefault();

                        await _tenantRepository.AddEmployee(new EmployeeModel()
                        {
                            CreatedOn = DateTime.Now,
                            Email = model.Email,
                            UserId = guid,
                            Name = model.FirstName + " " + model.LastName,
                            DateofJoining = DateTime.MinValue,
                            Manager = String.Empty,
                            Designation = String.Empty,
                            IsUser = true,
                            IsEditable = true,
                            DepartmentId = model.DepartmentId,
                            IsActive = true,
                        }, model.TenantId);

                        if (model.IsAdmin && adminRole != null)
                        {
                            await _tenantRepository.AddUserSecurityRole(new UserSecurityRoleModel()
                            {
                                CreatedOn = DateTime.Now,
                                UserId = guid,
                                SecurityRoleId = adminRole.RowId,
                                IsActive = true,
                            }, model.TenantId);
                        }                        
                    }
                    else
                    {
                        ViewBag.ErrorMessage = "User with same email or loginname already exists";
                        return View("_User", model);
                    }
                }
                else
                {
                    var user = tenantUsers.Where(where => (where.Email == model.Email || where.LoginName == model.LoginName) &&  model.UserId != where.UserId);
                    if (user.Count() > 0)
                    {
                        ViewBag.ErrorMessage = "User with same email or loginname already exists";
                        return View("_User", model);
                    }

                    await _catalogRepository.AddUser(model, model.TenantId);

                    await _tenantRepository.UpdateEmployee(new EmployeeModel()
                    {
                        ModifiedOn = DateTime.Now,
                        Email = model.Email,
                        UserId = model.UserId,
                        IsUser = true,
                    }, model.TenantId);
                    if (!string.IsNullOrEmpty(model.SecurityRoleId))
                    {
                        await _tenantRepository.AddUserSecurityRole(new UserSecurityRoleModel()
                        {
                            CreatedOn = DateTime.Now,
                            UserId = model.UserId,
                            SecurityRoleId = Guid.Parse(model.SecurityRoleId),
                            IsActive = true,
                        }, model.TenantId);
                    }


                    var securityRoles = await _tenantRepository.GetAllSecurityRoles(model.TenantId);
                    var adminRole = securityRoles.Where(where => where.Name == "Admin").FirstOrDefault();

                    if (model.IsAdmin && adminRole != null)
                    {
                        await _tenantRepository.AddUserSecurityRole(new UserSecurityRoleModel()
                        {
                            CreatedOn = DateTime.Now,
                            UserId = model.UserId,
                            SecurityRoleId = adminRole.RowId,
                            IsActive = true,
                        }, model.TenantId);
                    }
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(0, ex, "Login failed for user {regEmail}", model.Email);
                return View("_User", model);
            }
            return Redirect(url);
        }


        [Route("{tenant}/user")]
        public new async Task<ActionResult> User(string id, string tenant)
        {
            var models = new List<UserModel>();
            if (tenant == "alpagroup")
            {
            
                models = await _catalogRepository.GetAllUsers();
            }
            else
            {
                var tenantId = await _catalogRepository.GetTenant(tenant);
                models = await _catalogRepository.GetAllUsers(tenantId.TenantId);

            }
           
            var user = models.Where(where => where.UserId == Guid.Parse(id)).FirstOrDefault();
            var serialised = JsonConvert.SerializeObject(user);
            var model = JsonConvert.DeserializeObject<UserViewModel>(serialised);
            if (tenant == "alpagroup")
            {
                var tenants = await _catalogRepository.GetAllTenants();
                if (tenants != null)
                {
                    model.TenantList = tenants.Select(h => new SelectListItem
                    {
                        Text = h.TenantName,
                        Value = h.TenantId.ToString()
                    });
                }
            }

            if (tenant != "alpagroup")
            {
                var tenantDetails = await _catalogRepository.GetTenant(tenant);
                var securityRoles = await _tenantRepository.GetAllSecurityRoles(tenantDetails.TenantId);
                if (securityRoles != null)
                {
                    model.SecurityRoleList = securityRoles.Select(h => new SelectListItem
                    {
                        Text = h.Name,
                        Value = h.RowId.ToString()
                    });
                }

                model.SecurityRole = await _tenantRepository.GetSecurityRole(user.UserId, tenantDetails.TenantId);
                model.SecurityRoleId = model.SecurityRole?.RowId.ToString();
            }

            return PartialView("_User", model);
        }


        [Route("{tenant}/department")]
        public async Task<ActionResult> Department(string id, string tenant)
        {

            string url = "/";
            try
            {
                var tenantDetails = await _catalogRepository.GetTenant(tenant);
                if (tenantDetails != null)
                {

                    var models = await _tenantRepository.GetAllDepartment(tenantDetails.TenantId);
                    var model = models.Where(where => where.RowId == Guid.Parse(id)).FirstOrDefault();
                    return PartialView("_Department", model);
                }
                else
                {
                    var message = _localizer["The user does not exist."];
                    DisplayMessage(message, "Error");
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(0, ex, "Email", "");
                return View();
            }
            return Redirect(url);
        }

        [HttpPost]
        [Route("{tenant}/department")]
        public async Task<ActionResult> Department(DepartmentModel model, string tenant)
        {
            string url = "/";
            try
            {
                var tenantDetails = await _catalogRepository.GetTenant(tenant);
                if (tenantDetails != null)
                {
                    if (model.DepartmentId != 0)
                    {
                        model.ModifiedBy = GetUser.UserId;
                        await _tenantRepository.UpdateDepartment(model, tenantDetails.TenantId);
                    }
                    else
                    {
                        model.CreatedBy = GetUser.UserId;
                        await _tenantRepository.AddDepartment(model, tenantDetails.TenantId);
                    }
                    return RedirectToAction("Index");
                }
                else
                {
                    var message = _localizer["The user does not exist."];
                    DisplayMessage(message, "Error");
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(0, ex, "Email", "");
                return PartialView(model);
            }
            return Redirect(url);
        }

        [Route("{tenant}/hazardcategory")]
        public async Task<ActionResult> HazardCategory(string id, string tenant)
        {

            string url = "/";
            try
            {
                var tenantDetails = await _catalogRepository.GetTenant(tenant);
                if (tenantDetails != null)
                {

                    var models = await _tenantRepository.GetAllHazardCategory(tenantDetails.TenantId);
                    var model = models.Where(where => where.RowId == Guid.Parse(id)).FirstOrDefault();
                    return PartialView("_HazardCategory", model);
                }
                else
                {
                    var message = _localizer["The user does not exist."];
                    DisplayMessage(message, "Error");
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(0, ex, "Email", "");
                return View();
            }
            return Redirect(url);
        }

        [HttpPost]
        [Route("{tenant}/hazardcategory")]
        public async Task<ActionResult> HazardCategory(HazardCategoryModel model, string tenant)
        {

            string url = "/";
            try
            {
                var tenantDetails = await _catalogRepository.GetTenant(tenant);
                if (tenantDetails != null)
                {
                    if (model.HazardCategoryId != 0)
                    {
                        model.ModifiedBy = GetUser.UserId;
                        await _tenantRepository.UpdateHazardCategory(model, tenantDetails.TenantId);
                    }
                    else
                    {
                        model.CreatedBy = GetUser.UserId;
                        await _tenantRepository.AddHazardCategory(model, tenantDetails.TenantId);
                    }
                    return RedirectToAction("Index");
                }
                else
                {
                    var message = _localizer["The user does not exist."];
                    DisplayMessage(message, "Error");
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(0, ex, "Email", "");
                return PartialView(model);
            }
            return Redirect(url);
        }

        [Route("{tenant}/hazardstatus")]
        public async Task<ActionResult> HazardStatus(string id, string tenant)
        {

            string url = "/";
            try
            {
                var tenantDetails = await _catalogRepository.GetTenant(tenant);
                if (tenantDetails != null)
                {

                    var models = await _tenantRepository.GetAllHazardStatus(tenantDetails.TenantId);
                    var model = models.Where(where => where.RowId == Guid.Parse(id)).FirstOrDefault();
                    return PartialView("_HazardStatus", model);
                }
                else
                {
                    var message = _localizer["The user does not exist."];
                    DisplayMessage(message, "Error");
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(0, ex, "Email", "");
                return View();
            }
            return Redirect(url);
        }

        [HttpPost]
        [Route("{tenant}/hazardstatus")]
        public async Task<ActionResult> HazardStatus(HazardStatusModel model, string tenant)
        {

            string url = "/";
            try
            {
                var tenantDetails = await _catalogRepository.GetTenant(tenant);
                if (tenantDetails != null)
                {
                    if (model.HazardStatusId != 0)
                    {
                        model.ModifiedBy = GetUser.UserId;
                        await _tenantRepository.UpdateHazardStatus(model, tenantDetails.TenantId);
                    }
                    else
                    {
                        model.CreatedBy = GetUser.UserId;
                        await _tenantRepository.AddHazardStatus(model, tenantDetails.TenantId);
                    }
                    return RedirectToAction("Index");
                }
                else
                {
                    var message = _localizer["The user does not exist."];
                    DisplayMessage(message, "Error");
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(0, ex, "Email", "");
                return PartialView(model);
            }
            return Redirect(url);
        }

        [Route("{tenant}/employee")]
        public async Task<ActionResult> Employee(string id, string tenant)
        {

            string url = "/";
            try
            {
                var tenantDetails = await _catalogRepository.GetTenant(tenant);
                if (tenantDetails != null)
                {

                    var models = await _tenantRepository.GetAllEmployee(tenantDetails.TenantId);
                    var employee = models.Where(where => where.RowId == Guid.Parse(id)).FirstOrDefault();
                    var serialised = JsonConvert.SerializeObject(employee);
                    var model = JsonConvert.DeserializeObject<EmployeeViewModel>(serialised);

                    var employeeList = await _tenantRepository.GetAllEmployee(tenantDetails.TenantId);
                    var excludeIds = new HashSet<Guid>(employeeList.Where(where => where.EmployeeId != model.EmployeeId).Select(x => x.UserId));

                    var departmentList = await _tenantRepository.GetAllDepartment(tenantDetails.TenantId);
                    if (departmentList != null)
                    {
                        model.DepartmentList = departmentList.Select(h => new SelectListItem
                        {
                            Text = h.Name,
                            Value = h.DepartmentId.ToString()
                        });
                    }

                    var employmenttype = await _tenantRepository.GetAllEmploymentType(tenantDetails.TenantId);
                    if (employmenttype != null)
                    {
                        model.EmploymentTypeList = employmenttype.Select(h => new SelectListItem
                        {
                            Text = h.Name,
                            Value = h.EmploymentTypeId.ToString()
                        }); ;
                    }
                    var userList = await _catalogRepository.GetAllUsers(tenantDetails.TenantId);
                    userList.Remove(userList.Where(where => where.UserId == model.UserId).FirstOrDefault());
                    userList = userList.Where(where => !excludeIds.Contains(where.UserId)).ToList();
                    if (userList != null)
                    {
                        model.UserList = userList.Select(h => new SelectListItem
                        {
                            Text = h.FirstName + ' ' + h.LastName,
                            Value = h.UserId.ToString()
                        });
                    }
                    return PartialView("_Employee", model);
                }
                else
                {
                    var message = _localizer["The user does not exist."];
                    DisplayMessage(message, "Error");
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(0, ex, "Email", "");
                return View();
            }
            return Redirect(url);
        }

        [HttpPost]
        [Route("{tenant}/employee")]
        public async Task<ActionResult> Employee(EmployeeViewModel model, string tenant)
        {

            string url = "/";
            try
            {
                var tenantDetails = await _catalogRepository.GetTenant(tenant);
                if (tenantDetails != null)
                {
                    if (model.EmployeeId != 0)
                    {
                        model.ModifiedBy = GetUser.UserId;
                        await _tenantRepository.UpdateEmployee(model, tenantDetails.TenantId);
                    }
                    else
                    {
                        model.CreatedBy = GetUser.UserId;
                        await _tenantRepository.AddEmployee(model, tenantDetails.TenantId);
                    }
                    return RedirectToAction("Index");
                }
                else
                {
                    var message = _localizer["The user does not exist."];
                    DisplayMessage(message, "Error");
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(0, ex, "Email", "");
                return PartialView(model);
            }
            return Redirect(url);
        }


        [Route("{tenant}/hazardtype")]
        public async Task<ActionResult> HazardType(string id, string tenant)
        {

            string url = "/";
            try
            {
                var tenantDetails = await _catalogRepository.GetTenant(tenant);
                if (tenantDetails != null)
                {

                    var models = await _tenantRepository.GetAllHazardType(tenantDetails.TenantId);
                    var model = models.Where(where => where.RowId == Guid.Parse(id)).FirstOrDefault();
                    return PartialView("_HazardType", model);
                }
                else
                {
                    var message = _localizer["The user does not exist."];
                    DisplayMessage(message, "Error");
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(0, ex, "Email", "");
                return View();
            }
            return Redirect(url);
        }

        [HttpPost]
        [Route("{tenant}/hazardtype")]
        public async Task<ActionResult> HazardType(HazardTypeModel model, string tenant)
        {

            string url = "/";
            try
            {
                var tenantDetails = await _catalogRepository.GetTenant(tenant);
                if (tenantDetails != null)
                {
                    if (model.HazardTypeId != 0)
                    {
                        model.ModifiedBy = GetUser.UserId;
                        await _tenantRepository.UpdateHazardType(model, tenantDetails.TenantId);
                    }
                    else
                    {
                        model.CreatedBy = GetUser.UserId;
                        await _tenantRepository.AddHazardType(model, tenantDetails.TenantId);
                    }
                    return RedirectToAction("Index");
                }
                else
                {
                    var message = _localizer["The user does not exist."];
                    DisplayMessage(message, "Error");
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(0, ex, "Email", "");
                return PartialView(model);
            }
            return Redirect(url);
        }

        [Route("{tenant}/location")]
        public async Task<ActionResult> Location(string id, string tenant)
        {

            string url = "/";
            try
            {
                var tenantDetails = await _catalogRepository.GetTenant(tenant);
                if (tenantDetails != null)
                {

                    var models = await _tenantRepository.GetAllLocation(tenantDetails.TenantId);
                    var model = models.Where(where => where.RowId == Guid.Parse(id)).FirstOrDefault();
                    return PartialView("_Location", model);
                }
                else
                {
                    var message = _localizer["The user does not exist."];
                    DisplayMessage(message, "Error");
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(0, ex, "Email", "");
                return View();
            }
            return Redirect(url);
        }

        [HttpPost]
        [Route("{tenant}/location")]
        public async Task<ActionResult> Location(LocationModel model, string tenant)
        {

            string url = "/";
            try
            {
                var tenantDetails = await _catalogRepository.GetTenant(tenant);
                if (tenantDetails != null)
                {
                    if (model.LocationId != 0)
                    {
                        model.ModifiedBy = GetUser.UserId;
                        await _tenantRepository.UpdateLocation(model, tenantDetails.TenantId);
                    }
                    else
                    {
                        model.CreatedBy = GetUser.UserId;
                        await _tenantRepository.AddLocation(model, tenantDetails.TenantId);
                    }
                    return RedirectToAction("Index");
                }
                else
                {
                    var message = _localizer["The user does not exist."];
                    DisplayMessage(message, "Error");
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(0, ex, "Email", "");
                return PartialView(model);
            }
            return Redirect(url);
        }

        [Route("{tenant}/areaorbuilding")]
        public async Task<ActionResult> AreaOrBuilding(string id, string tenant)
        {

            string url = "/";
            try
            {
                var tenantDetails = await _catalogRepository.GetTenant(tenant);
                if (tenantDetails != null)
                {

                    var models = await _tenantRepository.GetAllAreaOrBuilding(tenantDetails.TenantId);
                    var areaOrBuiding = models.Where(where => where.RowId == Guid.Parse(id)).FirstOrDefault();
                    var serialised = JsonConvert.SerializeObject(areaOrBuiding);
                    var model = JsonConvert.DeserializeObject<AreaOrBuildingViewModel>(serialised);
                    var location = await _tenantRepository.GetAllLocation(tenantDetails.TenantId);
                    if (location != null)
                    {
                        model.LocationList = location.Select(h => new SelectListItem
                        {
                            Text = h.Name,
                            Value = h.LocationId.ToString()
                        }); ;
                    }
                    return PartialView("_AreaOrBuilding", model);
                }
                else
                {
                    var message = _localizer["The user does not exist."];
                    DisplayMessage(message, "Error");
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(0, ex, "Email", "");
                return View();
            }
            return Redirect(url);
        }

        [HttpPost]
        [Route("{tenant}/areaorbuilding")]
        public async Task<ActionResult> EditAreaOrBuilding(AreaOrBuildingModel model, string tenant)
        {

            string url = "/";
            try
            {
                var tenantDetails = await _catalogRepository.GetTenant(tenant);
                if (tenantDetails != null)
                {
                    if (model.AreaOrBuildingId != 0)
                    {
                        model.ModifiedBy = GetUser.UserId;
                        await _tenantRepository.UpdateAreaOrBuilding(model, tenantDetails.TenantId);
                    }
                    else
                    {
                        model.CreatedBy = GetUser.UserId;
                        await _tenantRepository.AddAreaOrBuilding(model, tenantDetails.TenantId);
                    }
                    return RedirectToAction("Index");
                }
                else
                {
                    var message = _localizer["The user does not exist."];
                    DisplayMessage(message, "Error");
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(0, ex, "Email", "");
                return PartialView(model);
            }
            return Redirect(url);
        }

        [HttpPost]
        [Route("{tenant}/areaorbuilding/remove")]
        public async Task<ActionResult> RemoveAreaOrBuilding(int id, string tenant)
        {

            string url = "/";
            try
            {
                var tenantDetails = await _catalogRepository.GetTenant(tenant);
                if (tenantDetails != null)
                {

                    var model = await _tenantRepository.GetAreaOrBuilding(id, tenantDetails.TenantId);
                    model.ModifiedBy = GetUser.UserId;
                    model.IsDeleted = true;
                    await _tenantRepository.UpdateAreaOrBuilding(model, tenantDetails.TenantId);
                    return RedirectToAction("Index");
                }
                else
                {
                    var message = _localizer["The user does not exist."];
                    DisplayMessage(message, "Error");
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(0, ex, "Email", "");
                return PartialView();
            }
            return Redirect(url);
        }

        [HttpPost]
        [Route("{tenant}/hazardcategory/remove")]
        public async Task<ActionResult> RemoveHazardCategory(int id, string tenant)
        {

            string url = "/";
            try
            {
                var tenantDetails = await _catalogRepository.GetTenant(tenant);
                if (tenantDetails != null)
                {

                    var model = await _tenantRepository.GetHazardCategory(id, tenantDetails.TenantId);
                    model.ModifiedBy = GetUser.UserId;
                    model.IsDeleted = true;
                    await _tenantRepository.UpdateHazardCategory(model, tenantDetails.TenantId);
                    return RedirectToAction("Index");
                }
                else
                {
                    var message = _localizer["The user does not exist."];
                    DisplayMessage(message, "Error");
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(0, ex, "Email", "");
                return PartialView();
            }
            return Redirect(url);
        }

        [HttpPost]
        [Route("{tenant}/employee/remove")]
        public async Task<ActionResult> RemoveEmployee(int id, string tenant)
        {

            string url = "/";
            try
            {
                var tenantDetails = await _catalogRepository.GetTenant(tenant);
                if (tenantDetails != null)
                {

                    var model = await _tenantRepository.GetEmployee(id, tenantDetails.TenantId);
                    model.ModifiedBy = GetUser.UserId;
                    model.IsDeleted = true;
                    await _tenantRepository.UpdateEmployee(model, tenantDetails.TenantId);
                    return RedirectToAction("Index");
                }
                else
                {
                    var message = _localizer["The user does not exist."];
                    DisplayMessage(message, "Error");
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(0, ex, "Email", "");
                return PartialView();
            }
            return Redirect(url);
        }


        [HttpPost]
        [Route("{tenant}/hazardstatus/remove")]
        public async Task<ActionResult> RemoveHazardStatus(int id, string tenant)
        {

            string url = "/";
            try
            {
                var tenantDetails = await _catalogRepository.GetTenant(tenant);
                if (tenantDetails != null)
                {

                    var model = await _tenantRepository.GetHazardStatus(id, tenantDetails.TenantId);
                    model.ModifiedBy = GetUser.UserId;
                    model.IsDeleted = true;
                    await _tenantRepository.UpdateHazardStatus(model, tenantDetails.TenantId);
                    return RedirectToAction("Index");
                }
                else
                {
                    var message = _localizer["The user does not exist."];
                    DisplayMessage(message, "Error");
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(0, ex, "Email", "");
                return PartialView();
            }
            return Redirect(url);
        }

        [HttpPost]
        [Route("{tenant}/hazardtype/remove")]
        public async Task<ActionResult> RemoveHazardType(int id, string tenant)
        {

            string url = "/";
            try
            {
                var tenantDetails = await _catalogRepository.GetTenant(tenant);
                if (tenantDetails != null)
                {

                    var model = await _tenantRepository.GetHazardType(id, tenantDetails.TenantId);
                    model.ModifiedBy = GetUser.UserId;
                    model.IsDeleted = true;
                    await _tenantRepository.UpdateHazardType(model, tenantDetails.TenantId);
                    return RedirectToAction("Index");
                }
                else
                {
                    var message = _localizer["The user does not exist."];
                    DisplayMessage(message, "Error");
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(0, ex, "Email", "");
                return PartialView();
            }
            return Redirect(url);
        }


        [HttpPost]
        [Route("{tenant}/location/remove")]
        public async Task<ActionResult> RemoveLocation(int id, string tenant)
        {

            string url = "/";
            try
            {
                var tenantDetails = await _catalogRepository.GetTenant(tenant);
                if (tenantDetails != null)
                {

                    var model = await _tenantRepository.GetLocation(id, tenantDetails.TenantId);
                    model.ModifiedBy = GetUser.UserId;
                    model.IsDeleted = true;
                    await _tenantRepository.UpdateLocation(model, tenantDetails.TenantId);
                    return RedirectToAction("Index");
                }
                else
                {
                    var message = _localizer["The user does not exist."];
                    DisplayMessage(message, "Error");
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(0, ex, "Email", "");
                return PartialView();
            }
            return Redirect(url);
        }

        [HttpPost]
        [Route("{tenant}/department/remove")]
        public async Task<ActionResult> RemoveDepartment(int id, string tenant)
        {

            string url = "/";
            try
            {
                var tenantDetails = await _catalogRepository.GetTenant(tenant);
                if (tenantDetails != null)
                {

                    var model = await _tenantRepository.GetDepartment(id, tenantDetails.TenantId);
                    model.ModifiedBy = GetUser.UserId;
                    model.IsDeleted = true;
                    await _tenantRepository.UpdateDepartment(model, tenantDetails.TenantId);
                    return RedirectToAction("Index");
                }
                else
                {
                    var message = _localizer["The user does not exist."];
                    DisplayMessage(message, "Error");
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(0, ex, "Email", "");
                return PartialView();
            }
            return Redirect(url);
        }

        [HttpPost]
        [Route("{tenant}/user/remove")]
        public async Task<ActionResult> RemoveUser(Guid id)
        {

            string url = "/";
            try
            {

                var user = await _catalogRepository.RemoveUser(id);

                var model = await _tenantRepository.GetEmployee(user.UserId, user.TenantId);
                model.ModifiedBy = GetUser.UserId;
                model.IsDeleted = true;
                await _tenantRepository.UpdateEmployee(model, user.TenantId);
                return RedirectToAction("Index");

            }
            catch (Exception ex)
            {
                _logger.LogError(0, ex, "Email", "");
                return PartialView();
            }
            return Redirect(url);
        }
    }
}
