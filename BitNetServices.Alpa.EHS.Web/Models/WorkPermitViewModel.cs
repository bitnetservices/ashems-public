using BitNetServices.Alpa.EHS.Common.Model.Tenant;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BitNetServices.Alpa.EHS.Web.Tenant.Models
{
    public class WorkPermitViewModel: WorkPermitModel
    {
        public WorkPermitViewModel()
        {
            LocationList = new List<SelectListItem>();
            AreaOrBuildingList = new List<SelectListItem>();
            WorkPermitTypeList = new List<SelectListItem>();
            DepartmentList = new List<SelectListItem>();
        }
        public IEnumerable<SelectListItem> LocationList { get; set; }

        public IEnumerable<SelectListItem> AreaOrBuildingList { get; set; }

        public IEnumerable<SelectListItem> WorkPermitTypeList { get; set; }

        public IEnumerable<SelectListItem> DepartmentList { get; set; }
    }
}
