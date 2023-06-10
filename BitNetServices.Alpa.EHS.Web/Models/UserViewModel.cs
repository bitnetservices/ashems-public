using BitNetServices.Alpa.EHS.Common.Model.Catalog;
using BitNetServices.Alpa.EHS.Common.Model.Tenant;
using BitNetServices.Alpa.EHS.Common.Model.Tenant.Master;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BitNetServices.Alpa.EHS.Web.Tenant.Models
{
    public class UserViewModel : UserModel
    {
        public UserViewModel()
        {
            TenantList = new List<SelectListItem>();
            SecurityRoleList = new List<SelectListItem>();
        }
        public IEnumerable<SelectListItem> TenantList { get; set; }

        public IEnumerable<SelectListItem> SecurityRoleList { get; set; }

        public int DepartmentId { get; set; }

        public string? SecurityRoleId { get; set; }


    }
}
