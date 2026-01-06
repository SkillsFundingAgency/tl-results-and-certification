using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminChangeLog;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces
{
    public interface IAdminChangeLogLoader
    {
        Task<AdminSearchChangeLogViewModel> SearchChangeLogsAsync(string searchKey = "", int? pageNumber = null);

        Task<AdminViewChangeRecordViewModel> GetAdminViewChangeStartYearRecord(int changeLogId);

        Task<AdminViewChangeRecordViewModel> GetAdminViewChangeIPRecord(int changeLogId);

        Task<AdminViewChangeRecordViewModel> GetAdminViewChangeCoreAssessmentRecord(int changeLogId);

        Task<AdminViewChangeRecordViewModel> GetAdminViewChangeSpecialismAssessmentRecord(int changeLogId);

        Task<AdminViewChangeRecordViewModel> GetAdminViewChangeAddPathwayResultRecord(int changeLogId);

        Task<AdminViewChangeRecordViewModel> GetAdminViewChangeAddSpecialismResultRecord(int changeLogId);

        Task<AdminViewChangeRecordViewModel> GetAdminViewChangeRemoveCoreAssessmentRecord(int changeLogId);

        Task<AdminViewChangeRecordViewModel> GetAdminViewChangeRemoveSpecialismAssessmentRecord(int changeLogId);

        Task<AdminViewChangeRecordViewModel> GetAdminViewChangePathwayResultRecord(int changeLogId);

        Task<AdminViewChangeRecordViewModel> GetAdminViewChangeSpecialismResultRecord(int changeLogId);

        Task<AdminViewChangeRecordViewModel> GetAdminViewChangeOpenPathwayRommRecord(int changeLogId);

        Task<AdminViewChangeRecordViewModel> GetAdminViewChangeOpenSpecialismRommRecord(int changeLogId);

        Task<AdminViewChangeRecordViewModel> GetAdminViewChangePathwayRommOutcomeRecord(int changeLogId);

        Task<AdminViewChangeRecordViewModel> GetAdminViewChangeSpecialismRommOutcomeRecord(int changeLogId);

        Task<AdminViewChangeRecordViewModel> GetAdminViewChangeOpenPathwayAppealRecord(int changeLogId);

        Task<AdminViewChangeRecordViewModel> GetAdminViewChangeOpenSpecialismAppealRecord(int changeLogId);

        Task<AdminViewChangeRecordViewModel> GetAdminViewChangePathwayAppealOutcomeRecord(int changeLogId);

        Task<AdminViewChangeRecordViewModel> GetAdminViewChangeSpecialismAppealOutcomeRecord(int changeLogId);
    }
}