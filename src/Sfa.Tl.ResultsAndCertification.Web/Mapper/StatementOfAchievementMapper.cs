using AutoMapper;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.StatementOfAchievement;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.StatementOfAchievement;

namespace Sfa.Tl.ResultsAndCertification.Web.Mapper
{
    public class StatementOfAchievementMapper : Profile
    {
        public StatementOfAchievementMapper()
        {
            CreateMap<FindSoaLearnerRecord, RequestSoaUlnNotWithdrawnViewModel>()
                .ForMember(d => d.Uln, opts => opts.MapFrom(s => s.Uln))
               .ForMember(d => d.LearnerName, opts => opts.MapFrom(s => s.LearnerName))
               .ForMember(d => d.DateofBirth, opts => opts.MapFrom(s => s.DateofBirth.ToDobFormat()))
               .ForMember(d => d.ProviderName, opts => opts.MapFrom(s => s.ProviderName))
               .ForMember(d => d.TLevelTitle, opts => opts.MapFrom(s => s.TlevelTitle))
               .ForMember(d => d.IsWithdrawn, opts => opts.MapFrom(s => s.Status == RegistrationPathwayStatus.Withdrawn));
        }
    }
}