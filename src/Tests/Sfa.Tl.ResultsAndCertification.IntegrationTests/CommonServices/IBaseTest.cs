using System;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.CommonServices
{
    public interface IBaseTest : IDisposable
    {
        void Setup();
        void Given();
        void When();
    }
}
