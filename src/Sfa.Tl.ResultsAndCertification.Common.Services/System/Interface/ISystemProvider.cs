using System;

namespace Sfa.Tl.ResultsAndCertification.Common.Services.System.Interface
{
    public interface ISystemProvider
    {
        /// <summary>
        /// Returns a DateTime representing the current UTC date.
        /// </summary>
        DateTime UtcToday { get; }

        /// <summary>
        /// Returns a DateTime representing the current date.
        /// </summary>
        DateTime Today { get; }

        /// <summary>
        /// Returns a DateTime representing the current UTC date and time.
        /// </summary>
        DateTime UtcNow { get; }
    }
}