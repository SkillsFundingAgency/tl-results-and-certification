using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.DataExport;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.ProviderRegistrations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces
{
    public interface IProviderRegistrationsLoader
    {
        Task<IList<AvailableStartYearViewModel>> GetAvailableStartYearsAsync();
        Task<DataExportResponse> GetRegistrationsDataExportAsync(long providerUkprn, int startYear, string requestedBy);
        DownloadRegistrationsDataForViewModel GetDownloadRegistrationsDataForViewModel(DataExportResponse dataExportResponse, int startYear);
        Task<FileStreamResult> GetRegistrationsFileAsync(long providerUkprn, Guid blobUniqueReference);
    }
}