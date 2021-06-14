using Sfa.Tl.ResultsAndCertification.Models.Contracts.StatementOfAchievement;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.InternalApi.Interfaces
{
    public interface IStatementOfAchievementController
    {
        Task<FindSoaLearnerRecord> FindSoaLearnerRecordAsync(long providerUkprn, long uln);
        Task<SoaLearnerRecordDetails> GetSoaLearnerRecordDetailsAsync(long providerUkprn, int profileId);
        Task<SoaPrintingResponse> CreateSoaPrintingRequestAsync(SoaPrintingRequest request);
        Task<PrintRequestSnapshot> GetPrintRequestSnapshotAsync(long providerUkprn, int profileId, int pathwayId);
    }
}