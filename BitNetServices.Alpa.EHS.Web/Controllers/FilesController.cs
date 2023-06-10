using System;
using System.Threading.Tasks;
using BitNetServices.Alpa.EHS.Common.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using BitNetServices.Alpa.EHS.Web.Tenant.Models;
using System.IO; //For MemoryStream
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using BitNetServices.Alpa.EHS.Web.Filter;
using BitNetServices.Alpa.EHS.Common.Services;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BitNetServices.Alpa.EHS.Web.Controllers
{
    [SessionExpire]
    [AuthorizedTenantOnly]
    public class FilesController : BaseController
    {
        #region Fields

        private readonly ITenantRepository _tenantRepository;
        private readonly IStringLocalizer<AccountController> _localizer;
        private readonly ICatalogRepository _catalogRepository;
        private readonly ILogger _logger;
        private readonly DnsClient.ILookupClient _client;
        private readonly IHostingEnvironment _environment;

        #endregion

        #region Constructors

        public FilesController(IStringLocalizer<AccountController> localizer, IStringLocalizer<BaseController> baseLocalizer,
            ITenantRepository tenantRepository, ICatalogRepository catalogRepository, ILogger<AccountController> logger,
            IConfiguration configuration, DnsClient.ILookupClient client, IHostingEnvironment hostingEnvironment)
            : base(baseLocalizer, tenantRepository, configuration, client)
        {
            _localizer = localizer;
            _tenantRepository = tenantRepository;
            _catalogRepository = catalogRepository;
            _logger = logger;
            _client = client;
            _environment = hostingEnvironment;
        }

        #endregion

        [Route("{tenant}/file/upload")]
        public ActionResult Upload(string id, string tenant, string formControl, string imageControl, bool showCamera = true, string redirect = "", string folder = "")
        {
            try
            {
                var model = new FileViewModel() { Identifier = id, FormControlToUpdate = formControl, ImageControlToUpdate = imageControl, ShowCamera = showCamera, RedirectOnSuccess = redirect, Folder = folder };
                return PartialView("_Upload", model);
            }
            catch (Exception ex)
            {

            }
            return PartialView("_Upload");
        }

        [HttpPost]
        [Route("{tenant}/file/upload")]
        public async Task<ActionResult> Upload(FileViewModel model, string folder, string tenant)
        {
            try
            {
                var files = HttpContext.Request.Form.Files;
                string blobUri = String.Empty;
                if (files != null)
                {
                    foreach (var file in files)
                    {
                        if (file.Length > 0)
                        {
                            var uploads = Path.Combine(_environment.WebRootPath, "uploads");
                            bool exists = Directory.Exists(uploads);
                            if (!exists)
                                Directory.CreateDirectory(uploads);

                            var fileName = Path.GetFileName(file.FileName);
                            var myUniqueFileName = Convert.ToString(Guid.NewGuid());
                            var fileExtension = Path.GetExtension(fileName);
                            var newFileName = fileName; //string.Concat(myUniqueFileName, fileExtension);

                            string mimeType = file.ContentType;
                            StoreInFolder(file, uploads + $@"\{newFileName}");
                            byte[] fileData = System.IO.File.ReadAllBytes(Path.Combine(uploads, newFileName));

                            BlobStorageService objBlobService = new BlobStorageService(String.Empty, tenant, model.Folder ?? folder);
                            blobUri = await objBlobService.UploadFileToBlob(Path.Combine(uploads, newFileName), fileData, mimeType);
                            model.BlobFileUri = blobUri;
                        }
                    }
                    Response.StatusCode = 200; // OK
                    if (!string.IsNullOrEmpty(model.RedirectOnSuccess))
                    {
                        return Redirect(model.RedirectOnSuccess);
                    }
                    return Json(blobUri);
                }
                else
                {
                    return PartialView("_Upload", model);
                }
            }
            catch (Exception ex)
            {
                return PartialView("_Upload", model);
            }

        }


        [HttpGet("{tenant}/{folder}")]
        public async Task<IActionResult> Index(string tenant, string folder)
        {

            
            var tenantDetails = await _catalogRepository.GetTenant(tenant);
            if (tenantDetails != null)
            {
                List<string> blobs = await new BlobStorageService(String.Empty, tenant, folder).GetFiles();
                var departments = await _tenantRepository.GetAllDepartment(tenantDetails.TenantId);
                IEnumerable<SelectListItem> departmentList = new List<SelectListItem>();
                if (departments != null)
                {
                    departmentList = departments.Select(h => new SelectListItem
                    {
                        Text = h.Name,
                        Value = h.DepartmentId.ToString()
                    });
                }

                var summary = new PPESummaryViewModel()
                {
                    Files = GetBlobViewModels(blobs),
                    Folder = folder,
                    DepartmentList = departmentList
                };
                return View(summary);
            }
            return View();
        }

        [HttpGet("{tenant}/files/download/{folder}/{fileName}")]
        public async Task<IActionResult> Download(string fileName, string tenant, string folder)
        {
            try
            {
                var blobStream = await new BlobStorageService(String.Empty, tenant, folder).Download(fileName);
                return File(blobStream.FileStream, blobStream.ContentType, blobStream.FileName);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View();
            }
        }


        [Route("{tenant}/files/delete/{fileName}")]
        [HttpGet]
        public async Task<IActionResult> Delete(string fileName, string tenant)
        {
            try
            {
                await new BlobStorageService(String.Empty, tenant, String.Empty).Delete(fileName);
                return RedirectToAction("Index", new { folder = fileName.Replace("%2F", "/").Split('/')[0] });
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        private void StoreInFolder(IFormFile file, string fileName)
        {
            using (FileStream fs = System.IO.File.Create(fileName))
            {
                file.CopyTo(fs);
                fs.Flush();
            }
        }

        private List<BlobViewModel> GetBlobViewModels(List<string> blobs)
        {
            List<BlobViewModel> viewModel = new List<BlobViewModel>();
            foreach (string blob in blobs)
            {
                viewModel.Add(new BlobViewModel() { FileName = blob.Split('/')[2], Department = blob.Split('/')[1], BlobPath = blob });
            }

            return viewModel;
        }
    }
}
