using BitNetServices.Alpa.EHS.Common.Model.Catalog;
using BitNetServices.Alpa.EHS.Common.Model.Tenant;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BitNetServices.Alpa.EHS.Web.Tenant.Models
{
    public class PPESummaryViewModel
    {
        public PPESummaryViewModel()
        {
            Files = new List<BlobViewModel>();            
            Folder = String.Empty;
            DepartmentList = new List<SelectListItem>();
        }
        public List<BlobViewModel> Files { get; set; }

        public string Folder { get; set; }

        public string DepartmentName { get; set; }
        public IEnumerable<SelectListItem> DepartmentList { get; set; }


    }

    public class BlobViewModel
    {
        public BlobViewModel()
        {
            FileName = String.Empty;
            Department = String.Empty;
            BlobPath = String.Empty;
        }
        public string FileName { get; set; }
        public string BlobPath  { get; set; }
       
        public string Department { get; set; }
    }

}
