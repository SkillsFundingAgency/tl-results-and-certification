using AutoMapper;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminPostResults;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Common;
using Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminPostResults;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.Loader
{
    public class AdminPostResultsLoader : IAdminPostResultsLoader
    {
        private readonly IResultsAndCertificationInternalApiClient _internalApiClient;
        private readonly IMapper _mapper;

        public AdminPostResultsLoader(
            IResultsAndCertificationInternalApiClient internalApiClient,
            IMapper mapper)
        {
            _internalApiClient = internalApiClient;
            _mapper = mapper;
        }

        public Task<AdminOpenPathwayRommViewModel> GetAdminOpenPathwayRommAsync(int registrationPathwayId, int pathwayAssessmentId)
            => GetAndMapLearnerRecordAsync<AdminOpenPathwayRommViewModel>(registrationPathwayId, pathwayAssessmentId);

        public AdminOpenPathwayRommReviewChangesViewModel GetAdminOpenPathwayRommReviewChangesAsync(AdminOpenPathwayRommViewModel openPathwayRommViewModel)
            => _mapper.Map<AdminOpenPathwayRommReviewChangesViewModel>(openPathwayRommViewModel);

        public Task<bool> ProcessAdminOpenPathwayRommAsync(AdminOpenPathwayRommReviewChangesViewModel openPathwayRommReviewChangesViewModel)
        {
            var request = _mapper.Map<OpenPathwayRommRequest>(openPathwayRommReviewChangesViewModel);
            return _internalApiClient.ProcessAdminOpenPathwayRommAsync(request);
        }

        public Task<AdminOpenSpecialismRommViewModel> GetAdminOpenSpecialismRommAsync(int registrationPathwayId, int specialismAssessmentId)
            => GetAndMapLearnerRecordAsync<AdminOpenSpecialismRommViewModel>(registrationPathwayId, specialismAssessmentId);

        public AdminOpenSpecialismRommReviewChangesViewModel GetAdminOpenSpecialismRommReviewChangesAsync(AdminOpenSpecialismRommViewModel openSpecialismRommViewModel)
           => _mapper.Map<AdminOpenSpecialismRommReviewChangesViewModel>(openSpecialismRommViewModel);

        public Task<bool> ProcessAdminOpenSpecialismRommAsync(AdminOpenSpecialismRommReviewChangesViewModel openSpecialismRommReviewChangesViewModel)
        {
            var request = _mapper.Map<OpenSpecialismRommRequest>(openSpecialismRommReviewChangesViewModel);
            return _internalApiClient.ProcessAdminOpenSpecialismRommAsync(request);
        }

        public Task<AdminAddCoreRommOutcomeViewModel> GetAdminAddPathwayRommOutcomeAsync(int registrationPathwayId, int pathwayAssessmentId)
            => GetAndMapLearnerRecordAsync<AdminAddCoreRommOutcomeViewModel>(registrationPathwayId, pathwayAssessmentId);

        public Task<AdminAddSpecialismRommOutcomeViewModel> GetAdminAddSpecialismRommOutcomeAsync(int registrationPathwayId, int pathwayAssessmentId)
            => GetAndMapLearnerRecordAsync<AdminAddSpecialismRommOutcomeViewModel>(registrationPathwayId, pathwayAssessmentId);


        public Task<AdminOpenPathwayAppealViewModel> GetAdminOpenPathwayAppealAsync(int registrationPathwayId, int pathwayAssessmentId)
            => GetAndMapLearnerRecordAsync<AdminOpenPathwayAppealViewModel>(registrationPathwayId, pathwayAssessmentId);

        public Task<AdminOpenSpecialismAppealViewModel> GetAdminOpenSpecialismAppealAsync(int registrationPathwayId, int pathwayAssessmentId)
            => GetAndMapLearnerRecordAsync<AdminOpenSpecialismAppealViewModel>(registrationPathwayId, pathwayAssessmentId);

        public async Task<AdminAppealCoreReviewChangesViewModel> GetAdminAppealCoreReviewChangesAsync(int registrationPathwayId, int pathwayAssessmentId)
        {
            AdminLearnerRecord learnerRecord = await _internalApiClient.GetAdminLearnerRecordAsync(registrationPathwayId);

            return _mapper.Map<AdminAppealCoreReviewChangesViewModel>(learnerRecord, opt =>
            {
                opt.Items[Constants.AssessmentId] = pathwayAssessmentId;
            });

        }

        public async Task<AdminAppealSpecialismReviewChangesViewModel> GetAdminAppealSpecialismReviewChangesAsync(int registrationPathwayId, int specialismAssessmentId)
        {
            AdminLearnerRecord learnerRecord = await _internalApiClient.GetAdminLearnerRecordAsync(registrationPathwayId);

            return _mapper.Map<AdminAppealSpecialismReviewChangesViewModel>(learnerRecord, opt =>
            {
                opt.Items[Constants.AssessmentId] = specialismAssessmentId;
            });
        }

        public async Task<bool> ProcessAdminOpenCoreAppealAsync(AdminAppealCoreReviewChangesViewModel openppealCoreReviewChangesViewModel)
        {
            var request = _mapper.Map<OpenCoreAppealRequest>(openppealCoreReviewChangesViewModel);
            return await _internalApiClient.ProcessAdminOpenCoreAppealAsync(request);
        }

        public async Task<bool> ProcessAdminOpenSpecialismAppealAsync(AdminAppealSpecialismReviewChangesViewModel openppealSpecialismReviewChangesViewModel)
        {
            var request = _mapper.Map<OpenSpecialismAppealRequest>(openppealSpecialismReviewChangesViewModel);
            return await _internalApiClient.ProcessAdminOpenSpecialismAppealAsync(request);

        }

        private async Task<T> GetAndMapLearnerRecordAsync<T>(int registrationPathwayId, int pathwayAssessmentId)
        {
            AdminLearnerRecord learnerRecord = await _internalApiClient.GetAdminLearnerRecordAsync(registrationPathwayId);
            return _mapper.Map<T>(learnerRecord, opt =>
            {
                opt.Items[Constants.AssessmentId] = pathwayAssessmentId;
            });
        }

        public async Task<AdminAddRommOutcomeChangeGradeCoreViewModel> GetAdminAddRommOutcomeChangeGradeCoreAsync(int registrationPathwayId, int assessmentId)
        {
            var viewmodel = await GetAdminAddResultAsync<AdminAddRommOutcomeChangeGradeCoreViewModel>(registrationPathwayId, assessmentId, LookupCategory.PathwayComponentGrade, false);
            viewmodel.Grades = GetAdminAddRommOutcomeChangeGradeCoreGrades(viewmodel.Grades);
            viewmodel.Grades.Remove(viewmodel.Grades.FirstOrDefault(t => t.Value == viewmodel.Grade));
            return viewmodel;
        }

        public async Task LoadAdminAddRommOutcomeChangeGradeCoreGrades(AdminAddRommOutcomeChangeGradeCoreViewModel model)
         => model.Grades = GetAdminAddRommOutcomeChangeGradeCoreGrades(await GetAdminChangeResultGrades(LookupCategory.PathwayComponentGrade, model.Grade, true));

        private List<LookupViewModel> GetAdminAddRommOutcomeChangeGradeCoreGrades(List<LookupViewModel> Grades)
        {
            return Grades.Where(t => !t.Code.Equals(Constants.PathwayComponentGradeQpendingResultCode, StringComparison.InvariantCultureIgnoreCase)
            && !t.Code.Equals(Constants.PathwayComponentGradeXNoResultCode, StringComparison.InvariantCultureIgnoreCase)
            && !t.Code.Equals(Constants.NotReceived, StringComparison.InvariantCultureIgnoreCase)).ToList();
        }

        public async Task<AdminAddRommOutcomeChangeGradeSpecialismViewModel> GetAdminAddRommOutcomeChangeGradeSpecialismAsync(int registrationPathwayId, int assessmentId)
        {
            var viewmodel = await GetAdminAddResultAsync<AdminAddRommOutcomeChangeGradeSpecialismViewModel>(registrationPathwayId, assessmentId, LookupCategory.SpecialismComponentGrade, false);
            viewmodel.Grades = GetAdminAddRommOutcomeChangeGradeSpecialismGrades(viewmodel.Grades);
            viewmodel.Grades.Remove(viewmodel.Grades.FirstOrDefault(t => t.Value == viewmodel.Grade));
            return viewmodel;
        }

        public async Task LoadAdminAddRommOutcomeChangeGradeSpecialismGrades(AdminAddRommOutcomeChangeGradeSpecialismViewModel model)
            => model.Grades = GetAdminAddRommOutcomeChangeGradeSpecialismGrades(await GetAdminChangeResultGrades(LookupCategory.SpecialismComponentGrade, model.Grade, true));

        private List<LookupViewModel> GetAdminAddRommOutcomeChangeGradeSpecialismGrades(List<LookupViewModel> Grades)
        {
            return Grades.Where(t => !t.Code.Equals(Constants.SpecialismComponentGradeQpendingResultCode, StringComparison.InvariantCultureIgnoreCase)
            && !t.Code.Equals(Constants.SpecialismComponentGradeXNoResultCode, StringComparison.InvariantCultureIgnoreCase)
            && !t.Code.Equals(Constants.NotReceived, StringComparison.InvariantCultureIgnoreCase)).ToList();
        }

        private async Task<List<LookupViewModel>> GetAdminChangeResultGrades(LookupCategory lookupCategory, string grade, bool isRomm = false)
        {
            IList<LookupData> grades = await _internalApiClient.GetLookupDataAsync(lookupCategory);
            grades.Remove(grades.FirstOrDefault((t => t.Value == grade)));
            if (!isRomm) grades.Add(new LookupData { Code = Constants.NotReceived, Value = Content.Result.ManageSpecialismResult.Option_Remove_Result });
            return _mapper.Map<List<LookupViewModel>>(grades);
        }

        private async Task<TAddResultViewModel> GetAdminAddResultAsync<TAddResultViewModel>(int registrationPathwayId, int assessmentId, LookupCategory lookupCategory, bool ischange = false)
            where TAddResultViewModel : class
        {
            Task<AdminLearnerRecord> learnerRecordTask = _internalApiClient.GetAdminLearnerRecordAsync(registrationPathwayId);
            Task<IList<LookupData>> gradesTask = _internalApiClient.GetLookupDataAsync(lookupCategory);

            await Task.WhenAll(learnerRecordTask, gradesTask);

            AdminLearnerRecord learnerRecord = learnerRecordTask.Result;
            IList<LookupData> grades = gradesTask.Result;

            if (learnerRecord == null || grades == null)
                return null;

            if (ischange)
            {
                grades.Add(new LookupData { Code = Constants.NotReceived, Value = Content.Result.ManageSpecialismResult.Option_Remove_Result });
            }


            return _mapper.Map<TAddResultViewModel>(learnerRecord, opt =>
            {
                opt.Items[Constants.AssessmentId] = assessmentId;
                opt.Items["grades"] = grades;
            });
        }
    }
}