using AutoMapper;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.PostResultsService;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService;
using System.Linq;

namespace Sfa.Tl.ResultsAndCertification.Web.Mapper
{
    public class PostResultsServiceMapper : Profile
    {
        public PostResultsServiceMapper()
        {
            CreateMap<AssessmentResult, PrsLearnerDetailsViewModel>();
            CreateMap<PrsLearnerDetails, PrsLearnerDetailsViewModel>()
                .ForMember(d => d.ProfileId, opts => opts.MapFrom(s => s.ProfileId))
                .ForMember(d => d.Uln, opts => opts.MapFrom(s => s.Uln))
                .ForMember(d => d.Firstname, opts => opts.MapFrom(s => s.Firstname))
                .ForMember(d => d.Lastname, opts => opts.MapFrom(s => s.Lastname))
                .ForMember(d => d.DateofBirth, opts => opts.MapFrom(s => s.DateofBirth))
                .ForMember(d => d.ProviderName, opts => opts.MapFrom(s => s.ProviderName))
                .ForMember(d => d.ProviderUkprn, opts => opts.MapFrom(s => s.ProviderUkprn))
                .ForMember(d => d.TlevelTitle, opts => opts.MapFrom(s => s.TlevelTitle))
                .ForMember(d => d.Status, opts => opts.MapFrom(s => s.Status))
                .ForMember(d => d.PathwayTitle, opts => opts.MapFrom(s => $"{s.PathwayName} ({s.PathwayCode})"))
                .ForMember(d => d.PathwayAssessmentSeries, opts => opts.MapFrom(s => s.AssessmentResults.Any() ? s.AssessmentResults.FirstOrDefault().PathwayAssessmentSeries : null))
                .ForMember(d => d.PathwayGrade, opts => opts.MapFrom(s => s.AssessmentResults.Any() ? s.AssessmentResults.FirstOrDefault().PathwayGrade : null))
                .ForMember(d => d.PathwayResultId, opts => opts.MapFrom(s => s.AssessmentResults.Any() ? s.AssessmentResults.FirstOrDefault().PathwayResultId : null))
                .ForMember(d => d.PathwayGradeLastUpdatedOn, opts => opts.MapFrom(s => s.AssessmentResults.Any() ? s.AssessmentResults.FirstOrDefault().PathwayGradeLastUpdatedOn.Value.ToDobFormat() : null))
                .ForMember(d => d.PathwayGradeLastUpdatedBy, opts => opts.MapFrom(s => s.AssessmentResults.Any() ? s.AssessmentResults.FirstOrDefault().PathwayGradeLastUpdatedBy : null));

            CreateMap<PrsLearnerDetails, AppealCoreGradeViewModel>()
               .ForMember(d => d.ProfileId, opts => opts.MapFrom(s => s.ProfileId))
               .ForMember(d => d.Uln, opts => opts.MapFrom(s => s.Uln))
               .ForMember(d => d.LearnerName, opts => opts.MapFrom(s => $"{s.Firstname} {s.Lastname}"))
               .ForMember(d => d.DateofBirth, opts => opts.MapFrom(s => s.DateofBirth))
               .ForMember(d => d.PathwayDisplayName, opts => opts.MapFrom(s => $"{s.PathwayName}<br/>({s.PathwayCode})"))
               .ForMember(d => d.PathwayResultId, opts => opts.MapFrom((src, dest, destMember, context) => src.AssessmentResults.FirstOrDefault(r => r.PathwayResultId == (int)context.Items["pathwayResultId"])?.PathwayResultId))
               .ForMember(d => d.PathwayAssessmentSeries, opts => opts.MapFrom((src, dest, destMember, context) => src.AssessmentResults.FirstOrDefault(r => r.PathwayResultId == (int)context.Items["pathwayResultId"])?.PathwayAssessmentSeries))
               .ForMember(d => d.PathwayGrade, opts => opts.MapFrom((src, dest, destMember, context) => src.AssessmentResults.FirstOrDefault(r => r.PathwayResultId == (int)context.Items["pathwayResultId"])?.PathwayGrade))
               .ForMember(d => d.HasPathwayResult, opts => opts.MapFrom((src, dest, destMember, context) => src.AssessmentResults.FirstOrDefault(r => r.PathwayResultId == (int)context.Items["pathwayResultId"])?.HasPathwayResult));
        }
    }
}