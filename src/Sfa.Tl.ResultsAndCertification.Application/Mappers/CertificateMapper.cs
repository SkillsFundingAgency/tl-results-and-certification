using AutoMapper;
using Newtonsoft.Json;
using Sfa.Tl.ResultsAndCertification.Application.Models;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Printing;
using Sfa.Tl.ResultsAndCertification.Models.OverallResults;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sfa.Tl.ResultsAndCertification.Application.Mappers
{
    public class CertificateMapper : Profile
    {
        public CertificateMapper()
        {
            CreateMap<IEnumerable<LearnerResultsPrintingData>, Batch>()
                .ForMember(d => d.Type, opts => opts.MapFrom(s => BatchType.Printing))
                .ForMember(d => d.Status, opts => opts.MapFrom(s => BatchStatus.Created))
                .ForMember(d => d.PrintBatchItems, opts => opts.MapFrom(s => s))
                .ForMember(d => d.CreatedBy, opts => opts.MapFrom(s => Constants.FunctionPerformedBy));

            CreateMap<LearnerResultsPrintingData, PrintBatchItem>()
                .ForMember(d => d.TlProviderAddressId, opts => opts.MapFrom(s => s.TlProvider.TlProviderAddresses.FirstOrDefault().Id))
                .ForMember(d => d.CreatedBy, opts => opts.MapFrom(s => Constants.FunctionPerformedBy))
                .ForMember(d => d.PrintCertificates, opts => opts.MapFrom(s => s.OverallResults));

            CreateMap<OverallResult, PrintCertificate>()
                .ForMember(d => d.Id, opts => opts.Ignore())
                .ForMember(d => d.Uln, opts => opts.MapFrom(s => s.TqRegistrationPathway.TqRegistrationProfile.UniqueLearnerNumber))
                .ForMember(d => d.LearnerName, opts => opts.MapFrom(s => $"{s.TqRegistrationPathway.TqRegistrationProfile.Firstname} {s.TqRegistrationPathway.TqRegistrationProfile.Lastname}"))
                .ForMember(d => d.TqRegistrationPathwayId, opts => opts.MapFrom(s => s.TqRegistrationPathwayId))
                .ForMember(d => d.Type, opts => opts.MapFrom(s => s.CertificateType.Value))
                .ForMember(d => d.DisplaySnapshot, opts => opts.Ignore())
                .ForMember(d => d.LearningDetails, opts => opts.MapFrom(s => TransformLearningDetails(s)))
                .ForMember(d => d.CreatedBy, opts => opts.MapFrom(s => Constants.FunctionPerformedBy))
                .ForMember(d => d.TqRegistrationPathway, opts => opts.Ignore());
        }

        private static string TransformLearningDetails(OverallResult overallResult)
        {
            var overallResultDetail = JsonConvert.DeserializeObject<OverallResultDetail>(overallResult.Details);

            var specialisms = new List<OccupationalSpecialism>();
            if (overallResultDetail.SpecialismDetails == null || !overallResultDetail.SpecialismDetails.Any())
                specialisms.Add(new OccupationalSpecialism { Specialism = string.Empty, Grade = Constants.NotCompleted });
            else
            {
                overallResultDetail.SpecialismDetails?.ForEach(x =>
                {
                    specialisms.Add(new OccupationalSpecialism { Specialism = x.SpecialismName, Grade = !string.IsNullOrWhiteSpace(x.SpecialismResult) ? x.SpecialismResult : Constants.NotCompleted });
                });
            }

            var profile = overallResult.TqRegistrationPathway.TqRegistrationProfile;
            var learningDetails = new LearningDetails
            {
                TLevelTitle = overallResultDetail.TlevelTitle.Replace(Constants.TLevelIn, string.Empty, StringComparison.InvariantCultureIgnoreCase),
                Core = overallResultDetail.PathwayName,
                CoreGrade = !string.IsNullOrWhiteSpace(overallResultDetail.PathwayResult) ? overallResultDetail.PathwayResult : Constants.NotCompleted,
                OccupationalSpecialism = specialisms,
                IndustryPlacement = GetIndustryPlacementText(overallResultDetail.IndustryPlacementStatus),
                Grade = overallResult.ResultAwarded,
                EnglishAndMaths = GetEnglishAndMathsText(profile.EnglishStatus, profile.MathsStatus),
                Date = DateTime.UtcNow.ToCertificateDateFormat()
            };

            return JsonConvert.SerializeObject(learningDetails);
        }

        private static string GetEnglishAndMathsText(SubjectStatus? englishStatus, SubjectStatus? mathsStatus)
        {
            if ((englishStatus == SubjectStatus.Achieved || englishStatus == SubjectStatus.AchievedByLrs) &&
                (mathsStatus == SubjectStatus.Achieved || mathsStatus == SubjectStatus.AchievedByLrs))
                return Constants.MathsAndEnglishAchievedText;
            else if (mathsStatus == SubjectStatus.Achieved || mathsStatus == SubjectStatus.AchievedByLrs)
                return Constants.MathsAchievedText;
            else if (englishStatus == SubjectStatus.Achieved || englishStatus == SubjectStatus.AchievedByLrs)
                return Constants.EnglishAchievedText;
            else
                return null;
        }

        private static string GetIndustryPlacementText(string industryPlacementStatus)
        {
            var ipStatus = EnumExtensions.GetEnumByDisplayName<IndustryPlacementStatus>(industryPlacementStatus);
            
            if (ipStatus == IndustryPlacementStatus.Completed || ipStatus == IndustryPlacementStatus.CompletedWithSpecialConsideration)
                return Constants.Met;
            
           return Constants.NotMet;
        }
    }
}