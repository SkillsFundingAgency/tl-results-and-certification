using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.DataExport;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Result;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Result.Manual;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces
{
    public interface IResultLoader
    {
        Task<UploadResultsResponseViewModel> ProcessBulkResultsAsync(UploadResultsRequestViewModel viewModel);
        Task<Stream> GetResultValidationErrorsFileAsync(long aoUkprn, Guid blobUniqueReference);

        Task<UlnResultsNotFoundViewModel> FindUlnResultsAsync(long aoUkprn, long Uln);
        Task<ResultWithdrawnViewModel> GetResultWithdrawnViewModelAsync(long aoUkprn, int profileId);
        Task<ResultDetailsViewModel> GetResultDetailsAsync(long aoUkprn, int profileId, RegistrationPathwayStatus? status = null);

        Task<AddResultResponse> AddCoreResultAsync(long aoUkprn, ManageCoreResultViewModel viewModel);
        Task<ManageCoreResultViewModel> GetManageCoreResultAsync(long aoUkprn, int profileId, int assessmentId, bool isChangeMode);
        Task<bool?> IsCoreResultChangedAsync(long aoUkprn, ManageCoreResultViewModel model);
        Task<ChangeResultResponse> ChangeCoreResultAsync(long aoUkprn, ManageCoreResultViewModel viewModel);
        Task<IList<DataExportResponse>> GenerateResultsExportAsync(long aoUkprn, string requestedBy);
        Task<Stream> GetResultsDataFileAsync(long aoUkprn, Guid blobUniqueReference, ComponentType componentType);
        Task<ManageSpecialismResultViewModel> GetManageSpecialismResultAsync(long aoUkprn, int profileId, int assessmentId, bool isChangeMode);
    }
}
