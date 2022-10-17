using AutoMapper;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Common;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.TrainingProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Application.Services
{
    public class TrainingProviderService : ITrainingProviderService
    {
        private readonly IRepository<TqRegistrationProfile> _tqRegistrationProfile;
        private readonly ITrainingProviderRepository _trainingProviderRepository;
        private readonly IRepository<Batch> _batchRepository;
        private readonly IRepository<PrintCertificate> _printCertificateRepository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public TrainingProviderService(IRepository<TqRegistrationProfile> tqRegistrationProfile,
            ITrainingProviderRepository trainingProviderRepository,
            IRepository<Batch> batchRepository,
            IRepository<PrintCertificate> printCertificateRepository,
            IMapper mapper,
            ILogger<TrainingProviderService> logger)
        {
            _tqRegistrationProfile = tqRegistrationProfile;
            _trainingProviderRepository = trainingProviderRepository;
            _batchRepository = batchRepository;
            _printCertificateRepository = printCertificateRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<SearchLearnerFilters> GetSearchLearnerFiltersAsync(long providerUkprn)
        {
            var statusFiltersList = new List<FilterLookupData>();
            foreach (var status in EnumExtensions.GetEnumDisplayNameAndValue<SearchLearnerFilterStatus>())
                statusFiltersList.Add(new FilterLookupData { Id = (int)status.Key, Name = status.Value, IsSelected = false });

            return new SearchLearnerFilters
            {
                AcademicYears = await _trainingProviderRepository.GetSearchAcademicYearFiltersAsync(DateTime.UtcNow),
                Tlevels = await _trainingProviderRepository.GetSearchTlevelFiltersAsync(),
                Status = statusFiltersList
            };
        }

        public async Task<PagedResponse<SearchLearnerDetail>> SearchLearnerDetailsAsync(SearchLearnerRequest request)
        {
            return await _trainingProviderRepository.SearchLearnerDetailsAsync(request);
        }

        public async Task<FindLearnerRecord> FindLearnerRecordAsync(long providerUkprn, long uln)
        {
            return await _trainingProviderRepository.FindLearnerRecordAsync(providerUkprn, uln);
        }

        public async Task<LearnerRecordDetails> GetLearnerRecordDetailsAsync(long providerUkprn, int profileId, int? pathwayId = null)
        {
            return await _trainingProviderRepository.GetLearnerRecordDetailsAsync(providerUkprn, profileId, pathwayId);
        }

        public async Task<bool> UpdateLearnerSubjectAsync(UpdateLearnerSubjectRequest request)
        {
            var profile = await _tqRegistrationProfile.GetFirstOrDefaultAsync(p => p.Id == request.ProfileId
                                                                    && p.TqRegistrationPathways.Any(pa => pa.TqProvider.TlProvider.UkPrn == request.ProviderUkprn
                                                                    && (pa.Status == RegistrationPathwayStatus.Active || pa.Status == RegistrationPathwayStatus.Withdrawn)));
            if (profile == null ||
                (request.SubjectType == SubjectType.Maths && profile.MathsStatus != null) ||
                (request.SubjectType == SubjectType.English && profile.EnglishStatus != null) ||
                (request.SubjectType == SubjectType.NotSpecified) ||
                (request.SubjectStatus == SubjectStatus.NotSpecified))
            {
                _logger.LogWarning(LogEvent.NoDataFound, $"No valid record found to for ProfileId = {request.ProfileId}. Method: UpdateLearnerSubjectAsync({request})");
                return false;
            }

            if (request.SubjectType == SubjectType.Maths)
                profile.MathsStatus = request.SubjectStatus;
            if (request.SubjectType == SubjectType.English)
                profile.EnglishStatus = request.SubjectStatus;

            profile.ModifiedOn = DateTime.UtcNow;
            profile.ModifiedBy = request.PerformedBy;

            return await _tqRegistrationProfile.UpdateAsync(profile) > 0;
        }

        public async Task<bool> CreateReplacementDocumentPrintingRequestAsync(ReplacementPrintRequest request)
        {
            var printCertificate = await _printCertificateRepository
                .GetFirstOrDefaultAsync(p => p.Id == request.PrintCertificateId
                                        && p.Uln == request.Uln
                                        && p.PrintBatchItem.TlProviderAddress.TlProvider.UkPrn == request.ProviderUkprn
                                        && p.TqRegistrationPathway.Status == RegistrationPathwayStatus.Active,
                                        navigationPropertyPath: new Expression<Func<PrintCertificate, object>>[]
                                        {
                                            p => p.PrintBatchItem
                                        });

            if (printCertificate == null)
                return false;

            var replacementBatchRequest = _mapper.Map<Batch>(printCertificate, opt => 
            { 
                opt.Items["providerAddressId"] = request.ProviderAddressId;
                opt.Items["performedBy"] = request.PerformedBy;
            });

            var result = await _batchRepository.CreateAsync(replacementBatchRequest);
            return result > 0;
        }
    }
}