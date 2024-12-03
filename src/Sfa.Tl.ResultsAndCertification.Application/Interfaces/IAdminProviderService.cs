using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminProvider;
using System;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Application.Interfaces
{
    public interface IAdminProviderService
    {
        Task<GetProviderResponse> GetProviderAsync(int providerId);
        Task<AddProviderResponse> AddProviderAsync(AddProviderRequest addRequest);
        Task<UpdateProviderResponse> UpdateProviderAsync(UpdateProviderRequest updateRequest, Func<DateTime> getNow);
    }
}