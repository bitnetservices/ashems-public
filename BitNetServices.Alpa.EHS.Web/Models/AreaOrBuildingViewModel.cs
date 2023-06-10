using BitNetServices.Alpa.EHS.Common.Model.Tenant;
using BitNetServices.Alpa.EHS.Common.Model.Tenant.Master;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BitNetServices.Alpa.EHS.Web.Tenant.Models
{
    public class AreaOrBuildingViewModel : AreaOrBuildingModel
    {
        public AreaOrBuildingViewModel()
        {

            LocationList = new List<SelectListItem>();
        }
        public IEnumerable<SelectListItem> LocationList { get; set; }

    }
}
