using BitNetServices.Alpa.EHS.Entities.Tenant.Master;
using System.ComponentModel.DataAnnotations;

namespace BitNetServices.Alpa.EHS.Tenant.Entities
{
    public class InvestigationRootCause : BaseEntity
    {
        public InvestigationRootCause()
        {

        }
        [Key]
        public int InvestigationRootCauseId { get; set; }
        public int RootCauseId { get; set; }
        public int HazardIncidentInvestigationId { get; set; }
        public RootCause RootCause { get; set; }
        public HazardIncidentInvestigation HazardIncidentInvestigation { get; set; }

    }
}
