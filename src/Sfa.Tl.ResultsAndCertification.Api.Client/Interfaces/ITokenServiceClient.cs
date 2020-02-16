using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces
{
    public interface ITokenServiceClient
    {
        Task<string> GetToken();
    }
}
