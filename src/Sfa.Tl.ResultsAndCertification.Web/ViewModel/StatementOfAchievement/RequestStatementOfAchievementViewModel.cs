using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using System;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.StatementOfAchievement
{
    public class RequestStatementOfAchievementViewModel
    {
        public BackLinkModel BackLink => new BackLinkModel { RouteName = RouteConstants.Home };
    }
}