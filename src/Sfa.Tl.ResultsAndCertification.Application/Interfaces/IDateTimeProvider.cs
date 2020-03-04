using System;
using System.Collections.Generic;
using System.Text;

namespace Sfa.Tl.ResultsAndCertification.Application.Interfaces
{
    public interface IDateTimeProvider
    {
        DateTime GetUtcNow();
        string GetUtcNowString(string format);        
    }
}
