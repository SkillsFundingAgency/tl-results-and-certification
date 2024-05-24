using AutoMapper;
using Newtonsoft.Json;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.CertificateTrackingExtraction;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Printing;

namespace Sfa.Tl.ResultsAndCertification.Functions.Mapper
{
    public class CertificateTrackingExtractionMapper : Profile
    {
        public CertificateTrackingExtractionMapper()
        {
            CreateMap<PrintCertificate, CertificateTrackingExtractionData>()
                .ForMember(d => d.Uln, opts => opts.MapFrom(p => p.TqRegistrationPathway.TqRegistrationProfile.UniqueLearnerNumber))
                .ForMember(d => d.FirstName, opts => opts.MapFrom(p => p.TqRegistrationPathway.TqRegistrationProfile.Firstname))
                .ForMember(d => d.LastName, opts => opts.MapFrom(p => p.TqRegistrationPathway.TqRegistrationProfile.Lastname))
                .ForMember(d => d.TrackingId, opts => opts.MapFrom(p => p.PrintBatchItem.TrackingId))
                .ForMember(d => d.PrintCertificateType, opts => opts.MapFrom(p => p.Type))
                .ForMember(d => d.TLevelTitle, opts => opts.MapFrom(p => GetTLevelTitleFromLearningDetails(p.LearningDetails)))
                .ForMember(d => d.BatchId, opts => opts.MapFrom(p => p.PrintBatchItem.BatchId))
                .ForMember(d => d.PrintingBatchItemStatus, opts => opts.MapFrom(p => p.PrintBatchItem.Status))
                .ForMember(d => d.SignedForBy, opts => opts.MapFrom(p => p.PrintBatchItem.SignedForBy))
                .ForMember(d => d.StatusChangedOn, opts => opts.MapFrom(p => p.PrintBatchItem.StatusChangedOn))
                .ForMember(d => d.BatchType, opts => opts.MapFrom(p => p.PrintBatchItem.Batch.Type))
                .ForMember(d => d.BatchStatus, opts => opts.MapFrom(p => p.PrintBatchItem.Batch.Status));
        }

        private static string GetTLevelTitleFromLearningDetails(string learningDetailsJson)
        {
            LearningDetails learningDetails = JsonConvert.DeserializeObject<LearningDetails>(learningDetailsJson);
            return learningDetails.TLevelTitle;
        }
    }
}