using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Registration;
using Sfa.Tl.ResultsAndCertification.Models.Registration.BulkProcess;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Application.Interfaces
{
    public interface IWithdrawlService
    {
        Task<IList<WithdrawlRecordResponse>> ValidateWithdrawlTlevelsAsync(long aoUkprn, IEnumerable<WithdrawlCsvRecordResponse> withdrawlsData);

        Task<WithdrawlProcessResponse> ProcessWithdrawlsAsync(long AoUkprn, IList<TqRegistrationProfile> registrations, string performedBy);

        IList<TqRegistrationProfile> TransformWithdrawlModel(IList<WithdrawlRecordResponse> withdrawlsData, string performedBy);

        Task<IList<WithdrawlRecordResponse>> ValidateWithdrawlLearnersAsync(long aoUkprn, IEnumerable<WithdrawlCsvRecordResponse> validWithdrawlsData);
    }
}
