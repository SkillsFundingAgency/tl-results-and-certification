using AutoMapper;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Common;

namespace Sfa.Tl.ResultsAndCertification.Web.Mapper
{
    public class CommonMapper : Profile
    {
        public CommonMapper()
        {
            CreateMap<LookupData, LookupViewModel>();
        }
    }
}
