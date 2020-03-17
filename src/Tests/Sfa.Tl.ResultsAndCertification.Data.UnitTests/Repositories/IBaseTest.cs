using System;

namespace Sfa.Tl.ResultsAndCertification.Data.UnitTests.Repositories
{
    public interface IBaseTest<T> : IDisposable
    {
        void Setup();
        void Given();
        void When();

    }
}
