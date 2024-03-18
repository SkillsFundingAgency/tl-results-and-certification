using AutoMapper;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Learner;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminPostResults;
using System.Linq;
using System;

namespace Sfa.Tl.ResultsAndCertification.Web.Mapper
{
    public class AdminPostResultsMapper : Profile
    {
        public AdminPostResultsMapper()
        {
            CreateMap<AdminLearnerRecord, AdminOpenPathwayRommViewModel>()
                .ForMember(d => d.RegistrationPathwayId, opts => opts.MapFrom(s => s.RegistrationPathwayId))
                .ForMember(d => d.PathwayAssessmentId, opts => opts.MapFrom((src, dest, destMember, context) => (int)context.Items[Constants.AssessmentId]))
                .ForMember(d => d.PathwayName, opts => opts.MapFrom(s => $"{s.Pathway.Name} ({s.Pathway.LarId})"))
                .ForMember(d => d.Learner, opts => opts.MapFrom(s => $"{s.Firstname} {s.Lastname}"))
                .ForMember(d => d.Uln, opts => opts.MapFrom(s => s.Uln))
                .ForMember(d => d.Provider, opts => opts.MapFrom(s => $"{s.Pathway.Provider.Name} ({s.Pathway.Provider.Ukprn})"))
                .ForMember(d => d.Tlevel, opts => opts.MapFrom(s => s.Pathway.Name))
                .ForMember(d => d.StartYear, opts => opts.MapFrom(s => $"{s.Pathway.AcademicYear} to {s.Pathway.AcademicYear + 1}"))
                .ForMember(d => d.ExamPeriod, opts => opts.MapFrom((src, dest, destMember, context) => GetPathwayAssessmentPropertyValue(src, (int)context.Items[Constants.AssessmentId], p => p?.SeriesName)))
                .ForMember(d => d.Grade, opts => opts.MapFrom((src, dest, destMember, context) => GetPathwayAssessmentPropertyValue(src, (int)context.Items[Constants.AssessmentId], p => p?.Result?.Grade)));


            CreateMap<AdminLearnerRecord, AdminOpenSpecialismRommViewModel>()
                    .ForMember(d => d.RegistrationPathwayId, opts => opts.MapFrom(s => s.RegistrationPathwayId))
                    .ForMember(d => d.SpecialismAssessmentId, opts => opts.MapFrom((src, dest, destMember, context) => (int)context.Items[Constants.AssessmentId]))
                    .ForMember(d => d.SpecialismName, opts => opts.MapFrom((src, dest, destMember, context) =>
                    {
                        int assessmentId = (int)context.Items[Constants.AssessmentId];
                        Specialism specialism = src?.Pathway?.Specialisms?.SingleOrDefault(s => s.Assessments.Any(a => a.Id == assessmentId));

                        return $"{specialism?.Name} ({specialism.LarId})";

                    })).ForMember(d => d.Learner, opts => opts.MapFrom(s => $"{s.Firstname} {s.Lastname}"))
                    .ForMember(d => d.Uln, opts => opts.MapFrom(s => s.Uln))
                    .ForMember(d => d.Provider, opts => opts.MapFrom(s => $"{s.Pathway.Provider.Name} ({s.Pathway.Provider.Ukprn})"))
                    .ForMember(d => d.Tlevel, opts => opts.MapFrom(s => s.Pathway.Name))
                    .ForMember(d => d.StartYear, opts => opts.MapFrom(s => $"{s.Pathway.AcademicYear} to {s.Pathway.AcademicYear + 1}"))
                    .ForMember(d => d.ExamPeriod, opts => opts.MapFrom((src, dest, destMember, context) => GetSpecialismAssessmentPropertyValue(src, (int)context.Items[Constants.AssessmentId], p => p?.SeriesName)))
                    .ForMember(d => d.Grade, opts => opts.MapFrom((src, dest, destMember, context) => GetSpecialismAssessmentPropertyValue(src, (int)context.Items[Constants.AssessmentId], p => p?.Result?.Grade)));

        }

        private static T GetPathwayAssessmentPropertyValue<T>(AdminLearnerRecord learnerRecord, int assessmentId, Func<Assessment, T> getPropertyValue)
        {
            var pathwayAssessment = learnerRecord?.Pathway?.PathwayAssessments?.SingleOrDefault(p => p.Id == assessmentId);
            return pathwayAssessment == null ? default : getPropertyValue(pathwayAssessment);
        }

        private T GetSpecialismAssessmentPropertyValue<T>(AdminLearnerRecord learnerRecord, int assessmentId, Func<Assessment, T> getPropertyValue)
        {
            Assessment specialismAssessment = learnerRecord?.Pathway?.Specialisms?.SelectMany(s => s.Assessments).SingleOrDefault(a => a.Id == assessmentId);
            return specialismAssessment == null ? default : getPropertyValue(specialismAssessment);
        }
    }
}