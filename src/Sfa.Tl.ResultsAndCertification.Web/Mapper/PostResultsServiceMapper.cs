using AutoMapper;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.PostResultsService;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService;

namespace Sfa.Tl.ResultsAndCertification.Web.Mapper
{
    public class PostResultsServiceMapper : Profile
    {
        public PostResultsServiceMapper()
        {
            CreateMap<FindPrsLearnerRecord, FindPrsLearnerRecordViewModel>()
               .ForMember(d => d.ProfileId, opts => opts.MapFrom(s => s.ProfileId))
               .ForMember(d => d.Uln, opts => opts.MapFrom(s => s.Uln))
               .ForMember(d => d.LearnerName, opts => opts.MapFrom(s => s.LearnerName))
               .ForMember(d => d.ProviderName, opts => opts.MapFrom(s => s.ProviderName))
               .ForMember(d => d.TlevelTitle, opts => opts.MapFrom(s => s.TlevelTitle))
               .ForMember(d => d.Status, opts => opts.MapFrom(s => s.Status));
        }
    }
}
