using Sfa.Tl.ResultsAndCertification.Models.Contracts.StatementOfAchievement;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.StatementOfAchievement;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces
{
    public interface IStatementOfAchievementLoader
    {
        Task<FindSoaLearnerRecord> FindSoaLearnerRecordAsync(long providerUkprn, long uln);
        Task<SoaLearnerRecordDetailsViewModel> GetSoaLearnerRecordDetailsAsync(long providerUkprn, int profileId);
        Task<SoaPrintingResponse> CreateSoaPrintingRequestAsync(long providerUkprn, SoaLearnerRecordDetailsViewModel viewModel);
    }
}