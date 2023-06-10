using System;
using System.Threading.Tasks;
using BitNetServices.Alpa.EHS.Common.Interfaces;
using BitNetServices.Alpa.EHS.Common.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using BitNetServices.Alpa.EHS.Web.Filter;

namespace BitNetServices.Alpa.EHS.Web.Controllers
{
    public class HomeController : Controller
    {
        #region Fields

        private readonly ICatalogRepository _catalogRepository;
        private readonly ITenantRepository _tenantRepository;
        private readonly ILogger _logger;
        private readonly IUtilities _utilities;

        #endregion

        #region Constructors
        public HomeController(ICatalogRepository catalogRepository, ITenantRepository tenantRepository, ILogger<HomeController> logger, IUtilities utilities)
        {
            _catalogRepository = catalogRepository;
            _tenantRepository = tenantRepository;
            _logger = logger;
            _utilities = utilities;
        }

        #endregion

       
        public IActionResult Index()
        {
            return View();           
        }

        public IActionResult Error()
        {
            return View();
        }
        public IActionResult Terms()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }        
    }
}
