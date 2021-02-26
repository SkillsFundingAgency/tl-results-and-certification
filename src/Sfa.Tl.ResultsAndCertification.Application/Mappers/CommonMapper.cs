using AutoMapper;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;

namespace Sfa.Tl.ResultsAndCertification.Application.Mappers
{
    public class CommonMapper : Profile
    {
        public CommonMapper()
        {
            CreateMap<TlLookup, LookupData>();
        }
    }
}