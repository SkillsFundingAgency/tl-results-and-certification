using System;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.CommonServices
{
    public interface IBaseTest : IDisposable
    {
        void Setup();
        void Given();
        Task When();
    }
}
