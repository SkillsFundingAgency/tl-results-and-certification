using System;

namespace Sfa.Tl.ResultsAndCertification.Application.Interfaces
{
    public interface IDateTimeProvider
    {
        DateTime GetUtcNow();
        string GetUtcNowString(string format);        
    }
}
