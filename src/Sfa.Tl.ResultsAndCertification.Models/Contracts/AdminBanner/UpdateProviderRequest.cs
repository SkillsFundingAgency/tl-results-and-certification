using Sfa.Tl.ResultsAndCertification.Common.Enum;
using System;

namespace Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminBanner
{
    public class UpdateBannerRequest
    {
        public int BannerId { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public BannerTarget Target { get; set; }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        public bool IsActive { get; set; }

        public string ModifiedBy { get; set; }
    }
}