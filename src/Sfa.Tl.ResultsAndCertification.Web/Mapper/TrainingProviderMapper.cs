using AutoMapper;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Common;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.TrainingProvider;
using Sfa.Tl.ResultsAndCertification.Web.Mapper.Resolver;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider.Manual;
using SearchLearnerDetailsContent = Sfa.Tl.ResultsAndCertification.Web.Content.TrainingProvider.SearchLearnerDetails;

namespace Sfa.Tl.ResultsAndCertification.Web.Mapper
{
    public class TrainingProviderMapper : Profile
    {
        public TrainingProviderMapper()
        {
            CreateMap<PagedResponse<SearchLearnerDetail>, SearchLearnerDetailsListViewModel>()
               .ForMember(d => d.TotalRecords, opts => opts.MapFrom(s => s.TotalRecords))
               .ForMember(d => d.SearchLearnerDetailsList, opts => opts.MapFrom(s => s.Records));

            CreateMap<SearchLearnerDetail, SearchLearnerDetailsViewModel>()
               .ForMember(d => d.ProfileId, opts => opts.MapFrom(s => s.ProfileId))
               .ForMember(d => d.Uln, opts => opts.MapFrom(s => s.Uln))
               .ForMember(d => d.LearnerName, opts => opts.MapFrom(s => s.Firstname + " " + s.Lastname))
               .ForMember(d => d.TlevelName, opts => opts.MapFrom(s => s.TlevelName))
               .ForMember(d => d.StartYear, opts => opts.MapFrom(s => string.Format(SearchLearnerDetailsContent.Start_Year_Value, s.AcademicYear, s.AcademicYear + 1)))
               .ForMember(d => d.IsMathsAdded, opts => opts.MapFrom(s => s.MathsStatus != null && s.MathsStatus != SubjectStatus.NotSpecified))
               .ForMember(d => d.IsEnglishAdded, opts => opts.MapFrom(s => s.EnglishStatus != null && s.EnglishStatus != SubjectStatus.NotSpecified))
               .ForMember(d => d.IsIndustryPlacementAdded, opts => opts.MapFrom(s => s.IndustryPlacementStatus != null && s.IndustryPlacementStatus != IndustryPlacementStatus.NotSpecified));

            CreateMap<LearnerRecordDetails, LearnerRecordDetailsViewModel>()
               .ForMember(d => d.ProfileId, opts => opts.MapFrom(s => s.ProfileId))
               .ForMember(d => d.RegistrationPathwayId, opts => opts.MapFrom(s => s.RegistrationPathwayId))
               .ForMember(d => d.TlPathwayId, opts => opts.MapFrom(s => s.TlPathwayId))
               .ForMember(d => d.Uln, opts => opts.MapFrom(s => s.Uln))
               .ForMember(d => d.LearnerName, opts => opts.MapFrom(s => s.Name))
               .ForMember(d => d.DateofBirth, opts => opts.MapFrom(s => s.DateofBirth))
               .ForMember(d => d.ProviderName, opts => opts.MapFrom(s => s.ProviderName))
               .ForMember(d => d.ProviderUkprn, opts => opts.MapFrom(s => s.ProviderUkprn))
               .ForMember(d => d.TlevelTitle, opts => opts.MapFrom(s => s.TlevelTitle))
               .ForMember(d => d.AcademicYear, opts => opts.MapFrom(s => s.AcademicYear))
               .ForMember(d => d.AwardingOrganisationName, opts => opts.MapFrom(s => s.AwardingOrganisationName))
               .ForMember(d => d.MathsStatus, opts => opts.MapFrom(s => s.MathsStatus))
               .ForMember(d => d.EnglishStatus, opts => opts.MapFrom(s => s.EnglishStatus))
               .ForMember(d => d.IsLearnerRegistered, opts => opts.MapFrom(s => s.IsLearnerRegistered))
               .ForMember(d => d.IndustryPlacementId, opts => opts.MapFrom(s => s.IndustryPlacementId))
               .ForMember(d => d.IndustryPlacementStatus, opts => opts.MapFrom(s => s.IndustryPlacementStatus));

            CreateMap<LearnerRecordDetails, AddMathsStatusViewModel>()               
               .ForMember(d => d.ProfileId, opts => opts.MapFrom(s => s.ProfileId))
               .ForMember(d => d.LearnerName, opts => opts.MapFrom(s => s.Name))
               .ForMember(d => d.SubjectStatus, opts => opts.MapFrom(s => s.IsMathsAchieved));

            CreateMap<LearnerRecordDetails, AddEnglishStatusViewModel>()
               .ForMember(d => d.ProfileId, opts => opts.MapFrom(s => s.ProfileId))
               .ForMember(d => d.LearnerName, opts => opts.MapFrom(s => s.Name))
               .ForMember(d => d.SubjectStatus, opts => opts.MapFrom(s => s.IsEnglishAchieved));

            CreateMap<AddMathsStatusViewModel, UpdateLearnerSubjectRequest>()
               .ForMember(d => d.ProfileId, opts => opts.MapFrom(s => s.ProfileId))
               .ForMember(d => d.SubjectStatus, opts => opts.MapFrom(s => s.IsAchieved == null ? SubjectStatus.NotSpecified : (s.IsAchieved.Value ? SubjectStatus.Achieved : SubjectStatus.NotAchieved)))
               .ForMember(d => d.SubjectType, opts => opts.MapFrom(s => SubjectType.Maths))
               .ForMember(d => d.ProviderUkprn, opts => opts.MapFrom((src, dest, destMember, context) => (long)context.Items["providerUkprn"]))
               .ForMember(d => d.PerformedBy, opts => opts.MapFrom<UserNameResolver<AddMathsStatusViewModel, UpdateLearnerSubjectRequest>>());

            CreateMap<AddEnglishStatusViewModel, UpdateLearnerSubjectRequest>()
               .ForMember(d => d.ProfileId, opts => opts.MapFrom(s => s.ProfileId))
               .ForMember(d => d.SubjectStatus, opts => opts.MapFrom(s => s.IsAchieved == null ? SubjectStatus.NotSpecified : (s.IsAchieved.Value ? SubjectStatus.Achieved : SubjectStatus.NotAchieved)))
               .ForMember(d => d.SubjectType, opts => opts.MapFrom(s => SubjectType.English))
               .ForMember(d => d.ProviderUkprn, opts => opts.MapFrom((src, dest, destMember, context) => (long)context.Items["providerUkprn"]))
               .ForMember(d => d.PerformedBy, opts => opts.MapFrom<UserNameResolver<AddEnglishStatusViewModel, UpdateLearnerSubjectRequest>>());
        }
    }
}