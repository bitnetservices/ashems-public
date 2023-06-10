using BitNetServices.Alpa.EHS.Common.Model.Catalog;
using BitNetServices.Alpa.EHS.Common.Model.Tenant.Master;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace BitNetServices.Alpa.EHS.Web.Tenant.Models
{
    public class MasterViewModel
    {
        public MasterViewModel()
        {
        }
        public IEnumerable<HazardCategoryModel> HazardCateogries { get; set; }
        public IEnumerable<HazardStatusModel> HazardStatuses { get; set; }

        public IEnumerable<HazardTypeModel> HazardTypes{ get; set; }

        public IEnumerable<DepartmentModel> Departments { get; set; }

        public IEnumerable<EmployeeModel> Employees { get; set; }

        public IEnumerable<LocationModel> Locations { get; set; }

        public IEnumerable<AreaOrBuildingModel> AreaOrBuildings { get; set; }

        public IEnumerable<UserModel> Users { get; set; }
    
    }
}
