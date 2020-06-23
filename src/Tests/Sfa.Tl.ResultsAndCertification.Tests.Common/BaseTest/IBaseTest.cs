using System;

namespace Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest
{
    public interface IBaseTest<T> : IDisposable
    {
        void Setup();
        void Given();
        void When();
    }
}
