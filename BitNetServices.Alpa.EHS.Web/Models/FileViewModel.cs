using BitNetServices.Alpa.EHS.Common.Model.Tenant;
using BitNetServices.Alpa.EHS.Common.Model.Tenant.Master;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BitNetServices.Alpa.EHS.Web.Tenant.Models
{
    public class FileViewModel : BaseModel
    {
        public FileViewModel()
        {

        }

        public string Identifier { get; set; }
        public string BlobFileUri { get; set; }

        public bool ShowCamera { get; set; }

        public string ImageControlToUpdate { get; set; }

        public string FormControlToUpdate { get; set; }
        public string FileTypeFilter { get; set; }

        [Required]
        public IFormFile File { get; set; }

        public string RedirectOnSuccess { get; set; }

        public string Folder { get; set; }
    }
}
