using Sfa.Tl.ResultsAndCertification.Models.Functions;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Application.Interfaces
{
    public interface IIndustryPlacementNotificationService
    {
        Task<IndustryPlacementNotificationResponse> ProcessIndustryPlacementFirstDeadlineReminderAsync();
        Task<IndustryPlacementNotificationResponse> ProcessIndustryPlacementMissedDeadlineReminderAsync();
        Task<IndustryPlacementNotificationResponse> ProcessIndustryPlacementChaseBigGapsReminderAsync();
        Task<IndustryPlacementNotificationResponse> ProcessIndustryPlacementOneOutstandingUlnReminderAsync();
    }
}