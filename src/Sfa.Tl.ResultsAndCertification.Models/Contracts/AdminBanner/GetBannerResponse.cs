﻿using Sfa.Tl.ResultsAndCertification.Common.Enum;
using System;

namespace Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminBanner
{
    public class GetBannerResponse
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public BannerTarget Target { get; set; }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        public bool IsOptedin { get; set; }
    }
}
