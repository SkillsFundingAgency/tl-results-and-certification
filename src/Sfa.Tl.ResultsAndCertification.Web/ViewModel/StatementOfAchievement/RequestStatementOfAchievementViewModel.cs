using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using System;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.StatementOfAchievement
{
    public class RequestStatementOfAchievementViewModel
    {
        public bool IsSoaAvailable => SoaAvailableDate == null || DateTime.UtcNow >= SoaAvailableDate;
        public DateTime? SoaAvailableDate { get; set; }
        public BackLinkModel BackLink => new BackLinkModel { RouteName = RouteConstants.Home };
    }
}