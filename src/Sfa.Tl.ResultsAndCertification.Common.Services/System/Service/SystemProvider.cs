using Sfa.Tl.ResultsAndCertification.Common.Services.System.Interface;
using System;

namespace Sfa.Tl.ResultsAndCertification.Common.Services.System.Service
{
    public class SystemProvider : ISystemProvider
    {
        public DateTime UtcToday => DateTime.UtcNow.Date;

        public DateTime Today => DateTime.Today;

        public DateTime UtcNow => DateTime.UtcNow;
    }
}