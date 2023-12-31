﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading;
using BitNetServices.Alpa.EHS.Common.Interfaces;
using BitNetServices.Alpa.EHS.Common.Models;
using BitNetServices.Alpa.EHS.Common.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;

namespace BitNetServices.Alpa.EHS.Web.Controllers
{
    public class BaseController : Controller
    {
        #region Fields
        private readonly IStringLocalizer<BaseController> _localizer;
        private readonly ITenantRepository _tenantRepository;
        private readonly IConfiguration _configuration;
        private readonly DnsClient.ILookupClient _client;
        #endregion

        #region Constructors
        public BaseController(IStringLocalizer<BaseController> localizer, ITenantRepository tenantRepository, IConfiguration configuration, DnsClient.ILookupClient client)
        {
            _localizer = localizer;
            _tenantRepository = tenantRepository;
            _configuration = configuration;
            _client = client;
        }

        #endregion

        #region Protected Methods

        protected void DisplayMessage(string content, string header)
        {
            if (!string.IsNullOrWhiteSpace(content))
            {
                string heading = header == "Confirmation" ? _localizer["Confirmation"] : _localizer["Error"];

                TempData["msg"] = $"<script>showAlert(\'{heading}\', '{content}');</script>";
            }
        }

        protected void SetTenantConfig(int tenantId, string tenantIdInString)
        {
            var host = HttpContext.Request.Host.ToString();

            var tenantConfig = PopulateTenantConfigs(tenantId, tenantIdInString, host);

            if (tenantConfig != null)
            {
                var tenantConfigs = HttpContext.Session.GetObjectFromJson<List<TenantConfig>>("TenantConfigs");
                if (tenantConfigs == null)
                {
                    tenantConfigs = new List<TenantConfig>
                    {
                        tenantConfig
                    };
                    HttpContext.Session.SetObjectAsJson("TenantConfigs", tenantConfigs);
                }
                else
                {
                    var tenantsInfo = tenantConfigs.Where(i => i.TenantId == tenantId);

                    if (!tenantsInfo.Any())
                    {
                        tenantConfigs.Add(tenantConfig);
                        HttpContext.Session.SetObjectAsJson("TenantConfigs", tenantConfigs);
                    }
                    else
                    {
                        for (var i = 0; i < tenantConfigs.Count; i++)
                        {
                            if (tenantConfigs[i].TenantId == tenantId)
                            {
                                tenantConfigs[i] = tenantConfig;
                                HttpContext.Session.SetObjectAsJson("TenantConfigs", tenantConfigs);
                                break;
                            }
                        }
                    }
                }

                //localisation per venue's language
                //Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(tenantConfig.TenantCulture);
                //Thread.CurrentThread.CurrentUICulture = new CultureInfo(tenantConfig.TenantCulture);
            }
        }

        

        #endregion

        /// <summary>
        /// Populates the tenant configs.
        /// </summary>
        /// <param name="tenantId">The tenant identifier.</param>
        /// <param name="tenantIdInString">The tenant identifier in string.</param>
        /// <param name="host">The host.</param>
        /// <returns></returns>
        private TenantConfig PopulateTenantConfigs(int tenantId, string tenantIdInString, string host)
        {
            try
            {
                //get blobPath
                var blobPath = _configuration["BlobPath"];
                var defaultCulture = _configuration["DefaultRequestCulture"];

                //get user from url
                string user;
                if (host.Contains("localhost"))
                {
                    user = "testuser";
                }
                else
                {
                    string[] hostpieces = host.Split(new[] { "." }, StringSplitOptions.RemoveEmptyEntries);
                    user = hostpieces[2];
                }

                //get the venue details and populate in config settings
                var venueDetails = (_tenantRepository.GetVenueDetails(tenantId)).Result;
                var countries = (_tenantRepository.GetAllCountries(tenantId)).Result;                           
                var tenantServerName = venueDetails.DatabaseServerName;

                //get country language from db 
                var country = (_tenantRepository.GetCountry(venueDetails.CountryCode, tenantId)).Result;
                RegionInfo regionalInfo = new RegionInfo(country.Language);

                return new TenantConfig
                {
                    DatabaseName = venueDetails.DatabaseName,
                    DatabaseServerName = tenantServerName,
                    VenueName = venueDetails.VenueName,
                    //BlobImagePath = blobPath + venueTypeDetails.VenueType + "-user.jpg",
                    //EventTypeNamePlural = venueTypeDetails.EventTypeShortNamePlural.ToUpper(),
                    TenantId = tenantId,
                    TenantName = venueDetails.DatabaseName,
                    Currency = regionalInfo.CurrencySymbol,
                    TenantCulture = defaultCulture,
                    TenantCountries = countries,
                    TenantIdInString = tenantIdInString,
                    User = user
                };
            }
            catch (Exception exception)
            {
                Trace.TraceError(exception.Message, "Error in populating tenant config.");
            }
            return null;
        }

    }
}
