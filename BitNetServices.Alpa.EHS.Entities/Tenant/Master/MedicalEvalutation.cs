using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitNetServices.Alpa.EHS.Entities.Tenant.Master
{
    public class MedicalEvaluation : MasterBaseEntity
    {
        public MedicalEvaluation()
        {
            
            CreatedOn = CreatedOn.Equals(System.DateTime.MinValue) ? System.DateTime.Today : CreatedOn;
            ModifiedOn = ModifiedOn.Equals(System.DateTime.MinValue) ? System.DateTime.Today : ModifiedOn;

        }
        [Key]
        public int MedicalEvaluationId { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }
    }
}
