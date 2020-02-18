using System;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services
{
    public interface IBaseTest<T> : IDisposable
    {
        public void Setup();
        public void Given();
        public void When();
    }
}
