﻿using AutoMapper;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.TrainingProvider;
using Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider.Manual;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.Loader
{
    public class TrainingProviderLoader : ITrainingProviderLoader
    {
        private readonly IResultsAndCertificationInternalApiClient _internalApiClient;
        private readonly IMapper _mapper;

        public TrainingProviderLoader(IResultsAndCertificationInternalApiClient internalApiClient, IMapper mapper)
        {
            _internalApiClient = internalApiClient;
            _mapper = mapper;
        }

        public async Task<SearchLearnerDetailsListViewModel> SearchLearnerDetailsAsync(long providerUkprn, SearchCriteriaViewModel searchCriteriaViewModel)
        {
            var apiRequest = new SearchLearnerRequest
            {
                Ukprn = providerUkprn,
                PageNumber = searchCriteriaViewModel?.PageNumber,
                SearchKey = searchCriteriaViewModel?.SearchKey,
                AcademicYear = new List<int> { searchCriteriaViewModel?.AcademicYear ?? 0 },
                Statuses = searchCriteriaViewModel?.SearchLearnerFilters?.Status?.Where(a => a.IsSelected)?.Select(a => a.Id)?.ToList() ?? new List<int>(),
                IndustryPlacementStatus = searchCriteriaViewModel?.SearchLearnerFilters?.IndustryPlacementStatus?.Where(a => a.IsSelected)?.Select(a => a.Id)?.ToList() ?? new List<int>(),
                Tlevels = searchCriteriaViewModel?.SearchLearnerFilters?.Tlevels?.Where(a => a.IsSelected)?.Select(a => a.Id)?.ToList() ?? new List<int>(),
            };
            var apiResponse = await _internalApiClient.SearchLearnerDetailsAsync(apiRequest);

            return _mapper.Map<SearchLearnerDetailsListViewModel>(apiResponse);
        }

        public async Task<SearchLearnerFiltersViewModel> GetSearchLearnerFiltersAsync(long providerUkprn)
        {
            var apiResponse = await _internalApiClient.GetSearchLearnerFiltersAsync(providerUkprn);
            return _mapper.Map<SearchLearnerFiltersViewModel>(apiResponse);
        }

        public async Task<FindLearnerRecord> FindLearnerRecordAsync(long providerUkprn, long uln)
        {
            return await _internalApiClient.FindLearnerRecordAsync(providerUkprn, uln);
        }

        public async Task<T> GetLearnerRecordDetailsAsync<T>(long providerUkprn, int profileId, int? pathwayId = null)
        {
            var response = await _internalApiClient.GetLearnerRecordDetailsAsync(providerUkprn, profileId, pathwayId);
            return _mapper.Map<T>(response);
        }

        public async Task<LearnerRecordDetailsViewModel> GetLearnerRecordDetailsViewModel(long providerUkprn, int profileId, int documentRerequestInDays)
        {
            LearnerRecordDetails learnerRecordDetails = await _internalApiClient.GetLearnerRecordDetailsAsync(providerUkprn, profileId);

            var viewModel = _mapper.Map<LearnerRecordDetailsViewModel>(learnerRecordDetails, opt =>
            {
                opt.Items[Constants.CertificateRerequestDays] = documentRerequestInDays;
            });

            return viewModel;
        }

        public async Task<bool> UpdateLearnerSubjectAsync(long providerUkprn, AddMathsStatusViewModel model)
        {
            var learnerSubjectRequest = _mapper.Map<UpdateLearnerSubjectRequest>(model, opt => opt.Items["providerUkprn"] = providerUkprn);
            return await _internalApiClient.UpdateLearnerSubjectAsync(learnerSubjectRequest);
        }

        public async Task<bool> UpdateLearnerSubjectAsync(long providerUkprn, AddEnglishStatusViewModel model)
        {
            var learnerSubjectRequest = _mapper.Map<UpdateLearnerSubjectRequest>(model, opt => opt.Items["providerUkprn"] = providerUkprn);
            return await _internalApiClient.UpdateLearnerSubjectAsync(learnerSubjectRequest);
        }

        public async Task<bool> UpdateLearnerWithdrawnStatusAsync(long providerUkprn, ChangeWithdrawnStatusHaveYouToldAwardingOrganisationViewModel model)
        {
            var pendingWithdrawlRequest = _mapper.Map<SetRegistrationAsPendingWithdrawalRequest>(model, opt => opt.Items["providerUkprn"] = providerUkprn);
            return await _internalApiClient.SetRegistrationAsPendingWithdrawalAsync(pendingWithdrawlRequest);
        }

        /// <summary>
        /// Creates the replacement document printing request asynchronous.
        /// </summary>
        /// <param name="providerUkprn">The provider ukprn.</param>
        /// <param name="viewModel">The view model.</param>
        /// <returns></returns>
        public async Task<bool> CreateReplacementDocumentPrintingRequestAsync(long providerUkprn, RequestReplacementDocumentViewModel viewModel)
        {
            var request = _mapper.Map<ReplacementPrintRequest>(viewModel, opt => opt.Items["providerUkprn"] = providerUkprn);
            return await _internalApiClient.CreateReplacementDocumentPrintingRequestAsync(request);
        }

        public Task<bool> ReinstateRegistrationFromPendingWithdrawalAsync(ChangeBackToActiveStatusHaveYouToldAwardingOrganisationViewModel model)
        {
            var request = _mapper.Map<ReinstateRegistrationFromPendingWithdrawalRequest>(model);
            return _internalApiClient.ReinstateRegistrationFromPendingWithdrawalAsync(request);
        }
    }
}