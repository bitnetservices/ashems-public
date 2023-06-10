using System.Collections.Generic;
using System;
using BitNetServices.Alpa.EHS.Entities.Tenant.Master;
using System.Linq;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace  BitNetServices.Alpa.EHS.Tenant.Entities
{
    public partial class HazardIncident : BaseEntity
    {
        public HazardIncident()
        {
            IncidentBodyParts = IncidentBodyParts ?? new List<IncidentBodyPart>();
            IncidentFuturePreventions = IncidentFuturePreventions ?? new List<IncidentFuturePrevention>();
            IncidentInjuryTypes = IncidentInjuryTypes ?? new List<IncidentInjuryType>();
            IncidentTaskCarrieds = IncidentTaskCarrieds ?? new List<IncidentTaskCarried>();
        }


        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int HazardIncidentId { get; set; }
        [Required]
        [StringLength(50)]
        public string HazardIncidentFormat { get; set; }
        [Required]
        [StringLength(50)]
        public string ReportedBy { get; set; }
        public int EmployeeId { get; set; }
        [Required]
        [StringLength(50)]
        public string EmployeeDesignation { get; set; }
        [Required]
        [StringLength(50)]
        public string EmployeeManager { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime ReportingDate { get; set; }
        public int LocationOfHazardId { get; set; }
        public int AreaOrBuildingId { get; set; }
        [StringLength(50)]
        public string SpecificId { get; set; }

        [Display(Name = "Camera Capture")]
        public string HazardCameraCapture { get; set; }

        [Display(Name = "Camera Capture")]
        public string HazardCompleteCameraCapture { get; set; }

        [Display(Name = "Camera Capture")]
        public string IncidentCameraCapture { get; set; }

        [Display(Name = "Camera Capture")]
        public string IncidentCompleteCameraCapture { get; set; }

        public int? CategoryOfHazardId { get; set; }
        public int? TypeOfHazardId { get; set; }
        [StringLength(255)]
        public string Description { get; set; }
        [StringLength(255)]
        public string ImmediateAction { get; set; }
        public int? ResponsibleDepartmentId { get; set; }
        public int? EmployeeAssignedId { get; set; }
        public string EmployeeAssignedDesignation { get; set; }

        [StringLength(255)]
        public string CorrectiveActionToBeTaken { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CompletionETA { get; set; }
        public int? StatusOfHazardId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DateOfCompletion { get; set; }
        [StringLength(255)]
        public string ReasonForDelay { get; set; }
        [StringLength(500)]
        public string HazardTypeOtherDescription { get; set; }
        public int? IncidentTypeOfHazardId { get; set; }
        public int? InjuredPersonCategoryId { get; set; }
        public int? InjuredEmployeeId { get; set; }
        [StringLength(50)]
        public string InjuredEmployeeDesignation { get; set; }
        [StringLength(50)]
        public string InjuredEmployeeManager { get; set; }
        [StringLength(50)]
        public string InjuredIntern { get; set; }
        public int? InjuredContractorId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? IncidentDate { get; set; }
        public int? LocationOfIncidentId { get; set; }
        public int? AreaOrBuildingIncidentId { get; set; }
        [StringLength(50)]
        public string NameOfWitness { get; set; }
        [StringLength(50)]
        public string TaskBeingCarriedOutOtherDescription { get; set; }
        [StringLength(500)]
        public string TaskBeingCarriedOutDescription { get; set; }

        public int? EmploymentTypeId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DateofJoining { get; set; }
        public int? MonthsDoingThisJob { get; set; }
        [StringLength(500)]
        [Display(Description = "Description")]
        public string IncidentDescription { get; set; }

        [StringLength(500)]
        public string StatementOfWitness { get; set; }
        [StringLength(50)]
        public string IncidentInjuryTypeOtherDescription { get; set; }
        [StringLength(255)]
        public string MachinesInvolved { get; set; }
        [StringLength(255)]
        public string ToolsInvolved { get; set; }
        [StringLength(255)]
        public string PropoertyInvolved { get; set; }
        [StringLength(255)]
        public string VehicleInvolved { get; set; }
        public int? ConsequenceId { get; set; }
        public int? IncidentResultId { get; set; }
        public int? IncidentAnticipatedAbsenceId { get; set; }
        public bool? HasIncidentOccurredPreviously { get; set; }
        [Display(Name = "Incident Future Prevention Id")]
        public int? IncidentFuturePreventionId { get; set; }
        public int? IncidentOccurredPreviouslyId { get; set; }
        [StringLength(50)]
        public string IncidentFuturePreventionOtherDescription { get; set; }
        [StringLength(255)]
        public string RecommendationsToPreventRecurrence { get; set; }
        public int? ResponsibleIncidentDepartmentId { get; set; }
        public int? IncidentEmployeeAssignedId { get; set; }
        [StringLength(50)]
        public string IncidentEmployeeAssignedDesignation { get; set; }
        [StringLength(255)]
        public string DetailsOfCorrectiveActionTaken { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? IncidentCompletionETA { get; set; }
        [Display(Name = "Incident Status Of Hazard")]
        public int? IncidentStatusOfHazardId { get; set; }
        [Display(Name = "Hazard Type Other Description")]
        public string IncidentHazardTypeOtherDescription { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? IncidentDateOfCompletion { get; set; }
        [StringLength(500)]
        public string IncidentReasonForDelay { get; set; }
        [StringLength(500)]
        public string DescriptionPropertyDamaged { get; set; }
        [StringLength(500)]
        public string DamageDescription { get; set; }
        [StringLength(500)]
        public string HowDamageOccured { get; set; }
        [Column(TypeName = "money")]
        public decimal? ApproximateRepairCost { get; set; }
        [Column(TypeName = "money")]
        public decimal? ApproximateReplacementCost { get; set; }
        public bool? PropertyCoveredWithInsurance { get; set; }

        public int HazardIncidentStageId { get; set; }



        public virtual Employee Employee { get; set; }
        public virtual Employee EmployeeAssigned  { get; set; }
        public virtual Employee InjuredEmployee { get; set; }
        public virtual Employee IncidentEmployeeAssigned { get; set; }
        public virtual AreaOrBuilding AreaOrBuilding { get; set; }
        public virtual AreaOrBuilding AreaOrBuildingIncident { get; set; }
        public virtual HazardCategory CategoryOfHazard { get; set; }
        public virtual Consequence Consequence { get; set; }
        public virtual EmploymentType EmploymentType { get; set; }
        public virtual AnticipatedAbsence IncidentAnticipatedAbsence { get; set; }
        public virtual IncidentResult IncidentResult { get; set; }
        public virtual HazardStatus IncidentStatusOfHazard { get; set; }
        public virtual HazardType IncidentTypeOfHazard { get; set; }
        public virtual Contractor InjuredContractor { get; set; }
        public virtual InjuredPersonCategory InjuredPersonCategory { get; set; }
        public virtual Location LocationOfHazard { get; set; }
        public virtual Location LocationOfIncident { get; set; }
        public virtual Department ResponsibleDepartment { get; set; }
        public virtual Department ResponsibleIncidentDepartment { get; set; }
        public virtual HazardStatus StatusOfHazard { get; set; }
        public virtual HazardType TypeOfHazard { get; set; }
        public virtual ICollection<IncidentBodyPart> IncidentBodyParts { get; set; }
        public virtual ICollection<IncidentFuturePrevention> IncidentFuturePreventions { get; set; }
        public virtual ICollection<IncidentInjuryType> IncidentInjuryTypes { get; set; }
        public virtual ICollection<IncidentTaskCarried> IncidentTaskCarrieds { get; set; }

        public virtual HazardIncidentStage HazardIncidentStage { get; set; }

        public virtual HazardIncidentInvestigation Investigation { get; set; }

    }
}
