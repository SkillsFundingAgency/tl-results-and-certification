using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces
{
    public interface IRegistrationLoader
    {
        Task<UploadRegistrationsResponseViewModel> ProcessBulkRegistrationsAsync(UploadRegistrationsRequestViewModel viewModel);
    }
}
