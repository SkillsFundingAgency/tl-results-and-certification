﻿using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService
{
    public class PostResultServiceUlnNotFoundViewModel
    {
        public string Uln { get; set; }

        public virtual BackLinkModel BackLink => new BackLinkModel { RouteName = RouteConstants.SearchPostResultsService };
    }
}
