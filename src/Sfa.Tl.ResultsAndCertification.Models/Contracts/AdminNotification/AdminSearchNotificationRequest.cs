using System.Collections.Generic;
using System.Linq;

namespace Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminNotification
{
    public class AdminSearchNotificationRequest
    {
        public IEnumerable<int> SelectedActive { get; set; } = Enumerable.Empty<int>();

        public int? PageNumber { get; set; }
    }
}