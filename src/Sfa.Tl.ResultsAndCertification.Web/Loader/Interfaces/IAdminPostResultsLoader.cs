using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminPostResults;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces
{
    public interface IAdminPostResultsLoader
    {
        Task<AdminOpenPathwayRommViewModel> GetAdminOpenPathwayRommAsync(int registrationPathwayId, int pathwayAssessmentId);

        AdminOpenPathwayRommReviewChangesViewModel GetAdminOpenPathwayRommReviewChangesAsync(AdminOpenPathwayRommViewModel openPathwayRommViewModel);

        Task<bool> ProcessAdminOpenPathwayRommAsync(AdminOpenPathwayRommReviewChangesViewModel openPathwayRommReviewChangesViewModel);

        Task<AdminOpenSpecialismRommViewModel> GetAdminOpenSpecialismRommAsync(int registrationPathwayId, int specialismAssessmentId);

        AdminOpenSpecialismRommReviewChangesViewModel GetAdminOpenSpecialismRommReviewChangesAsync(AdminOpenSpecialismRommViewModel openSpecialismRommViewModel);

        Task<bool> ProcessAdminOpenSpecialismRommAsync(AdminOpenSpecialismRommReviewChangesViewModel openSpecialismRommReviewChangesViewModel);

        Task<AdminOpenPathwayAppealViewModel> GetAdminOpenPathwayAppealAsync(int registrationPathwayId, int pathwayAssessmentId);

        Task<AdminOpenSpecialismAppealViewModel> GetAdminOpenSpecialismAppealAsync(int registrationPathwayId, int pathwayAssessmentId);

        Task<AdminAddCoreRommOutcomeViewModel> GetAdminAddPathwayRommOutcomeAsync(int registrationPathwayId, int pathwayAssessmentId);

        Task<AdminAddSpecialismRommOutcomeViewModel> GetAdminAddSpecialismRommOutcomeAsync(int registrationPathwayId, int pathwayAssessmentId);

        Task<AdminAddRommOutcomeChangeGradeCoreViewModel> GetAdminAddRommOutcomeChangeGradeCoreAsync(int registrationPathwayId, int assessmentId);
        Task LoadAdminAddRommOutcomeChangeGradeCoreGrades(AdminAddRommOutcomeChangeGradeCoreViewModel model);

        Task<AdminAddRommOutcomeChangeGradeSpecialismViewModel> GetAdminAddRommOutcomeChangeGradeSpecialismAsync(int registrationPathwayId, int assessmentId);
        Task LoadAdminAddRommOutcomeChangeGradeSpecialismGrades(AdminAddRommOutcomeChangeGradeSpecialismViewModel model);

        AdminReviewChangesRommOutcomeCoreViewModel GetAdminReviewChangesRommOutcomeCoreAsync<TAddRommOutcomeChangeGradeCoreViewModel>(TAddRommOutcomeChangeGradeCoreViewModel revieChangeRommOutcomeCoreViewModel);
        Task<bool> ProcessAdminReviewChangesRommOutcomeCoreAsync(AdminReviewChangesRommOutcomeCoreViewModel revieChangeRommOutcomeCoreViewModel);

        AdminReviewChangesRommOutcomeSpecialismViewModel GetAdminReviewChangesRommOutcomeSpecialismAsync<TAddRommOutcomeChangeGradeSpecialismViewModel>(TAddRommOutcomeChangeGradeSpecialismViewModel revieChangeRommOutcomeSpecialismViewModel);

        Task<bool> ProcessAdminReviewChangesRommOutcomeSpecialismAsync(AdminReviewChangesRommOutcomeSpecialismViewModel revieChangeRommOutcomeSpecialismViewModel);

        Task<AdminAppealCoreReviewChangesViewModel> GetAdminAppealCoreReviewChangesAsync(int registrationPathwayId, int pathwayAssessmentId);

        Task<AdminAppealSpecialismReviewChangesViewModel> GetAdminAppealSpecialismReviewChangesAsync(int registrationPathwayId, int specialismAssessmentId);

        Task<bool> ProcessAdminOpenCoreAppealAsync(AdminAppealCoreReviewChangesViewModel openppealCoreReviewChangesViewModel);

        Task<bool> ProcessAdminOpenSpecialismAppealAsync(AdminAppealSpecialismReviewChangesViewModel openppealSpecialismReviewChangesViewModel);

        Task<AdminAddCoreAppealOutcomeViewModel> GetAdminAddPathwayAppealOutcomeAsync(int registrationPathwayId, int assessmentId);

        Task<AdminAddAppealOutcomeChangeGradeCoreViewModel> GetAdminAddAppealOutcomeChangeGradeCoreAsync(int registrationPathwayId, int assessmentId);
        Task LoadAdminAddAppealOutcomeChangeGradeCoreGrades(AdminAddAppealOutcomeChangeGradeCoreViewModel model);

        Task<AdminAddSpecialismAppealOutcomeViewModel> GetAdminAddSpecialismAppealOutcomeAsync(int registrationPathwayId, int assessmentId);

        Task<AdminAddAppealOutcomeChangeGradeSpecialismViewModel> GetAdminAddAppealOutcomeChangeGradeSpecialismAsync(int registrationPathwayId, int assessmentId);
        Task LoadAdminAddAppealOutcomeChangeGradeSpecialismGrades(AdminAddAppealOutcomeChangeGradeSpecialismViewModel model);

        AdminReviewChangesAppealOutcomeCoreViewModel GetAdminReviewChangesAppealOutcomeCoreAsync<TAddAppealOutcomeChangeGradeCoreViewModel>(TAddAppealOutcomeChangeGradeCoreViewModel addAppealcachedModel);

        Task<bool> ProcessAdminReviewChangesAppealOutcomeCoreAsync(AdminReviewChangesAppealOutcomeCoreViewModel model);

        AdminReviewChangesAppealOutcomeSpecialismViewModel GetAdminReviewChangesAppealOutcomeSpecialismAsync<TAddSpecialismOutcomeChangeGradeSpecialismViewModel>(TAddSpecialismOutcomeChangeGradeSpecialismViewModel addAppealcachedModel);

        Task<bool> ProcessAdminReviewChangesAppealOutcomeSpecialismAsync(AdminReviewChangesAppealOutcomeSpecialismViewModel model);

    }
}