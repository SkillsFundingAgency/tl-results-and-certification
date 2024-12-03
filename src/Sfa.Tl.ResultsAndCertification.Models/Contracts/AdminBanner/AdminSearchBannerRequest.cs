using Sfa.Tl.ResultsAndCertification.Common.Enum;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminBanner
{
    public class AdminSearchBannerRequest
    {
        public IList<BannerTarget> SelectedTargets { get; set; }

        public bool? SelectActiveBanners { get; set; }

        public int? PageNumber { get; set; }
    }
}