using BitNetServices.Alpa.EHS.Common.Model.Tenant;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BitNetServices.Alpa.EHS.Web.Tenant.Models
{
    public class HazardIncidentViewModel: HazardIncidentModel
    {
        public HazardIncidentViewModel()
        {
            EmployeeList = new List<SelectListItem>();
            LocationList = new List<SelectListItem>();
            AreaOrBuildingList = new List<SelectListItem>();
            HazardCategoryList = new List<SelectListItem>();

            HazardTypeList = new List<SelectListItem>();
            HazardStatusList = new List<SelectListItem>();
            TasksCarriedList = new List<SelectListItem>();
            EmploymentTypeList = new List<SelectListItem>();
            BodyPartList = new List<SelectListItem>();
            ConsequenceList = new List<SelectListItem>();
            ContractorList = new List<SelectListItem>();
            DepartmentList = new List<SelectListItem>();
            FuturePreventionList = new List<SelectListItem>();
            InjuredPersonCategoryList = new List<SelectListItem>();
            InjuryTypeList = new List<SelectListItem>();
            AnticipatedAbsenceList = new List<SelectListItem>();
            IncidentResultList = new List<SelectListItem>();


            ProcessEnvironmentList = new List<SelectListItem>();
            PersonnelList = new List<SelectListItem>();
            RootCauseList = new List<SelectListItem>();
            CorrectiveMeasureList = new List<SelectListItem>();

            MedicalEvaluationList = new List<SelectListItem>();
        }
        public IEnumerable<SelectListItem> EmployeeList { get; set; }
        public IEnumerable<SelectListItem> LocationList { get; set; }

        public IEnumerable<SelectListItem> AreaOrBuildingList { get; set; }

        public IEnumerable<SelectListItem> HazardCategoryList { get; set; }

        public IEnumerable<SelectListItem> HazardTypeList { get; set; }

        public IEnumerable<SelectListItem> HazardStatusList { get; set; }

        public IEnumerable<SelectListItem> TasksCarriedList { get; set; }

        public IEnumerable<SelectListItem> EmploymentTypeList { get; set; }

        public IEnumerable<SelectListItem> BodyPartList { get; set; }

        public IEnumerable<SelectListItem> ConsequenceList { get; set; }

        public IEnumerable<SelectListItem> ContractorList { get; set; }

        public IEnumerable<SelectListItem> DepartmentList { get; set; }

        public IEnumerable<SelectListItem> FuturePreventionList { get; set; }

        public IEnumerable<SelectListItem> InjuredPersonCategoryList { get; set; }

        public IEnumerable<SelectListItem> InjuryTypeList { get; set; }

        public IEnumerable<SelectListItem> AnticipatedAbsenceList { get; set; }

        public IEnumerable<SelectListItem> IncidentResultList { get; set; }


        public IEnumerable<SelectListItem> ProcessEnvironmentList { get; set; }

        public IEnumerable<SelectListItem> PersonnelList { get; set; }

        public IEnumerable<SelectListItem> RootCauseList { get; set; }

        public IEnumerable<SelectListItem> CorrectiveMeasureList { get; set; }

        public IEnumerable<SelectListItem> MedicalEvaluationList { get; set; }

    }
}
