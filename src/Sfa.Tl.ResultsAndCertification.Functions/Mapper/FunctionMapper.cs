using AutoMapper;
using Lrs.PersonalLearningRecordService.Api.Client;
using Sfa.Tl.ResultsAndCertification.Models.Functions;

namespace Sfa.Tl.ResultsAndCertification.Functions.Mapper
{
    public class FunctionMapper : Profile
    {
        public FunctionMapper()
        {
            CreateMap<GetLearnerLearningEventsResponse, LearnerRecordDetails>()
            .ForMember(d => d.ProfileId, opts => opts.MapFrom((src, dest, destMember, context) => (int)context.Items["profileId"]))
            .ForMember(d => d.Uln, opts => opts.MapFrom(s => s.IncomingULN))
            .ForMember(d => d.LearningEventDetails, opts => opts.MapFrom(s => s.LearnerRecord))
            .ForMember(d => d.PerformedBy, opts => opts.MapFrom(s => "System"));

            CreateMap<LearningEvent, LearningEventDetails>()
                .ForMember(d => d.QualificationCode, opts => opts.MapFrom(s => s.SubjectCode))
                .ForMember(d => d.QualificationTitle, opts => opts.MapFrom(s => s.Subject))
                .ForMember(d => d.Grade, opts => opts.MapFrom(s => s.Grade));
        }
    }
}