using BitNetServices.Alpa.EHS.Common.Model.Catalog;
using BitNetServices.Alpa.EHS.Common.Model.Tenant;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BitNetServices.Alpa.EHS.Web.Tenant.Models
{
    public class DashboardViewModel
    {
        public DashboardViewModel()
        {
            User = new UserModel();
            ChartHazardIncidentByStage = new List<ChartData>();
            ChartHazardIncidentByType = new List<ChartData>();
            HazardIncidentCount = 0;
        }
        public IEnumerable<ChartData> ChartHazardIncidentByStage { get;set;}
        public int HazardIncidentCount { get; set; }
        public int NeedAttention { get; set; }
        public int YourDepartment { get; set; }

        public int UnderInvestigation { get; set; }
        public int InvestigationFinished { get; set; }

        public string HazardIncidentAverageClosingTime { get; set; }
        public IEnumerable<ChartData> ChartHazardIncidentByType { get; set; }
        public UserModel User { get; set; }

    }

    public class ChartData
    {
        public string labels { get; set; }
        public int data { get; set; }
    }
}
