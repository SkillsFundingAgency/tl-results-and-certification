using System;

namespace Sfa.Tl.ResultsAndCertification.Common.Services.System.Interface
{
    public interface ISystemProvider
    {
        /// <summary>
        /// Returns a DateTime representing the current UTC date.
        /// </summary>
        DateTime UtcToday { get; }
    }
}