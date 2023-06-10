using BitNetServices.Alpa.EHS.Entities.Tenant.Master;
using System.ComponentModel.DataAnnotations;

namespace BitNetServices.Alpa.EHS.Tenant.Entities
{
    public class InvestigationProcessEnvironment : BaseEntity
    {
        public InvestigationProcessEnvironment()
        {

        }

        [Key]
        public int InvestigationProcessEnvironmentId { get; set; }
        public int ProcessEnvironmentId { get; set; }
        public int HazardIncidentInvestigationId { get; set; }
        public ProcessEnvironment ProcessEnvironment { get; set; }
        public HazardIncidentInvestigation HazardIncidentInvestigation { get; set; }

    }
}
