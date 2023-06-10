using System.Collections.Generic;
using System;
using BitNetServices.Alpa.EHS.Entities.Tenant.Master;
using System.Linq;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace  BitNetServices.Alpa.EHS.Tenant.Entities
{
    public partial class WorkPermit : BaseEntity
    {
        public WorkPermit()
        {
            WorkerWorkPermits = WorkerWorkPermits ?? new List<WorkerWorkPermit>();          
        }


        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int WorkPermitId { get; set; }

        [Required]
        public int WorkPermitTypeId { get; set; }
        [Required]
        public int WorkPermitStageId { get; set; }  
        
        [StringLength(500)]
        public string PerformTasks { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime StartDate { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime EndDate { get; set; }

        public int? LocationId { get; set; }

        public int? AreaOrBuildingId { get; set; }

        public int? ResponsibleDepartmentId { get; set; }

        public int? EmployeeAssignedId { get; set; }

        [Required]
        [StringLength(500)]
        public string JobDescription { get; set; }

        public virtual Department ResponsibleDepartment { get; set; }
        public virtual Employee EmployeeAssigned { get; set; }

        public virtual AreaOrBuilding AreaOrBuilding { get; set; }
        public virtual Location Location { get; set; }

        public virtual WorkPermitStage WorkPermitStage { get; set; }
        public virtual WorkPermitType WorkPermitType { get; set; }


        public virtual ICollection<WorkerWorkPermit> WorkerWorkPermits { get; set; }

    }
}
