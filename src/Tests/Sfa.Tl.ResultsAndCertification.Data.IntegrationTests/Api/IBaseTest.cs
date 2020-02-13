using System;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Api
{
    public interface IBaseTest<T> : IDisposable
    {
        public void Setup();
        public void Given();
        public void When();

    }
}
