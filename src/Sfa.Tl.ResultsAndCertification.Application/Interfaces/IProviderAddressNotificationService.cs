using Sfa.Tl.ResultsAndCertification.Models.Functions;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Application.Interfaces
{
    public interface IProviderAddressNotificationService
    {
        Task<ProviderAddressNotificationResponse> ProcessProviderAddressMissingReminderAsync(int acadamicYearToProcess);

        Task<ProviderAddressNotificationResponse> ProcessProviderAddressValidationReminderAsync(int academicYearToProcess);
    }
}