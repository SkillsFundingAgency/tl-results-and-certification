using AutoMapper;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminChangeLog;
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
                .ForMember(d => d.DateAndTimeOfChange, opts => opts.MapFrom(s => s.CreatedOn));
        }
    }
}
