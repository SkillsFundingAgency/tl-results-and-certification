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
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.Result;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Common;
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

            List<Assessment> validAssessments = new();

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

            if (specialism != null)
            {
                // exclude the existing ones where learner has entry.
                validAssessments = activeAssessmentIncludingPreviousYear.Except(specialism.Assessments, new AssessmentComparer()).ToList();
            }

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
            var reviewIndustryPlacementRequest = _mapper.Map<ReviewChangeIndustryPlacementRequest>(adminChangeIpViewModel);
            return await _internalApiClient.ProcessChangeIndustryPlacementAsync(reviewIndustryPlacementRequest);
        }

        #region Remove assessment

        public Task<AdminRemovePathwayAssessmentEntryViewModel> GetRemovePathwayAssessmentEntryAsync(int registrationPathwayId, int pathwayAssessmentId)
           => GetRemoveAssessmentEntryAsync<AdminRemovePathwayAssessmentEntryViewModel>(registrationPathwayId, pathwayAssessmentId);

        public Task<AdminRemoveSpecialismAssessmentEntryViewModel> GetRemoveSpecialismAssessmentEntryAsync(int registrationPathwayId, int specialismAssessmentId)
            => GetRemoveAssessmentEntryAsync<AdminRemoveSpecialismAssessmentEntryViewModel>(registrationPathwayId, specialismAssessmentId);

        private async Task<TRemoveAssessmentEntryViewModel> GetRemoveAssessmentEntryAsync<TRemoveAssessmentEntryViewModel>(int registrationPathwayId, int assessmentId)
        {
            AdminLearnerRecord learnerRecord = await _internalApiClient.GetAdminLearnerRecordAsync(registrationPathwayId);

            return _mapper.Map<TRemoveAssessmentEntryViewModel>(learnerRecord, opt =>
            {
                opt.Items[Constants.AssessmentId] = assessmentId;
            });
        }

        public async Task<bool> ProcessRemoveAssessmentEntry(AdminReviewRemoveCoreAssessmentEntryViewModel model)
        {
            var reviewRemoveAssessmentEntryRequest = _mapper.Map<ReviewRemoveAssessmentEntryRequest>(model);
            return await _internalApiClient.RemoveAssessmentEntryAsync(reviewRemoveAssessmentEntryRequest);
        }

        public async Task<bool> ProcessRemoveSpecialismAssessmentEntryAsync(AdminReviewRemoveSpecialismAssessmentEntryViewModel model)
        {
            var reviewRemoveSpecialismEntryRequest = _mapper.Map<ReviewRemoveAssessmentEntryRequest>(model);
            return await _internalApiClient.RemoveSpecialAssessmentEntryAsync(reviewRemoveSpecialismEntryRequest);
        }

        #endregion

        #region Add result

        public Task<AdminAddPathwayResultViewModel> GetAdminAddPathwayResultAsync(int registrationPathwayId, int assessmentId)
            => GetAdminAddResultAsync<AdminAddPathwayResultViewModel>(registrationPathwayId, assessmentId, LookupCategory.PathwayComponentGrade);

        public async Task LoadAdminAddPathwayResultGrades(AdminAddPathwayResultViewModel model)
            => model.Grades = await GetAdminAddResultGrades(LookupCategory.PathwayComponentGrade);

        public Task<AdminAddSpecialismResultViewModel> GetAdminAddSpecialismResultAsync(int registrationPathwayId, int assessmentId)
            => GetAdminAddResultAsync<AdminAddSpecialismResultViewModel>(registrationPathwayId, assessmentId, LookupCategory.SpecialismComponentGrade);

        public async Task LoadAdminAddSpecialismResultGrades(AdminAddSpecialismResultViewModel model)
            => model.Grades = await GetAdminAddResultGrades(LookupCategory.SpecialismComponentGrade);

        private async Task<TAddResultViewModel> GetAdminAddResultAsync<TAddResultViewModel>(int registrationPathwayId, int assessmentId, LookupCategory lookupCategory)
            where TAddResultViewModel : class
        {
            Task<AdminLearnerRecord> learnerRecordTask = _internalApiClient.GetAdminLearnerRecordAsync(registrationPathwayId);
            Task<IList<LookupData>> gradesTask = _internalApiClient.GetLookupDataAsync(lookupCategory);

            await Task.WhenAll(learnerRecordTask, gradesTask);

            AdminLearnerRecord learnerRecord = learnerRecordTask.Result;
            IList<LookupData> grades = gradesTask.Result;

            if (learnerRecord == null || grades == null)
                return null;

            return _mapper.Map<TAddResultViewModel>(learnerRecord, opt =>
            {
                opt.Items[Constants.AssessmentId] = assessmentId;
                opt.Items["grades"] = grades;
            });
        }

        private async Task<List<LookupViewModel>> GetAdminAddResultGrades(LookupCategory lookupCategory)
        {
            IList<LookupData> grades = await _internalApiClient.GetLookupDataAsync(lookupCategory);
            return _mapper.Map<List<LookupViewModel>>(grades);
        }

        public AdminAddPathwayResultReviewChangesViewModel CreateAdminAddPathwayResultReviewChanges(AdminAddPathwayResultViewModel model)
            => _mapper.Map<AdminAddPathwayResultReviewChangesViewModel>(model);

        public Task<bool> ProcessChangeIndustryPlacementAsync(AdminAddPathwayResultReviewChangesViewModel model)
        {
            var request = _mapper.Map<AddPathwayResultRequest>(model);
            return _internalApiClient.ProcessAdminAddPathwayResultAsync(request);
        }

        #endregion
    }
}