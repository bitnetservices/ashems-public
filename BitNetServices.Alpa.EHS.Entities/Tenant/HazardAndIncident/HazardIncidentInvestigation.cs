using System;
using System.Linq;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using BitNetServices.Alpa.EHS.Entities.Tenant.Master;
using System.Collections.Generic;

namespace BitNetServices.Alpa.EHS.Tenant.Entities
{
    public partial class HazardIncidentInvestigation : BaseEntity
    {
        public HazardIncidentInvestigation()
        {
            InvestigationProcessEnvironments = InvestigationProcessEnvironments ?? new List<InvestigationProcessEnvironment>();
            InvestigationPersonnels = InvestigationPersonnels ?? new List<InvestigationPersonnel>();
            InvestigationRootCauses = InvestigationRootCauses ?? new List<InvestigationRootCause>();
            InvestigationCorrectiveMeasures = InvestigationCorrectiveMeasures ?? new List<InvestigationCorrectiveMeasure>();
        }

        [Key]
        public int HazardIncidentInvestigationId { get; set; }

        [Display(Description = "Hazard Incident Id")]
        public int HazardIncidentId { get; set; }

        public int EmployeeId { get; set; }

        public string EmployeeDesignation { get; set; }
        public string EmployeeManager { get; set; }

        [Required]
        [StringLength(500)]
        [Display(Name = "Other Investigations")]
        public string OtherInvestigations { get; set; }

        public bool? HazardSufficientlyControlled { get; set; }

        public bool? MedicalassistanceProvided { get; set; }

        public bool? SceneSecured { get; set; }

        public int MedicalEvalutionId { get; set; }


        [Required]
        [StringLength(500)]
        [Display(Name = "Sequence Of Events")]
        public string SequenceOfEvents { get; set; }

        public virtual Employee Employee { get; set; }

        public MedicalEvaluation MedicalEvalution { get; set; }

        public virtual HazardIncident HazardIncident { get; set; }
        public virtual ICollection<InvestigationProcessEnvironment> InvestigationProcessEnvironments { get; set; }
        public virtual ICollection<InvestigationPersonnel> InvestigationPersonnels { get; set; }
        public virtual ICollection<InvestigationRootCause> InvestigationRootCauses { get; set; }
        public virtual ICollection<InvestigationCorrectiveMeasure> InvestigationCorrectiveMeasures { get; set; }


    }
}
