using AutoMapper;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Common;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Comparer;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Learner;
using Sfa.Tl.ResultsAndCertification.Web.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.Assessment;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.IndustryPlacement;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.Loader
{
    public class AdminDashboardLoader : IAdminDashboardLoader
    {
        private readonly IResultsAndCertificationInternalApiClient _internalApiClient;
        private readonly IMapper _mapper;

        public AdminDashboardLoader(IResultsAndCertificationInternalApiClient internalApiClient, IMapper mapper)
        {
            _internalApiClient = internalApiClient;
            _mapper = mapper;
        }

        public async Task<AdminSearchLearnerFiltersViewModel> GetAdminSearchLearnerFiltersAsync()
        {
            AdminSearchLearnerFilters apiResponse = await _internalApiClient.GetAdminSearchLearnerFiltersAsync();
            return _mapper.Map<AdminSearchLearnerFiltersViewModel>(apiResponse);
        }

        public async Task<AdminSearchLearnerDetailsListViewModel> GetAdminSearchLearnerDetailsListAsync(AdminSearchLearnerCriteriaViewModel adminSearchCriteria)
        {
            var adminSearchLearnerRequest = _mapper.Map<AdminSearchLearnerRequest>(adminSearchCriteria);
            PagedResponse<AdminSearchLearnerDetail> apiResponse = await _internalApiClient.GetAdminSearchLearnerDetailsAsync(adminSearchLearnerRequest);
            return _mapper.Map<AdminSearchLearnerDetailsListViewModel>(apiResponse);
        }

        public async Task<TLearnerRecordViewModel> GetAdminLearnerRecordAsync<TLearnerRecordViewModel>(int registrationPathwayId)
        {
            AdminLearnerRecord learnerRecord = await _internalApiClient.GetAdminLearnerRecordAsync(registrationPathwayId);

            TLearnerRecordViewModel response = _mapper.Map<TLearnerRecordViewModel>(learnerRecord, opt =>
            {
                opt.Items[Constants.RegistrationPathwayId] = learnerRecord.RegistrationPathwayId;
            });

            return response;
        }

        public async Task<AdminCoreComponentViewModel> GetAdminLearnerRecordWithCoreComponents(int registrationPathwayId)
        {
            Task<AdminLearnerRecord> learnerRecordTask = _internalApiClient.GetAdminLearnerRecordAsync(registrationPathwayId);
            Task<IList<AssessmentSeriesDetails>> assessmentSeriesTask = _internalApiClient.GetAssessmentSeriesAsync();

            await Task.WhenAll(learnerRecordTask, assessmentSeriesTask);

            AdminLearnerRecord learnerRecord = learnerRecordTask.Result;
            IList<AssessmentSeriesDetails> assessmentSeries = assessmentSeriesTask.Result;

            var activeAssessmentIncludingPreviousYear = CommonHelper.GetValidAssessmentSeries(
                assessmentSeries, learnerRecord.Pathway.AcademicYear, learnerRecord.Pathway.StartYear, ComponentType.Core, true)
                .Select(a => new Assessment()
                {
                    SeriesId = a.Id,
                    SeriesName = a.Name
                });

            var validAssessments = activeAssessmentIncludingPreviousYear.Except(learnerRecord.Pathway.PathwayAssessments, new AssessmentComparer()).ToList();

            AdminCoreComponentViewModel response = _mapper.Map<AdminCoreComponentViewModel>(learnerRecord, opt =>
            {
                opt.Items[Constants.AdminValidAssessmentSeries] = validAssessments;
                opt.Items[Constants.RegistrationPathwayId] = learnerRecord.RegistrationPathwayId;
            });

            return response;
        }

        public async Task<AdminOccupationalSpecialismViewModel> GetAdminLearnerRecordWithOccupationalSpecialism(int registrationPathwayId, int specialismId)
        {
            Task<AdminLearnerRecord> learnerRecordTask = _internalApiClient.GetAdminLearnerRecordAsync(registrationPathwayId);
            Task<IList<AssessmentSeriesDetails>> assessmentSeriesTask = _internalApiClient.GetAssessmentSeriesAsync();

            await Task.WhenAll(learnerRecordTask, assessmentSeriesTask);

            AdminLearnerRecord learnerRecord = learnerRecordTask.Result;
            IList<AssessmentSeriesDetails> assessmentSeries = assessmentSeriesTask.Result;

            var activeAssessmentIncludingPreviousYear = CommonHelper.GetValidAssessmentSeries(assessmentSeries, learnerRecord.Pathway.AcademicYear, learnerRecord.Pathway.StartYear, ComponentType.Specialism, true)
                .Select(a => new Assessment()
                {
                    SeriesId = a.Id,
                    SeriesName = a.Name
                });

            var specialism = learnerRecord.Pathway?.Specialisms.FirstOrDefault(p => p.Id == specialismId);

            // exclude the existing ones where learner has entry.
            var validAssessments = activeAssessmentIncludingPreviousYear.Except(specialism.Assessments, new AssessmentComparer());

            AdminOccupationalSpecialismViewModel response = _mapper.Map<AdminOccupationalSpecialismViewModel>(learnerRecord, opt =>
            {
                opt.Items[Constants.AdminSpecialismAssessmentId] = specialismId;
                opt.Items[Constants.AdminValidAssessmentSeries] = validAssessments ?? default;
                opt.Items[Constants.RegistrationPathwayId] = learnerRecord.RegistrationPathwayId;
            });

            return response;
        }

        public async Task<bool> ProcessChangeStartYearAsync(ReviewChangeStartYearViewModel reviewChangeStartYearViewModel)
        {
            var reviewChangeRequest = _mapper.Map<ReviewChangeStartYearRequest>(reviewChangeStartYearViewModel);
            return await _internalApiClient.ProcessChangeStartYearAsync(reviewChangeRequest);

        }

        public async Task<bool> ProcessChangeIndustryPlacementAsync(AdminReviewChangesIndustryPlacementViewModel adminChangeIpViewModel)
        {
            var reviewChangeStartYearRequest = _mapper.Map<ReviewChangeIndustryPlacementRequest>(adminChangeIpViewModel);
            return await _internalApiClient.ProcessChangeIndustryPlacementAsync(reviewChangeStartYearRequest);

        }
    }
}