using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Registration;
using Sfa.Tl.ResultsAndCertification.Models.Registration.BulkProcess;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Application.Interfaces
{
    public interface IWithdrawalService
    {
        Task<IList<WithdrawalRecordResponse>> ValidateWithdrawalTlevelsAsync(long aoUkprn, IEnumerable<WithdrawalCsvRecordResponse> withdrawalsData);

        Task<WithdrawalProcessResponse> ProcessWithdrawalsAsync(long AoUkprn, IList<TqRegistrationProfile> registrations, string performedBy);

        IList<TqRegistrationProfile> TransformWithdrawalModel(IList<WithdrawalRecordResponse> withdrawalsData, string performedBy);

        Task<IList<WithdrawalRecordResponse>> ValidateWithdrawalLearnersAsync(long aoUkprn, IEnumerable<WithdrawalCsvRecordResponse> validWithdrawalsData);
    }
}
