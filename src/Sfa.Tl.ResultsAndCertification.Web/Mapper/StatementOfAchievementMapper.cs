using AutoMapper;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.ProviderAddress;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.StatementOfAchievement;
using Sfa.Tl.ResultsAndCertification.Web.Mapper.Resolver;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.ProviderAddress;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.StatementOfAchievement;
using System;
using System.Collections.Generic;
using RequestSoaCheckAndSubmitContent = Sfa.Tl.ResultsAndCertification.Web.Content.StatementOfAchievement.RequestSoaCheckAndSubmit;

namespace Sfa.Tl.ResultsAndCertification.Web.Mapper
{
    public class StatementOfAchievementMapper : Profile
    {
        public StatementOfAchievementMapper()
        {
            CreateMap<SoaLearnerRecordDetails, SoaLearnerRecordDetailsViewModel>()
               .ForMember(d => d.ProfileId, opts => opts.MapFrom(s => s.ProfileId))
               .ForMember(d => d.Uln, opts => opts.MapFrom(s => s.Uln))
               .ForMember(d => d.LearnerName, opts => opts.MapFrom(s => $"{s.Firstname} {s.Lastname}"))
               .ForMember(d => d.DateofBirth, opts => opts.MapFrom(s => s.DateofBirth))
               .ForMember(d => d.ProviderDisplayName, opts => opts.MapFrom(s => $"{s.ProviderName} ({s.ProviderUkprn})"))
               .ForMember(d => d.ProviderName, opts => opts.MapFrom(s => s.ProviderName))
               .ForMember(d => d.ProviderUkprn, opts => opts.MapFrom(s => s.ProviderUkprn))
               .ForMember(d => d.TlevelTitle, opts => opts.MapFrom(s => s.TlevelTitle))
               .ForMember(d => d.RegistrationPathwayId, opts => opts.MapFrom(s => s.RegistrationPathwayId))
               .ForMember(d => d.PathwayDisplayName, opts => opts.MapFrom(s => $"{s.PathwayName} ({s.PathwayCode})"))
               .ForMember(d => d.PathwayName, opts => opts.MapFrom(s => s.PathwayName))
               .ForMember(d => d.PathwayCode, opts => opts.MapFrom(s => s.PathwayCode))
               .ForMember(d => d.PathwayGrade, opts => opts.MapFrom(s => !string.IsNullOrWhiteSpace(s.PathwayGrade) ? s.PathwayGrade : RequestSoaCheckAndSubmitContent.None))
               .ForMember(d => d.SpecialismDisplayName, opts => opts.MapFrom(s => !string.IsNullOrWhiteSpace(s.SpecialismName) ? $"{s.SpecialismName} ({s.SpecialismCode})" : RequestSoaCheckAndSubmitContent.Not_Specified))
               .ForMember(d => d.SpecialismName, opts => opts.MapFrom(s => s.SpecialismName))
               .ForMember(d => d.SpecialismCode, opts => opts.MapFrom(s => s.SpecialismCode))
               .ForMember(d => d.SpecialismGrade, opts => opts.MapFrom(s => !string.IsNullOrWhiteSpace(s.SpecialismGrade) ? s.SpecialismGrade : RequestSoaCheckAndSubmitContent.None))
               .ForMember(d => d.IsEnglishAndMathsAchieved, opts => opts.MapFrom(s => s.IsEnglishAndMathsAchieved))
               .ForMember(d => d.HasLrsEnglishAndMaths, opts => opts.MapFrom(s => s.HasLrsEnglishAndMaths))
               .ForMember(d => d.IsSendLearner, opts => opts.MapFrom(s => s.IsSendLearner))
               .ForMember(d => d.IndustryPlacementStatus, opts => opts.MapFrom(s => s.IndustryPlacementStatus))
               .ForMember(d => d.ProviderAddress, opts => opts.MapFrom(s => s.ProviderAddress))
               .ForMember(d => d.HasPathwayResult, opts => opts.MapFrom(s => s.HasPathwayResult))
               .ForMember(d => d.IsIndustryPlacementAdded, opts => opts.MapFrom(s => s.IsIndustryPlacementAdded))
               .ForMember(d => d.IsLearnerRegistered, opts => opts.MapFrom(s => s.IsLearnerRegistered))
               .ForMember(d => d.IsNotWithdrawn, opts => opts.MapFrom(s => s.IsNotWithdrawn))
               .ForMember(d => d.IsIndustryPlacementCompleted, opts => opts.MapFrom(s => s.IsIndustryPlacementCompleted))
               .ForMember(d => d.LastRequestedOn, opts => opts.MapFrom(s => s.LastRequestedOn));

            CreateMap<Address, AddressViewModel>()
                .ForMember(d => d.AddressId, opts => opts.MapFrom(s => s.AddressId))
                .ForMember(d => d.DepartmentName, opts => opts.MapFrom(s => s.DepartmentName))
                .ForMember(d => d.OrganisationName, opts => opts.MapFrom(s => s.OrganisationName))
                .ForMember(d => d.AddressLine1, opts => opts.MapFrom(s => s.AddressLine1))
                .ForMember(d => d.AddressLine2, opts => opts.MapFrom(s => s.AddressLine2))
                .ForMember(d => d.Town, opts => opts.MapFrom(s => s.Town))
                .ForMember(d => d.Postcode, opts => opts.MapFrom(s => s.Postcode))
                .ReverseMap();

            CreateMap<SoaLearnerRecordDetailsViewModel, SoaPrintingRequest>()
                .ForMember(d => d.ProviderUkprn, opts => opts.MapFrom((src, dest, destMember, context) => (long)context.Items["providerUkprn"]))
                .ForMember(d => d.AddressId, opts => opts.MapFrom(s => s.ProviderAddress.AddressId))
                .ForMember(d => d.RegistrationPathwayId, opts => opts.MapFrom(s => s.RegistrationPathwayId))
                .ForMember(d => d.Uln, opts => opts.MapFrom(s => s.Uln))
                .ForMember(d => d.LearnerName, opts => opts.MapFrom(s => s.LearnerName))
                .ForMember(d => d.LearningDetails, opts => opts.MapFrom(s => s))
                .ForMember(d => d.SoaPrintingDetails, opts => opts.MapFrom(s => s))
                .ForMember(d => d.PerformedBy, opts => opts.MapFrom<UserNameResolver<SoaLearnerRecordDetailsViewModel, SoaPrintingRequest>>());

            CreateMap<SoaLearnerRecordDetailsViewModel, LearningDetails>()
                .ForMember(d => d.TLevelTitle, opts => opts.MapFrom(s => s.TlevelTitle))
                .ForMember(d => d.Date, opts => opts.MapFrom(s => DateTime.UtcNow.ToSoaFormat()))
                .ForMember(d => d.Core, opts => opts.MapFrom(s => s.PathwayName))
                .ForMember(d => d.CoreGrade, opts => opts.MapFrom(s => s.PathwayGrade))
                .ForMember(d => d.OccupationalSpecialism, opts => opts.MapFrom(s => s))
                .ForMember(d => d.IndustryPlacement, opts => opts.MapFrom(s => (s.IndustryPlacementStatus == IndustryPlacementStatus.Completed 
                                                                             || s.IndustryPlacementStatus == IndustryPlacementStatus.CompletedWithSpecialConsideration) 
                                                                             ? Constants.IndustryPlacementCompleted : Constants.IndustryPlacementNotCompleted))
                .ForMember(d => d.EnglishAndMaths, opts => opts.MapFrom(s => s.IsEnglishAndMathsAchieved ? Constants.EnglishAndMathsMet : Constants.EnglishAndMathsNotMet));

            CreateMap<SoaLearnerRecordDetailsViewModel, IList<OccupationalSpecialismDetails>>()
                .ConstructUsing((m, context) =>
                {
                    return new List<OccupationalSpecialismDetails>
                    {
                        new OccupationalSpecialismDetails
                        {
                            Specialism = m.SpecialismName,
                            Grade = m.SpecialismGrade
                        }
                    };
                });

            CreateMap<SoaLearnerRecordDetailsViewModel, SoaPrintingDetails>()
                .ForMember(d => d.Uln, opts => opts.MapFrom(s => s.Uln))
                .ForMember(d => d.Name, opts => opts.MapFrom(s => s.LearnerName))
                .ForMember(d => d.Dateofbirth, opts => opts.MapFrom(s => s.DateofBirth.ToDobFormat()))
                .ForMember(d => d.ProviderName, opts => opts.MapFrom(s => s.ProviderDisplayName))
                .ForMember(d => d.TlevelTitle, opts => opts.MapFrom(s => s.TlevelTitle))
                .ForMember(d => d.Core, opts => opts.MapFrom(s => s.PathwayDisplayName))
                .ForMember(d => d.CoreGrade, opts => opts.MapFrom(s => s.PathwayGrade))
                .ForMember(d => d.Specialism, opts => opts.MapFrom(s => s.SpecialismDisplayName))
                .ForMember(d => d.SpecialismGrade, opts => opts.MapFrom(s => s.SpecialismGrade))
                .ForMember(d => d.IndustryPlacement, opts => opts.MapFrom(s => s.GetIndustryPlacementDisplayText))
                .ForMember(d => d.EnglishAndMaths, opts => opts.MapFrom(s => s.GetEnglishAndMathsStatusDisplayText))
                .ForMember(d => d.ProviderAddress, opts => opts.MapFrom(s => s.ProviderAddress));

            CreateMap<PrintRequestSnapshot, RequestSoaSubmittedAlreadyViewModel>(); // TODO:
        }
    }
}