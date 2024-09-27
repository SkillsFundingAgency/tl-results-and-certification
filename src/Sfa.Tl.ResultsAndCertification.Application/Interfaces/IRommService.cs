using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.PostResultsService;
using Sfa.Tl.ResultsAndCertification.Models.PostResultsService.BulkProcess;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Application.Interfaces
{
    public interface IRommService
    {
        Task<IList<RommsRecordResponse>> ValidateRommTlevelsAsync(long aoUkprn, IEnumerable<RommCsvRecordResponse> withdrawalsData);

        Task<RommsProcessResponse> ProcessRommsAsync(long AoUkprn, IList<TqRegistrationProfile> registrations, IEnumerable<RommsRecordResponse> rommData, string performedBy);

        IList<TqRegistrationProfile> TransformRommModel(IList<RommsRecordResponse> withdrawalsData, string performedBy);

        Task<IList<RommsRecordResponse>> ValidateRommLearnersAsync(long aoUkprn, IEnumerable<RommCsvRecordResponse> validRommsData);
    }
}
