using BitNetServices.Alpa.EHS.Common.Model.Catalog;
using BitNetServices.Alpa.EHS.Common.Model.Tenant;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BitNetServices.Alpa.EHS.Web.Tenant.Models
{
    public class WorkPermitSummaryViewModel
    {
        public WorkPermitSummaryViewModel()
        {           
            User = new UserModel();
            WorkPermits = new List<WorkPermitModel>();
        }
        public IEnumerable<WorkPermitModel> WorkPermits { get; set; }
        public UserModel User { get; set; }

        
    }
}
