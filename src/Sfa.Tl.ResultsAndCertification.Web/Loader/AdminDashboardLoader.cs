using AutoMapper;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Common;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Comparer;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Learner;
using Sfa.Tl.ResultsAndCertification.Web.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.Assessment;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.LearnerRecord;
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

        public async Task<AdminLearnerRecordViewModel> GetAdminLearnerRecordAsync(int registrationPathwayId)
        {
            Task<AdminLearnerRecord> learnerRecordTask = _internalApiClient.GetAdminLearnerRecordAsync(registrationPathwayId);
            Task<IList<AssessmentSeriesDetails>> assessmentSeriesTask = _internalApiClient.GetAssessmentSeriesAsync();

            await Task.WhenAll(learnerRecordTask, assessmentSeriesTask);

            AdminLearnerRecord learnerRecord = learnerRecordTask.Result;
            IList<AssessmentSeriesDetails> assessmentSeries = assessmentSeriesTask.Result;

            Pathway pathway = learnerRecord.Pathway;

            AdminLearnerRecordViewModel response = _mapper.Map<AdminLearnerRecordViewModel>(learnerRecord, opt =>
            {
                opt.Items["registrationPathwayId"] = learnerRecord.RegistrationPathwayId;
                opt.Items["currentCoreAssessmentSeriesId"] = CommonHelper.GetValidAssessmentSeries(assessmentSeries, pathway.AcademicYear, pathway.StartYear, ComponentType.Core)?.FirstOrDefault()?.Id ?? 0;
                opt.Items["coreSeriesName"] = CommonHelper.GetNextAvailableAssessmentSeries(assessmentSeries, pathway.AcademicYear, pathway.StartYear, ComponentType.Core)?.Name;
                opt.Items["currentSpecialismAssessmentSeriesId"] = CommonHelper.GetValidAssessmentSeries(assessmentSeries, pathway.AcademicYear, pathway.StartYear, ComponentType.Specialism)?.FirstOrDefault()?.Id ?? 0;
                opt.Items["specialismSeriesName"] = CommonHelper.GetNextAvailableAssessmentSeries(assessmentSeries, pathway.AcademicYear, pathway.StartYear, ComponentType.Specialism)?.Name;
            });

            return response;
        }

        public async Task<TLearnerRecordViewModel> GetAdminLearnerRecordAsync<TLearnerRecordViewModel>(int registrationPathwayId)
        {
            var response = await _internalApiClient.GetAdminLearnerRecordAsync(registrationPathwayId);
            return _mapper.Map<TLearnerRecordViewModel>(response);
        }

        public async Task<AdminCoreAssessmentViewModel> GetAdminLearnerRecordWithCoreAssesments(int registrationPathwayId)
        {
            Task<AdminLearnerRecord> learnerRecordTask = _internalApiClient.GetAdminLearnerRecordAsync(registrationPathwayId);
            Task<IList<AssessmentSeriesDetails>> assessmentSeriesTask = _internalApiClient.GetAssessmentSeriesAsync();

            await Task.WhenAll(learnerRecordTask, assessmentSeriesTask);

            AdminLearnerRecord learnerRecord = learnerRecordTask.Result;
            IList<AssessmentSeriesDetails> assessmentSeries = assessmentSeriesTask.Result;

            var availableAssessmentSeries = CommonHelper.GetValidAssessmentSeries(
                assessmentSeries, learnerRecord.Pathway.AcademicYear, learnerRecord.Pathway.StartYear, ComponentType.Core, true)
                .Select(a => new Assessment()
                {
                    SeriesId = a.Id,
                    SeriesName = a.Name,
                    ComponentType = a.ComponentType
                });

            learnerRecord.AvailableAssessments = availableAssessmentSeries.Except(learnerRecord.Pathway.PathwayAssessments, new AssessmentComparer()).ToList();

            return _mapper.Map<AdminCoreAssessmentViewModel>(learnerRecord);
        }

        public async Task<bool> ProcessChangeStartYearAsync(ReviewChangeStartYearViewModel reviewChangeStartYearViewModel)
        {
            var reviewChangeStartYearRequest = _mapper.Map<ReviewChangeStartYearRequest>(reviewChangeStartYearViewModel);
            return await _internalApiClient.ProcessChangeStartYearAsync(reviewChangeStartYearRequest);
        }
    }
}