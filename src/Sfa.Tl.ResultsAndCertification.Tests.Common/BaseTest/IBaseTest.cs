using System;

namespace Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest
{
    public interface IBaseTest<T> : IDisposable
    {
        public void Setup();
        public void Given();
        public void When();

    }
}
