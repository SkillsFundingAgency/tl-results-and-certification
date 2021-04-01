using AutoMapper;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.TrainingProvider;
using Sfa.Tl.ResultsAndCertification.Web.Mapper.Resolver;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider.Manual;

namespace Sfa.Tl.ResultsAndCertification.Web.Mapper
{
    public class TrainingProviderMapper : Profile
    {
        public TrainingProviderMapper()
        {
            CreateMap<LearnerRecordDetails, LearnerRecordDetailsViewModel>();

            CreateMap<AddLearnerRecordViewModel, AddLearnerRecordRequest>()
               .ForMember(d => d.Ukprn, opts => opts.MapFrom((src, dest, destMember, context) => (long)context.Items["Ukprn"]))
               .ForMember(d => d.Uln, opts => opts.MapFrom(s => s.Uln.EnterUln))
               .ForMember(d => d.HasLrsEnglishAndMaths, opts => opts.MapFrom(s => s.LearnerRecord.HasLrsEnglishAndMaths))
               .ForMember(d => d.EnglishAndMathsStatus, opts => opts.MapFrom(s => s.LearnerRecord.HasLrsEnglishAndMaths ? null : s.EnglishAndMathsQuestion.EnglishAndMathsStatus))
               .ForMember(d => d.IndustryPlacementStatus, opts => opts.MapFrom(s => s.IndustryPlacementQuestion.IndustryPlacementStatus))
               .ForMember(d => d.PerformedBy, opts => opts.MapFrom<UserNameResolver<AddLearnerRecordViewModel, AddLearnerRecordRequest>>());
        }
    }
}