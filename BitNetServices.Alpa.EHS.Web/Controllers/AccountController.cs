using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using BitNetServices.Alpa.EHS.Common.Interfaces;
using BitNetServices.Alpa.EHS.Common.Model.Catalog;
using BitNetServices.Alpa.EHS.Common.Model.Tenant.Master;
using BitNetServices.Alpa.EHS.Common.Models;
using BitNetServices.Alpa.EHS.Web.Tenant.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace BitNetServices.Alpa.EHS.Web.Controllers
{
    [Route("auth")]
    public class AccountController : BaseController
    {
        #region Fields

        private readonly ITenantRepository _tenantRepository;
        private readonly IStringLocalizer<AccountController> _localizer;
        private readonly ICatalogRepository _catalogRepository;
        private readonly ILogger _logger;
        private readonly DnsClient.ILookupClient _client;

        #endregion

        #region Constructors

        public AccountController(IStringLocalizer<AccountController> localizer, IStringLocalizer<BaseController> baseLocalizer, ITenantRepository tenantRepository, ICatalogRepository catalogRepository, ILogger<AccountController> logger, IConfiguration configuration, DnsClient.ILookupClient client)
            : base(baseLocalizer, tenantRepository, configuration, client)
        {
            _localizer = localizer;
            _tenantRepository = tenantRepository;
            _catalogRepository = catalogRepository;
            _logger = logger;
            _client = client;
        }

        #endregion

        [Route("signin")]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [Route("signin")]
        public async Task<ActionResult> Login(UserModel model)
        {

            string url = "/";
            try
            {
                var user = await _catalogRepository.GetUser(model.Email, model.Password);

                var tenantDetails = (_catalogRepository.GetTenant(user.TenantName)).Result;

                SetTenantConfig(tenantDetails.TenantId, tenantDetails.TenantIdInString);

                user = await _tenantRepository.GetUserPrincipal(user, tenantDetails.TenantId);

                var userSessions = HttpContext.Session.GetObjectFromJson<List<UserModel>>("SessionUsers");
                userSessions = userSessions ??  new List<UserModel>();
                userSessions.Add(user);                
                HttpContext.Session.SetObjectAsJson("SessionUsers", userSessions);
                url = this.Url.Action("Index", "HazardIncidents", new { tenant = Regex.Replace(tenantDetails.TenantName.ToLower(), @"\s+", "") });
            }
            catch (Exception ex)
            {
                _logger.LogError(0, ex, "Login failed for user {regEmail}", model.Email);
                ViewBag.ErrorMessage = ex.Message;
                return View(model);
            }
            return Redirect(url);
        }

        [Route("signout")]
        public ActionResult Logout(string tenant, string email)
        {
            try
            {
                var userSessions = HttpContext.Session.GetObjectFromJson<List<UserModel>>("SessionUsers");
                if (userSessions != null)
                {
                    userSessions.Remove(userSessions.First(a => a.Email.ToUpper() == email.ToUpper() && a.TenantName == tenant));
                    HttpContext.Session.SetObjectAsJson("SessionUsers", userSessions);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(0, ex, "Log out failed for tenant {tenant}", tenant);
                return View("TenantError", tenant);
            }
            return RedirectToAction("Index", "Home");
        }


        [Route("unauthorise")]
        public ActionResult Unauthorise()
        {
            return View();
        }

    }
}
