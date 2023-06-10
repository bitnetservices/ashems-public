using BitNetServices.Alpa.EHS.Common.Model.Tenant;
using BitNetServices.Alpa.EHS.Common.Model.Tenant.Master;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BitNetServices.Alpa.EHS.Web.Tenant.Models
{
    public class EmployeeViewModel: EmployeeModel
    {
        public EmployeeViewModel()
        {

            DepartmentList = new List<SelectListItem>();
            UserList = new List<SelectListItem>();
            EmploymentTypeList = new List<SelectListItem>();
        }
        public IEnumerable<SelectListItem> DepartmentList { get; set; }
        public IEnumerable<SelectListItem> EmploymentTypeList { get; set; }
        public IEnumerable<SelectListItem> UserList { get; set; }

    }
}
