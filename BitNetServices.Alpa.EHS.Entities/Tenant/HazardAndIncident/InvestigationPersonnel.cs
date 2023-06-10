using BitNetServices.Alpa.EHS.Entities.Tenant.Master;
using System.ComponentModel.DataAnnotations;

namespace BitNetServices.Alpa.EHS.Tenant.Entities
{
    public class InvestigationPersonnel : BaseEntity
{
        public InvestigationPersonnel()
        {

        }
        [Key]

        public int InvestigationPersonnelId { get; set; }
        public int PersonnelId { get; set; }
        public int HazardIncidentInvestigationId { get; set; }
        public Personnel Personnel { get; set; }
        public HazardIncidentInvestigation HazardIncidentInvestigation { get; set; }

    }
}
