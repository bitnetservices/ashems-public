using AutoMapper;
using BitNetServices.Alpa.EHS.Common.Model.Tenant;
using BitNetServices.Alpa.EHS.Web.Tenant.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BitNetServices.Alpa.EHS.Web
{
    public class MappingEntity : Profile
    {
        public MappingEntity()
        {
            CreateMap<HazardIncidentViewModel, HazardIncidentModel>();
            CreateMap<HazardIncidentModel, HazardIncidentViewModel>();
            CreateMap<WorkPermitViewModel, WorkPermitModel>();
            CreateMap<WorkPermitModel, WorkPermitViewModel>();
        }
    }
}
