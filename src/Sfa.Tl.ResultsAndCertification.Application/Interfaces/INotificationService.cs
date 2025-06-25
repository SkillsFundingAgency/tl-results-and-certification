using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Application.Interfaces
{
    public interface INotificationService
    {
        Task<bool> SendEmailNotificationAsync(string templateName, string toAddress, IDictionary<string, dynamic> tokens);

        Task<bool> SendEmailNotificationAsync(string templateName, List<string> recipients, IDictionary<string, dynamic> tokens);
    }
}
