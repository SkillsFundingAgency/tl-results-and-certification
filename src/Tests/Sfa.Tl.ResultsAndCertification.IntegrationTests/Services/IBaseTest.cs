using System;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services
{
    public interface IBaseTest<T> : IDisposable
    {
        void Setup();
        void Given();
        void When();
    }
}
