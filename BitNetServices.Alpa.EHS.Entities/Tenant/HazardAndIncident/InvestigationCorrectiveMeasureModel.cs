using BitNetServices.Alpa.EHS.Entities.Tenant.Master;
using System.ComponentModel.DataAnnotations;

namespace BitNetServices.Alpa.EHS.Tenant.Entities
{
    public class InvestigationCorrectiveMeasure : BaseEntity
    {
        public InvestigationCorrectiveMeasure()
        {

        }
        [Key]
        public int InvestigationCorrectiveMeasureId { get; set; }
        public int CorrectiveMeasureId { get; set; }
        public int HazardIncidentInvestigationId { get; set; }
        public CorrectiveMeasure CorrectiveMeasure { get; set; }
        public HazardIncidentInvestigation HazardIncidentInvestigation { get; set; }

    }
}
