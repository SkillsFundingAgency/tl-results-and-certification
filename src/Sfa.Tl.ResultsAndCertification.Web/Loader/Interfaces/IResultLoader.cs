using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Result;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces
{
    public interface IResultLoader
    {
        Task<UploadResultsResponseViewModel> ProcessBulkResultsAsync(UploadResultsRequestViewModel viewModel);
        Task<Stream> GetResultValidationErrorsFileAsync(long aoUkprn, Guid blobUniqueReference);
    }
}
