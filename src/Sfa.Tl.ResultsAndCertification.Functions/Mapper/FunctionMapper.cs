using AutoMapper;
using Lrs.PersonalLearningRecordService.Api.Client;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
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
            .ForMember(d => d.IsLearnerVerified, opts => opts.MapFrom(s => !s.ResponseCode.Equals("WSEC0208", System.StringComparison.InvariantCultureIgnoreCase)))
            .ForMember(d => d.LearningEventDetails, opts => opts.MapFrom(s => s.LearnerRecord))
            .ForMember(d => d.PerformedBy, opts => opts.MapFrom(s => "System"));

            CreateMap<LearningEvent, LearningEventDetails>()
                .ForMember(d => d.QualificationCode, opts => opts.MapFrom(s => s.SubjectCode))
                .ForMember(d => d.QualificationTitle, opts => opts.MapFrom(s => s.Subject))
                .ForMember(d => d.Grade, opts => opts.MapFrom(s => s.Grade));

            CreateMap<Lrs.LearnerService.Api.Client.Learner, LearnerRecordDetails>()
            .ForMember(d => d.ProfileId, opts => opts.MapFrom((src, dest, destMember, context) => (int)context.Items["profileId"]))
            .ForMember(d => d.Uln, opts => opts.MapFrom(s => s.ULN))
            .ForMember(d => d.Gender, opts => opts.MapFrom(s => EnumExtensions.GetDisplayName<LrsGender>(s.Gender)))
            .ForMember(d => d.IsLearnerVerified, opts => opts.MapFrom((src, dest, destMember, context) => ((string)context.Items["responseCode"]).Equals("WSRC0004", System.StringComparison.InvariantCultureIgnoreCase)))
            .ForMember(d => d.PerformedBy, opts => opts.MapFrom(s => "System"));
        }
    }
}