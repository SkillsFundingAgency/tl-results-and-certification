using AutoMapper;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using System.Linq;

namespace Sfa.Tl.ResultsAndCertification.Application.Mappers
{
    public class DualSpecialismMapper : Profile
    {
        public DualSpecialismMapper()
        {
            CreateMap<TlDualSpecialism, DualSpecialismDetails>()
                .ForMember(d => d.PathwayDetails, opts => opts.MapFrom(s => s.TlPathway));

            CreateMap<DualSpecialismDetails, TlDualSpecialism>()
                .ForMember(d => d.TlPathway, opts => opts.MapFrom(s => s.PathwayDetails));
        }
    }
}
