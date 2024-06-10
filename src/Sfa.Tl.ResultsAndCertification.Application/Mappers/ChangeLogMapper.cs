using AutoMapper;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminChangeLog;
using System.Collections.Generic;
using System.Linq;

namespace Sfa.Tl.ResultsAndCertification.Application.Mappers
{
    public class ChangeLogMapper : Profile
    {
        public ChangeLogMapper()
        {
            _ = CreateMap<ChangeLog, AdminChangeLogRecord>()
                .ForMember(d => d.ChangeLogId, opts => opts.MapFrom(s => s.Id))
                .ForMember(d => d.RegistrationPathwayId, opts => opts.MapFrom(s => s.TqRegistrationPathwayId))
                .ForMember(d => d.Uln, opts => opts.MapFrom(s => s.TqRegistrationPathway.TqRegistrationProfile.UniqueLearnerNumber))
                .ForMember(d => d.FirstName, opts => opts.MapFrom(s => s.TqRegistrationPathway.TqRegistrationProfile.Firstname))
                .ForMember(d => d.LastName, opts => opts.MapFrom(s => s.TqRegistrationPathway.TqRegistrationProfile.Lastname))
                .ForMember(d => d.CreatedBy, opts => opts.MapFrom(s => s.CreatedBy))
                .ForMember(d => d.ChangeType, opts => opts.MapFrom(s => s.ChangeType))
                .ForMember(d => d.ChangeDetails, opts => opts.MapFrom(s => s.Details))
                .ForMember(d => d.ChangeRequestedBy, opts => opts.MapFrom(s => s.Name))
                .ForMember(d => d.ReasonForChange, opts => opts.MapFrom(s => s.ReasonForChange))
                .ForMember(d => d.ZendeskTicketID, opts => opts.MapFrom(s => s.ZendeskTicketID))
                .ForMember(d => d.CoreCode, opts => opts.MapFrom(s => s.TqRegistrationPathway.TqProvider.TqAwardingOrganisation.TlPathway.LarId))
                .ForMember(d => d.PathwayName, opts => opts.MapFrom(s => s.TqRegistrationPathway.TqProvider.TqAwardingOrganisation.TlPathway.Name))
                .ForMember(d => d.CoreExamPeriod, opts => opts.MapFrom(s => s.TqRegistrationPathway.TqPathwayAssessments.FirstOrDefault().AssessmentSeries.Name))
                .ForMember(d => d.SpecialismCode, opts => opts.MapFrom(s => s.TqRegistrationPathway.TqRegistrationSpecialisms.FirstOrDefault().TlSpecialism.LarId))
                .ForMember(d => d.SpecialismName, opts => opts.MapFrom(s => s.TqRegistrationPathway.TqRegistrationSpecialisms.FirstOrDefault().TlSpecialism.Name))
                .ForMember(d => d.SpecialismExamPeriod, opts => opts.MapFrom(s => s.TqRegistrationPathway.TqRegistrationSpecialisms.FirstOrDefault().TqSpecialismAssessments.FirstOrDefault().AssessmentSeries.Name))
                .ForMember(d => d.ChangeDateOfRequest, opts => opts.MapFrom(s => s.DateOfRequest))
                .ForMember(d => d.DateAndTimeOfChange, opts => opts.MapFrom(s => s.CreatedOn))
                .ForMember(d => d.Pathway, opts => opts.MapFrom(s => s.TqRegistrationPathway));

            CreateMap<TqRegistrationPathway, AdminChangeLogPathway>()
               .ForMember(d => d.Id, opts => opts.MapFrom(s => s.Id))
               .ForMember(d => d.LarId, opts => opts.MapFrom(s => s.TqProvider.TqAwardingOrganisation.TlPathway.LarId))
               .ForMember(d => d.Name, opts => opts.MapFrom(s => s.TqProvider.TqAwardingOrganisation.TlPathway.Name))
               .ForMember(d => d.Title, opts => opts.MapFrom(s => s.TqProvider.TqAwardingOrganisation.TlPathway.TlevelTitle))
               .ForMember(d => d.StartYear, opts => opts.MapFrom(s => s.TqProvider.TqAwardingOrganisation.TlPathway.StartYear))
               .ForMember(d => d.AcademicYear, opts => opts.MapFrom(s => s.AcademicYear))
               .ForMember(d => d.Status, opts => opts.MapFrom(s => s.Status))
               .ForMember(d => d.Provider, opts => opts.MapFrom(s => s.TqProvider))
               .ForMember(d => d.PathwayAssessments, opts => opts.MapFrom(s => s.TqPathwayAssessments))
               .ForMember(d => d.Specialisms, opts => opts.MapFrom(s => s.TqRegistrationSpecialisms));

            CreateMap<TqPathwayAssessment, AdminChangeLogAssessment>()
               .ForMember(d => d.Id, opts => opts.MapFrom(s => s.Id))
               .ForMember(d => d.SeriesId, opts => opts.MapFrom(s => s.AssessmentSeries.Id))
               .ForMember(d => d.SeriesName, opts => opts.MapFrom(s => s.AssessmentSeries.Name))
               .ForMember(d => d.ResultEndDate, opts => opts.MapFrom(s => s.AssessmentSeries.EndDate))
               .ForMember(d => d.RommEndDate, opts => opts.MapFrom(s => s.AssessmentSeries.RommEndDate))
               .ForMember(d => d.AppealEndDate, opts => opts.MapFrom(s => s.AssessmentSeries.AppealEndDate))
               .ForMember(d => d.ComponentType, opts => opts.MapFrom(s => ComponentType.Core))
               .ForMember(d => d.LastUpdatedOn, opts => opts.MapFrom(s => s.CreatedOn))
               .ForMember(d => d.LastUpdatedBy, opts => opts.MapFrom(s => s.CreatedBy))
               .ForMember(d => d.Results, opts => opts.MapFrom(s => s.TqPathwayResults));

            CreateMap<TqPathwayResult, AdminChangeLogResult>()
               .ForMember(d => d.Id, opts => opts.MapFrom(s => s.Id))
               .ForMember(d => d.Grade, opts => opts.MapFrom(s => s.TlLookup.Value))
               .ForMember(d => d.GradeCode, opts => opts.MapFrom(s => s.TlLookup.Code))
               .ForMember(d => d.PrsStatus, opts => opts.MapFrom(s => s.PrsStatus))
               .ForMember(d => d.LastUpdatedOn, opts => opts.MapFrom(s => s.CreatedOn))
               .ForMember(d => d.LastUpdatedBy, opts => opts.MapFrom(s => s.CreatedBy));

            CreateMap<TqRegistrationSpecialism, AdminChangeLogSpecialism>()
               .ForMember(d => d.Id, opts => opts.MapFrom(s => s.Id))
               .ForMember(d => d.LarId, opts => opts.MapFrom(s => s.TlSpecialism.LarId))
               .ForMember(d => d.Name, opts => opts.MapFrom(s => s.TlSpecialism.Name))
               .ForMember(d => d.TlSpecialismCombinations, opts => opts.MapFrom(s => s.TqRegistrationPathway.TqProvider.TqAwardingOrganisation.TlPathway.TlPathwaySpecialismCombinations
                                .GroupBy(g => g.GroupId)
                                .Select(c => new KeyValuePair<int, string>(c.Key, string.Join(Constants.PipeSeperator, c.Select(i => i.TlSpecialism.LarId))))))
               .ForMember(d => d.Assessments, opts => opts.MapFrom(s => s.TqSpecialismAssessments));

            CreateMap<TqSpecialismAssessment, AdminChangeLogAssessment>()
               .ForMember(d => d.Id, opts => opts.MapFrom(s => s.Id))
               .ForMember(d => d.SeriesId, opts => opts.MapFrom(s => s.AssessmentSeries.Id))
               .ForMember(d => d.SeriesName, opts => opts.MapFrom(s => s.AssessmentSeries.Name))
               .ForMember(d => d.ResultEndDate, opts => opts.MapFrom(s => s.AssessmentSeries.EndDate))
               .ForMember(d => d.RommEndDate, opts => opts.MapFrom(s => s.AssessmentSeries.RommEndDate))
               .ForMember(d => d.AppealEndDate, opts => opts.MapFrom(s => s.AssessmentSeries.AppealEndDate))
               .ForMember(d => d.ComponentType, opts => opts.MapFrom(s => ComponentType.Specialism))
               .ForMember(d => d.LastUpdatedOn, opts => opts.MapFrom(s => s.CreatedOn))
               .ForMember(d => d.LastUpdatedBy, opts => opts.MapFrom(s => s.CreatedBy))
               .ForMember(d => d.Results, opts => opts.MapFrom(s => s.TqSpecialismResults));

            CreateMap<TqSpecialismResult, AdminChangeLogResult>()
              .ForMember(d => d.Id, opts => opts.MapFrom(s => s.Id))
              .ForMember(d => d.Grade, opts => opts.MapFrom(s => s.TlLookup.Value))
              .ForMember(d => d.GradeCode, opts => opts.MapFrom(s => s.TlLookup.Code))
              .ForMember(d => d.PrsStatus, opts => opts.MapFrom(s => s.PrsStatus))
              .ForMember(d => d.LastUpdatedOn, opts => opts.MapFrom(s => s.CreatedOn))
              .ForMember(d => d.LastUpdatedBy, opts => opts.MapFrom(s => s.CreatedBy));
        }
    }
}
